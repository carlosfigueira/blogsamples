using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAzure.MobileServices;

namespace WPFQuickStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://YOUR-SERVICE-HERE.azure-mobile.net/",
            "YOUR-KEY-HERE"
        );
    }
}
