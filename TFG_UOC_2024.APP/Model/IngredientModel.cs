using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.Model
{
    public class IngredientModel : INotifyPropertyChanged
    {
        private string id;

        private string name;

        private string imageUrl;

        private string quantity;

        private string categoryName;

        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
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

        public string Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    RaisedOnPropertyChanged("Quantity");
                }
            }
        }

        public string CategoryName
        {
            get { return CategoryName; }
            set
            {
                if (CategoryName != value)
                {
                    CategoryName = value;
                    RaisedOnPropertyChanged("CategoryName");
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
