using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Model;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.CORE.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class MenuViewModel : INotifyPropertyChanged
    {
        //[ObservableProperty]
        //private string _name;

        //[ObservableProperty]
        //private string _description;

        //[ObservableProperty]
        //private List<MenuDTO> _events;

        //[ObservableProperty]
        //private DateTime _menuDate;

        //private bool isVisible = false;

        private readonly IMenuService _menuService;
        //public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
            this.subjects = new List<string>();
            this.colors = new List<Brush>();
            this.notes = new List<string>();
            this.selectedDateMenus = new ObservableCollection<AdvancedEventModel>();
            this.CreateColors();
            this.IntializeAppoitments();
            //this.selectedDateMenus = this.GetSelectedDateAppointments(this.selectedDate);
            this.DisplayDate = DateTime.Now.Date;
            IsOpenCommand = new Command(OpenCommand);
        }

        public ICommand IsOpenCommand { private set; get; }

        public bool IsOpen { get; set;  }

        private void OpenCommand(object obj)
        {
            IsOpen = !IsOpen;
        }

        /*[RelayCommand]
        private async Task DatePickedCommand(DateTime date)
        {
            var monday = GetFirstWeekDay(date);
            var sunday = monday.AddDays(7);
            await _menuService.CreateWeeklyMenuAsync(monday, sunday);
            isVisible = false;
        }*/

        private DateTime GetFirstWeekDay(DateTime dateSelected)
        {
            return DateTime.Today.AddDays(-(int)dateSelected.DayOfWeek + (int)DayOfWeek.Monday);
        }

        /// <summary>
        /// team management
        /// </summary>
        private List<string> subjects;

        /// <summary>
        /// Notes Collection.
        /// </summary>
        private List<string> notes;

        /// <summary>
        /// color collection
        /// </summary>
        private List<Brush> colors;

        /// <summary>
        /// The selected date
        /// </summary>
        private DateTime selectedDate = DateTime.Now.Date;

        /// <summary>
        /// The bool value.
        /// </summary>
        private bool isToday = true;

        /// <summary>
        /// The date text color.
        /// </summary>
        private Color dateTextColor = Colors.White;

        /// <summary>
        /// The selected date meetings.
        /// </summary>
        public ObservableCollection<AdvancedEventModel>? selectedDateMenus;

        /// <summary>
        /// Gets or sets appointments.
        /// </summary>
        public ObservableCollection<AdvancedEventModel>? Events { get; set; }

        /// <summary>
        /// Gets or sets the selected date meetings.
        /// </summary>
        public ObservableCollection<AdvancedEventModel>? SelectedDateMeetings
        {
            get { return selectedDateMenus; }
            set
            {
                selectedDateMenus = value;
                RaiseOnPropertyChanged("SelectedDateMeetings");
            }
        }

        /// <summary>
        /// Gets or sets the schedule selected date.
        /// </summary>
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                RaiseOnPropertyChanged("SelectedDate");
            }
        }

        /// <summary>
        /// Gets or sets the date is today or not.
        /// </summary>
        public bool IsToday
        {
            get { return isToday; }
            set
            {
                isToday = value;
                RaiseOnPropertyChanged("IsToday");
            }
        }

        /// <summary>
        /// Gets or sets the date text color.
        /// </summary>
        public Color DateTextColor
        {
            get { return dateTextColor; }
            set
            {
                dateTextColor = value;
                RaiseOnPropertyChanged("DateTextColor");
            }
        }

        private async void IntializeAppoitments()
        {
            var rand = new Random();
            this.Events = new ObservableCollection<AdvancedEventModel>();

            var actualDate = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(actualDate.Year, actualDate.Month, 1);
            var lastDayOfMonth = new DateTime(actualDate.Year, actualDate.Month, DateTime.DaysInMonth(actualDate.Year, actualDate.Month));

            /*var menuList = await _menuService.GetWeeklyMenuAsync(firstDayOfMonth, lastDayOfMonth);

            foreach (var menu in menuList)
            {
                foreach (var item in Enum.GetValues(typeof(EatTime)))
                {
                    var meeting = new AdvancedEventModel();
                    meeting.Starting = menu.Date;
                    meeting.Name = menu.Recipe.Name;
                    meeting.Id = menu.Recipe.Id;
                    meeting.Background = this.colors[rand.Next(this.colors.Count())];

                    switch (item)
                    {
                        case EatTime.Breakfast:
                            meeting.Starting = menu.Date.AddHours(8);
                            break;
                        case EatTime.Lunch:
                            meeting.Starting = menu.Date.AddHours(13);
                            break;
                        case EatTime.Dinner:
                            meeting.Starting = menu.Date.AddHours(20);
                            break;
                    }

                    this.Events.Add(meeting);
                }
            }*/
        }

        /// <summary>
        /// Gets or sets the schedule display date.
        /// </summary>
        public DateTime DisplayDate { get; set; }

        public ObservableCollection<AdvancedEventModel> GetSelectedDateAppointments(DateTime date)
        {
            var selectedAppiointments = new ObservableCollection<AdvancedEventModel>();

            for (int i = 0; i < this.Events?.Count; i++)
            {
                DateTime startTime = this.Events[i].Starting;

                if (date.Day == startTime.Day && date.Month == startTime.Month && date.Year == startTime.Year)
                {
                    selectedAppiointments.Add(this.Events[i]);
                }
            }

            return selectedAppiointments;
        }

        private void CreateColors()
        {
            this.colors.AddRange(new List<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#FFA2C139")),   
                new SolidColorBrush(Color.FromArgb("#FF8B1FA9")),
                new SolidColorBrush(Color.FromArgb("#FFD20100")),
                new SolidColorBrush(Color.FromArgb("#FFFC571D")),
                new SolidColorBrush(Color.FromArgb("#FF36B37B")),
                new SolidColorBrush(Color.FromArgb("#FF3D4FB5")),
                new SolidColorBrush(Color.FromArgb("#FFE47C73")),
                new SolidColorBrush(Color.FromArgb("#FF636363")),
                new SolidColorBrush(Color.FromArgb("#FF85461E")),
                new SolidColorBrush(Color.FromArgb("#FF0F8644")),
                new SolidColorBrush(Color.FromArgb("#FF01A1EF"))
            });
        }

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
