using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.CORE.Components
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int TokenExpirationInDays { get; set; }
    }
}
