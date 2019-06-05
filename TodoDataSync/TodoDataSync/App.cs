﻿using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Push;
using TodoDataSync.Models;
using TodoDataSync.Views;
using Xamarin.Forms;

namespace TodoDataSync
{
    public class App : Application
    {
        public App()
        {
            Resources = new ResourceDictionary();
            Resources.Add("primaryGreen", Color.FromHex("91CA47"));
            Resources.Add("primaryDarkGreen", Color.FromHex("6FA22E"));

            var nav = new NavigationPage(new TodoListPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            nav.BarTextColor = Color.White;

            MainPage = nav;
        }

        protected override void OnStart()
        {
            // Setup AppCenter
            AppCenter.Start($"android={AppCenterConfiguration.Android};" +
                            $"ios={AppCenterConfiguration.iOS}",
                typeof(Data),
                typeof(Auth),
                typeof(Analytics),
                typeof(Crashes),
                typeof(Push));
        }
    }
}
