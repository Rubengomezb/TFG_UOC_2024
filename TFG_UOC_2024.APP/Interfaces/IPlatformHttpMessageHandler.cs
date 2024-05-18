using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Interfaces
{
    public interface IPlatformHttpMessageHandler
    {
        HttpMessageHandler GetHttpMessageHandler();
    }
}
