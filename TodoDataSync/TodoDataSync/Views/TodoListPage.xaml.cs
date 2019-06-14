using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter;
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

            await SignIn();

            await UpdateList();
        }

        async Task<UserInformation> SignIn()
        {
            var user = (Application.Current as App)?.UserInfo;
            try
            {
                user = await Auth.SignInAsync();
            }
            catch (Exception e)
            {
                // Do something with sign-in failure.
            }

            return user;
        }


        private async Task UpdateList()
        {
            this.IsBusy = true;
            try
            {
                listView.ItemsSource = await TodoItemDatabase.Instance.GetItemsAsync();
            }
            catch (Exception ex)
            {
                // do nothing
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                BindingContext = new TodoItem()
            });
        }

        async void OnLogout(object sender, EventArgs e)
        {
            Auth.SignOut();

            await DisplayAlert("Logout", "ログアウトしました", "OK");

            listView.ItemsSource = new List<TodoItem>();

        }

        async void OnLogin(object sender, EventArgs e)
        {
            try
            {
                var user = await SignIn();

                Analytics.TrackEvent("OnLogin", new Dictionary<string, string>
                {
                    ["User"] = user.AccountId.Substring(0, 10),
                });

                await DisplayAlert("Login", $"ログインしました User={user.AccountId.Substring(0, user.AccountId.Length / 2)}...", "Close");

                await UpdateList();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("OnLogin", new Dictionary<string, string>
                {
                    ["Error"] = ex.Message,
                });
                Crashes.TrackError(ex);

                await DisplayAlert("Error", ex.Message, "OK");
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
