﻿using System;
using System.Diagnostics;
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

			listView.ItemsSource = await TodoItemDatabase.Instance.GetItemsAsync();
		}

		async void OnItemAdded(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TodoItemPage
			{
				BindingContext = new TodoItem()
			});
		}

		async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            //((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
            //Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);
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
