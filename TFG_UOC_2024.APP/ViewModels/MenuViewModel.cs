using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private List<MenuDTO> _events;

        [ObservableProperty]
        private DateTime _menuDate;

        [ObservableProperty]
        private bool isVisible = false;

        private readonly IMenuService _menuService;
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ICommand VisibleCommand => new Command(AddWeeklyMenuCommand);

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible == value) return;
                isVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isVisible)));
            }
        }

        private void AddWeeklyMenuCommand(object obj)
        {
            isVisible = true;
        }

        [RelayCommand]
        private async Task DatePickedCommand(DateTime date)
        {
            var monday = GetFirstWeekDay(date);
            var sunday = monday.AddDays(7);
            await _menuService.CreateWeeklyMenuAsync(monday, sunday);
            isVisible = false;
        }

        private DateTime GetFirstWeekDay(DateTime dateSelected)
        {
            return DateTime.Today.AddDays(-(int)dateSelected.DayOfWeek + (int)DayOfWeek.Monday);
        }


    }
}
