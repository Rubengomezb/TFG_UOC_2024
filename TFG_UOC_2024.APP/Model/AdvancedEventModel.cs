using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class AdvancedEventModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Starting { get; set; }
        public Brush Background { get; set; }
    }
}
