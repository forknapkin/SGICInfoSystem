using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGICInfoSystem.Startup))]
namespace SGICInfoSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
