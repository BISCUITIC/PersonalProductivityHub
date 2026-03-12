namespace PersonalProductivityHub.Extensions;

public static class SwaggerExteensions
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}
