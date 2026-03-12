using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PersonalProductivityHub.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
    }
}
