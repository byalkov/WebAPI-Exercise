using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace Strata_WebAPI_Exercise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}