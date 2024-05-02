using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class CategoryModel : INotifyPropertyChanged
    {
        private string id;

        private string name;

        private string imageUrl;

        public string Id {
            get { return id; } 
            set { if (id != value)
                {
                    id = value;
                    RaisedOnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisedOnPropertyChanged("Name");
                }
            }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    RaisedOnPropertyChanged("ImageUrl");
                }
            }
        }

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
