using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TheCurseOfKnowledge.Desktop.DevEx.Extensions;

namespace TheCurseOfKnowledge.Desktop.DevEx
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                WindowsFormsSettings.ForceDirectXPaint();
                WindowsFormsSettings.SetDPIAware();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainForm = services.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
        }
        static IHostBuilder CreateHostBuilder()
            => Host.CreateDefaultBuilder()
            .ConfigureServices((context, services)
                =>
            {
                // Register Views
                var elements = Assembly.GetExecutingAssembly()
                    .GetViewControlAttribute();
                foreach (var element in elements)
                    services.AddKeyedTransient(typeof(XtraUserControl), element.Key, (Type)element.State);

                //// Register Repositories
                //services.AddSingleton<ITradeRepository, TradeRepository>();

                //// Register Controllers
                //services.AddTransient<MainController>();

                // Register Forms & UserControls
                services.AddTransient<MainForm>(); // Form Utama
            });
    }
}
