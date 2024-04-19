﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.DB.Components
{
    public class Enums
    {
        public enum Comparison
        {
            GreaterThan,
            GreaterThanOrEqual,
            Equal,
            LessThan,
            LessThanOrEqual,
            NotEqual
        }

        public enum ServiceStatus
        {
            Ok,
            BadRequest,
            NotFound
        }

        public enum EatTime
        {
            Breakfast,
            Lunch,
            Dinner
        }
    }
}
