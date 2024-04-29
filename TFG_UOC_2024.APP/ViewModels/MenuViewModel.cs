using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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

        private readonly IMenuService _menuService;

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

    }
}
