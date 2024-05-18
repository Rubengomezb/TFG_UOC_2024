﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.DataForm;

namespace TFG_UOC_2024.APP.Model
{
    public class SignUpModel : IDataErrorInfo, INotifyDataErrorInfo
    {
        [Display(Prompt = "Enter your first name", Name = "First name")]
        [DataFormDisplayOptions(ColumnSpan = 2)]
        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(20, ErrorMessage = "First name should not exceed 20 characters")]
        public string FirstName { get; set; }

        [Display(Prompt = "Enter your last name", Name = "Last name")]
        [DataFormDisplayOptions(ColumnSpan = 2)]
        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(20, ErrorMessage = "First name should not exceed 20 characters")]
        public string LastName { get; set; }

        [Display(Prompt = "Enter your email", Name = "Email")]
        [DataFormDisplayOptions(ColumnSpan = 2)]
        [EmailAddress(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [DataFormDisplayOptions(ColumnSpan = 2)]
        [Display(Prompt = "Enter your mobile number", Name = "Mobile number")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Please enter a valid number")]
        public double? MobileNumber { get; set; }

        [Display(Prompt = "Enter your password", Name = "Password")]
        [DataType(DataType.Password)]
        [DataFormDisplayOptions(ColumnSpan = 2, ValidMessage = "Password strength is good")]
        [Required(ErrorMessage = "Please enter the password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])[a-zA-Z\d]{8,}$", ErrorMessage = "A minimum 8-character password should contain a combination of uppercase and lowercase letters.")]
        public string Password { get; set; }

        [Display(Prompt = "Confirm password", Name = "Re-enter Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter the password")]
        [DataFormDisplayOptions(ColumnSpan = 2)]
        public string RetypePassword { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Prompt = "Enter your address", Name = "Address")]
        [DataFormDisplayOptions(ColumnSpan = 2, RowSpan = 2)]
        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        [Display(Prompt = "Enter your city", Name = "City")]
        [Required(ErrorMessage = "Please enter your city")]
        public string City { get; set; }

        [Display(Prompt = "Enter your state", Name = "State")]
        [Required(ErrorMessage = "Please enter your state")]
        public string State { get; set; }

        [Display(Prompt = "Enter your country", Name = "Country")]
        public string Country { get; set; }

        [Display(Prompt = "Enter zip code", Name = "Zip code")]
        [Required(ErrorMessage = "Please enter your zip code")]
        public double? ZipCode { get; set; }

        [Display(AutoGenerateField = false)]
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        [Display(AutoGenerateField = false)]
        public string this[string name]
        {
            get
            {
                string result = string.Empty;
                if (name == nameof(RetypePassword) && this.Password != this.RetypePassword)
                {
                    result = string.IsNullOrEmpty(this.RetypePassword) ? string.Empty : "The passwords do not match";
                }

                return result;
            }
        }

        [Display(AutoGenerateField = false)]
        public bool HasErrors
        {
            get
            {
                return false;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [Display(AutoGenerateField = false)]
        public IEnumerable GetErrors(string propertyName)
        {
            var list = new List<string>();
            if (propertyName.Equals("Country") && string.IsNullOrEmpty(this.Country))
                list.Add("Please select your country");
            return list;
        }
    }
}
