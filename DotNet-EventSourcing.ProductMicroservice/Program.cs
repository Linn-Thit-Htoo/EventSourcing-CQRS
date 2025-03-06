using DotNet_EventSourcing.ProductMicroservice.Dependencies;

namespace DotNet_EventSourcing.ProductMicroservice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDependencies(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseHealthChecks("/health");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
