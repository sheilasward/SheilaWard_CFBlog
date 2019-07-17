using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SheilaWard_CFBlog.Startup))]
namespace SheilaWard_CFBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
