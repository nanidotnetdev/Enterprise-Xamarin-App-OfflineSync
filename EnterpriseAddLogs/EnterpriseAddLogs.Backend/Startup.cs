using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EnterpriseAddLogs.Backend.Startup))]
namespace EnterpriseAddLogs.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}