using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLCAOOC.Startup))]
namespace QLCAOOC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
