using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class AdvancedEventModel : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _description;
        private DateTime _starting;
        private Brush _background;

        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public DateTime Starting
        {
            get => _starting;
            set
            {
                _starting = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Starting)));
            }
        }

        public Brush Background
        {
            get => _background;
            set
            {
                _background = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Background)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
