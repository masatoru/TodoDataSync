using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Data;
using TodoDataSync.Models;
using Xamarin.Forms.Internals;

namespace TodoDataSync.Services
{
    public class TodoItemDatabase
    {
        public static TodoItemDatabase Instance { get; } = new TodoItemDatabase();

        /// <summary>
        /// Get a list from database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TodoItem>> GetItemsAsync()
        {
            var list = new List<TodoItem>();
            var result = await Data.ListAsync<TodoItem>(DefaultPartitions.UserDocuments);
            foreach (var item in result)
            {
                list.Add(item.DeserializedValue);
            }
            return list;
        }

        /// <summary>
        /// Get a list database for not done.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            var list = await GetItemsAsync();
            return list.Where(m => !m.Done).ToList();
        }

        /// <summary>
        /// Get an item from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TodoItem> GetItemAsync(string id)
        {
            var list = await GetItemsAsync();
            return list.Where(i => i.Id.ToString() == id).FirstOrDefault();
        }

        /// <summary>
        /// Add or save a item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task SaveItemAsync(TodoItem item)
        {
            try
            {
                // new item if id is null.
                if ( Guid.Empty==item.Id)
                {
                    item.Id = Guid.NewGuid();
                    await Data.CreateAsync(item.Id.ToString(), item, DefaultPartitions.UserDocuments, new WriteOptions(TimeToLive.Infinite));
                }
                else
                {
                    await Data.ReplaceAsync(item.Id.ToString(), item, DefaultPartitions.UserDocuments, new WriteOptions(TimeToLive.Infinite));
                }
            }
            catch (Exception ex)
            {
                await (App.Current as App).MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Delete an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<string> DeleteItemAsync(TodoItem item)
        {
            throw new NotImplementedException();
        }
    }
}

