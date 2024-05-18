using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class NutricionalInfoModel
    {
        private double? calories;
        private double? proteins;
        private double? carbohydrates;
        private double? fats;

        public double? Calories
        {
            get { return calories; }
            set { calories = value; }
        }

        public double? Proteins
        {
            get { return proteins; }
            set { proteins = value; }
        }

        public double? Carbohydrates
        {
            get { return carbohydrates; }
            set { carbohydrates = value; }
        }

        public double? Fats
        {
            get { return fats; }
            set { fats = value; }
        }
    }
}
