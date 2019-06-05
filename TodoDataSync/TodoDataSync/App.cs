using System.Threading.Tasks;
using Microsoft.AppCenter;
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

        protected override async void OnStart()
        {

            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += this.Push_PushNotificationReceived;
            }

            // Setup AppCenter
            AppCenter.Start($"android={AppCenterConfiguration.Android};" +
                            $"ios={AppCenterConfiguration.iOS}",
                typeof(Data),
                typeof(Auth),
                typeof(Analytics),
                typeof(Crashes),
                typeof(Push));
        }

        private async void Push_PushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            // Add the notification message and title to the message
            var summary = $"Push notification received:" +
                          $"\n\tNotification title: {e.Title}" +
                          $"\n\tMessage: {e.Message}";

            // If there is custom data associated with the notification,
            // print the entries
            if (e.CustomData != null)
            {
                summary += "\n\tCustom data:\n";
                foreach (var key in e.CustomData.Keys)
                {
                    summary += $"\t\t{key} : {e.CustomData[key]}\n";
                }
            }

            // Send the notification summary to debug output
            await Application.Current.MainPage.DisplayAlert("Message from Push", summary, "ok");
        }
    }
}
