﻿using Android.App;
using Android.Runtime;

namespace TFG_UOC_2024.APP
{
#if DEBUG
    [Application(AllowBackup = false, Debuggable = true, UsesCleartextTraffic = true)]
#else
[Application]
#endif
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
