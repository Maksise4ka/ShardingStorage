using ShardingStorage.Core.Grpc.Extensions;

namespace ShardingStorage.Core.Web.Extensions;

internal static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication application)
    {
        // if (application.Environment.IsDevelopment())
        // {
        //     application.UseSwagger();
        //     application.UseSwaggerUI();
        // }

        application.UseHttpsRedirection();

        application.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

        application.UseAuthorization();

        application.MapControllers();

        application.MapGrpcServices();

        return application;
    }

    public static async Task InitializeAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    }
}