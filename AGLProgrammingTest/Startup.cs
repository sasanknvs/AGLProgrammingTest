using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AGLProgrammingTest.Startup))]
namespace AGLProgrammingTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
