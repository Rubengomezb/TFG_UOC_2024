using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Helper
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider) =>
            Services = serviceProvider;

        public static T GetService<T>() => Services.GetService<T>();
    }
}
