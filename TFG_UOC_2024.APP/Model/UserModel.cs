using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class UserModel : INotifyPropertyChanged
    {
        private string id;
        private string firstname;
        private string lastname;
        private string email;
        private string phone;
        private string imageurl;

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

        public string Firstname
        {
            get { return firstname; }
            set
            {
                if (firstname != value)
                {
                    firstname = value;
                    RaisedOnPropertyChanged("Firstname");
                }
            }
        }

        public string Lastname
        {
            get { return lastname; }
            set
            {
                if (lastname != value)
                {
                    lastname = value;
                    RaisedOnPropertyChanged("Lastname");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    RaisedOnPropertyChanged("Email");
                }
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    RaisedOnPropertyChanged("Phone");
                }
            }
        }

        public string ImageUrl
        {
            get { return imageurl; }
            set
            {
                if (imageurl != value)
                {
                    imageurl = value;
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
