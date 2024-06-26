﻿using TFG_UOC_2024.APP.Helper;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.APP.Views;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP
{
    public partial class App : Application
    {
        public static UserDTO user;

        public static List<RecipeDTO> recipes;

        private readonly IAuthService _authService;

        public App(IServiceProvider serviceProvider)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI1Mjc2NkAzMjM1MmUzMDJlMzBqSGR4amQwRUlaVzYvemt2VUh2NlZ6T05kSGpXaFVVdGQ1YXY5QzNsRVlVPQ==");
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
