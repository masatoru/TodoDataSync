using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Crashes;
using TodoDataSync.Models;
using TodoDataSync.Services;
using Xamarin.Forms;

namespace TodoDataSync.Views
{
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get a list when Appearing
        /// </summary>
		protected override async void OnAppearing()
        {
            base.OnAppearing();

            await UpdateList();
        }

        private async Task UpdateList()
        {
            try
            {
                listView.ItemsSource = await TodoItemDatabase.Instance.GetItemsAsync();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                BindingContext = new TodoItem()
            });
        }

        void OnLogout(object sender, EventArgs e)
        {
            Auth.SignOut();
        }

        async void OnLogin(object sender, EventArgs e)
        {
            try
            {
                var user = await Auth.SignInAsync();

                Analytics.TrackEvent("OnLogin", new Dictionary<string, string>
                {
                    ["User"] = user.AccountId.Substring(0, 10),
                });

                await DisplayAlert("User", $"{user.AccountId.Substring(0, 10)}", "Close");

                await UpdateList();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("OnLogin", new Dictionary<string, string>
                {
                    ["Error"] = ex.Message,
                });
                Crashes.TrackError(ex);

                await DisplayAlert("Error", ex.Message, "Close");
            }
        }

        async void OnCrash(object sender, EventArgs e)
        {
            Crashes.GenerateTestCrash();
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TodoItemPage
                {
                    BindingContext = e.SelectedItem as TodoItem
                });
            }
        }
    }
}
