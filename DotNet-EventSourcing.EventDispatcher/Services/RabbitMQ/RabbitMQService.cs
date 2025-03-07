namespace DotNet_EventSourcing.EventDispatcher.Services.RabbitMQ;

public class RabbitMQService : BackgroundService
{
    private readonly AppSetting _appSetting;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(
        IOptions<AppSetting> options,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMQService> logger
    )
    {
        _appSetting = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IConnection connection = CreateConnection();
        var channel = connection.CreateModel();

        foreach (var item in _appSetting.RabbitMQ.QueueList)
        {
            channel.ExchangeDeclare(item.Exchange, ExchangeType.Direct, true, false, null);
            channel.QueueDeclare(item.Queue, true, false, false);
            channel.QueueBind(item.Queue, item.Exchange, item.RoutingKey, null);
            channel.BasicQos(0, 1, false);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                if (ea.RoutingKey.Equals("eventdirect"))
                {
                    var requestModel = JsonConvert.DeserializeObject<DomainEvent>(content);
                    await HandleEventSourcing(requestModel!);
                }

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(item.Queue, false, consumer);
        }
    }

    private IConnection CreateConnection()
    {
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = _appSetting.RabbitMQ.HostName,
            UserName = _appSetting.RabbitMQ.UserName,
            Password = _appSetting.RabbitMQ.Password,
            VirtualHost = "/"
        };
        connectionFactory.AutomaticRecoveryEnabled = true;
        connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
        connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(15);
        connectionFactory.DispatchConsumersAsync = true;

        return connectionFactory.CreateConnection();
    }

    private async Task HandleEventSourcing(DomainEvent @event)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var eventStream = await dbContext
                .TblEventStreams.Where(x => x.StreamId == @event.StreamId)
                .SingleOrDefaultAsync();

            if (eventStream is null)
            {
                TblEventStream tblEventStream = new TblEventStream()
                {
                    StreamId = @event.StreamId,
                    AggregateType = @event.AggregateType,
                    CreatedAt = DateTime.Now,
                };
                await dbContext.TblEventStreams.AddAsync(tblEventStream);
            }

            var latestVersionEvent = await dbContext
                .TblEvents.OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync();
            TblEvent tblEvent = new TblEvent();

            if (latestVersionEvent is null)
            {
                tblEvent = new TblEvent()
                {
                    StreamId = @event.StreamId,
                    Version = 1,
                    CreatedAt = DateTime.Now,
                    EventData = @event.EventData,
                    EventId = Guid.NewGuid(),
                    EventType = @event.EventType,
                };
                await dbContext.TblEvents.AddAsync(tblEvent);
            }
            else
            {
                tblEvent = new TblEvent()
                {
                    StreamId = @event.StreamId,
                    Version = latestVersionEvent.Version + 1,
                    CreatedAt = DateTime.Now,
                    EventData = @event.EventData,
                    EventId = Guid.NewGuid(),
                    EventType = @event.EventType,
                };
                await dbContext.TblEvents.AddAsync(tblEvent);
            }

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
