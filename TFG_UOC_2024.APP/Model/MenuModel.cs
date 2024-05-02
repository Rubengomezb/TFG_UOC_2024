using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TFG_UOC_2024.DB.Components.Enums;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.Model
{
    public class MenuModel : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public EatTime EatTime { get; set; }

        public string userId { get; set; }

        public RecipeModel Recipe { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
    }
}
