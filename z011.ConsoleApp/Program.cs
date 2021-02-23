// <copyright file="Program.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ConsoleApp
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Main program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <returns>Task.</returns>
        internal static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<MainApp>();
                }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                // Run the main application.
                var rootApp = services.GetRequiredService<MainApp>();
                await rootApp.Run();
            }
        }
    }
}