using DotNet_EventSourcing.Gateway.Dependencies;
using Ocelot.Middleware;

namespace DotNet_EventSourcing.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDependencies(builder);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseHealthChecks("/health");

        app.UseOcelot();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
