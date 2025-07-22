using Microsoft.Extensions.Configuration;
using System.IO;

namespace DangLeAnhTuanWPF.Helpers
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; }
        static AppConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
        public static string AdminEmail => Configuration["Admin:Email"];
        public static string AdminPassword => Configuration["Admin:Password"];
        public static string ConnectionString => Configuration.GetConnectionString("DefaultConnection");
    }
} 