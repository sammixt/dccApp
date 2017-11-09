using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(com.dcc.Startup))]
namespace com.dcc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
