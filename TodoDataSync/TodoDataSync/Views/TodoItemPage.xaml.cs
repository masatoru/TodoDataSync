using System;
using TodoDataSync.Models;
using TodoDataSync.Services;
using Xamarin.Forms;

namespace TodoDataSync.Views
{
    public partial class TodoItemPage : ContentPage
	{
		public TodoItemPage()
		{
			InitializeComponent();
		}

		async void OnSaveClicked(object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
			await TodoItemDatabase.Instance.SaveItemAsync(todoItem);
			await Navigation.PopAsync();
		}

		async void OnDeleteClicked(object sender, EventArgs e)
		{
			var todoItem = (TodoItem)BindingContext;
			await TodoItemDatabase.Instance.DeleteItemAsync(todoItem);
			await Navigation.PopAsync();
		}

		async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		//void OnSpeakClicked(object sender, EventArgs e)
		//{
		//	var todoItem = (TodoItem)BindingContext;
		//	DependencyService.Get<ITextToSpeech>().Speak(todoItem.Name + " " + todoItem.Notes);
		//}
	}
}
