using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Scheduler;
using TFG_UOC_2024.APP.Model;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class MenuViewModel : INotifyPropertyChanged
    {
        private readonly IMenuService _menuService;
        private ListView listView; 

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
            this.subjects = new List<string>();
            this.colors = new List<Brush>();
            this.notes = new List<string>();
            this.CreateColors();
            this.IntializeAppoitments();
            this.DisplayDate = DateTime.Now.Date;
            _isOpenCommand = new Command<object>(OpenCommand);
            _addMenuCommand = new Command<object>(AddMenu);
        }

        public Command<object> _isOpenCommand;
        public Command<object> IsOpenCommand
        {
            get { return _isOpenCommand; }
            set { _isOpenCommand = value; }
 
        }

        public Command<object> _addMenuCommand;
        public Command<object> AddMenuCommand
        {
            get { return _addMenuCommand; }
            set { _addMenuCommand = value; }

        }

        private bool _isOpen { get; set;  }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisible { get; set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public IMenuService getServiceInstance()
        {
            return _menuService;
        }

        private async void OpenCommand(object obj)
        {
            if (obj is AdvancedEventModel)
            {
                await Shell.Current.GoToAsync($"{nameof(RecipeDetail)}?Id={((AdvancedEventModel)obj).Id}");
            }
            //IsOpen = true;
        }

        private async void AddMenu(object obj)
        {
            IsOpen = true;
            await _menuService.CreateWeeklyMenuAsync(this.selectedDate, this.selectedDate);
            var newRecipes = await _menuService.GetWeeklyMenuAsync(this.selectedDate, this.selectedDate);
            foreach (var re in newRecipes)
            {
                this._events.Add(this.ParseMenuResponse(re));
            }

            var visiblesDates = new List<DateTime>();
            visiblesDates.Add(this.selectedDate);
            this.QueryAppointments(visiblesDates);
            this._selectedDateMenus = this.GetSelectedDateAppointments(this.selectedDate);
        }

        public async void QueryAppointments(List<DateTime> visibleDates)
        {
            var newRecipes = await _menuService.GetWeeklyMenuAsync(visibleDates.First(), visibleDates.Last());
            foreach (var re in newRecipes)
            {
                this._events.Add(this.ParseMenuResponse(re));
            }
        }

        public AdvancedEventModel ParseMenuResponse(MenuDTO menu)
        {
            var recipe = new AdvancedEventModel()
            {
                Starting = menu.Date,
                Name = menu.Recipe.Name,
                Id = menu.Recipe.Id,
                Background = this.colors[new Random().Next(this.colors.Count())]
            };

            switch (menu.EatTime)
            {
                case EatTime.Breakfast:
                    recipe.Starting = menu.Date.AddHours(8);
                    break;
                case EatTime.Lunch:
                    recipe.Starting = menu.Date.AddHours(13);
                    break;
                case EatTime.Dinner:
                    recipe.Starting = menu.Date.AddHours(20);
                    break;
            }

            return recipe;
        }

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
        private ObservableCollection<AdvancedEventModel> _selectedDateMenus = new();

        public ObservableCollection<AdvancedEventModel> SelectedDateMenus
        {
            get { return _selectedDateMenus; }
            set
            {
                _selectedDateMenus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets appointments.
        /// </summary>
        private ObservableCollection<AdvancedEventModel> _events = new ObservableCollection<AdvancedEventModel>();
        public ObservableCollection<AdvancedEventModel> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged();
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


            var menuList = await _menuService.GetWeeklyMenuAsync(firstDayOfMonth, lastDayOfMonth);

            foreach (var menu in menuList)
            {
                var meeting = new AdvancedEventModel();
                meeting.Starting = menu.Date;
                meeting.Name = menu.Recipe.Name;
                meeting.Id = menu.Recipe.Id;
                meeting.Background = this.colors[rand.Next(this.colors.Count())];

                switch (menu.EatTime)
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

                this._events.Add(meeting);
            }
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
                    if (!selectedAppiointments.Any(x => x.Id == this.Events[i].Id || x.Name == this.Events[i].Name))
                    {
                        selectedAppiointments.Add(this.Events[i]);
                    } 
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> CreateWeeklyMenuAsync(DateTime a, DateTime b)
        {
            return await _menuService.CreateWeeklyMenuAsync(a, b);
        }

        public async Task<bool> GetWeeklyMenuAsync(DateTime a, DateTime b)
        {
            return await _menuService.CreateWeeklyMenuAsync(a, b);
        }
    }
}
