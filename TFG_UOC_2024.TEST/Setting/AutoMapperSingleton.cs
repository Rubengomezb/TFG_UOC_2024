using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.TEST.Setting
{
    using AutoMapper;
    using TFG_UOC_2024.CORE.Helpers;

    public static class AutomapperSingleton
    {
        private static IMapper _mapper;
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
                    _mapper = mappingConfig.CreateMapper();
                }
                return _mapper;
            }
        }
    }
}
