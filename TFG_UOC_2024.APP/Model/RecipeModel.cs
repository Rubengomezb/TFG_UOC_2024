using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TFG_UOC_2024.APP.Model
{
    public class RecipeModel : INotifyPropertyChanged
    {
        private string id;

        private string name;

        private string description;

        private string imageUrl;

        private List<string> ingredients;

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

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisedOnPropertyChanged("Description");
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

        public List<string> Ingredients
        {
            get { return ingredients; }
            set
            {
                if (ingredients != value)
                {
                    ingredients = value;
                    RaisedOnPropertyChanged("Ingredients");
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
