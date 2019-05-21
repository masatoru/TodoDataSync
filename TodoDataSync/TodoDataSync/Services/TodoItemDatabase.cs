using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Data;
using TodoDataSync.Models;

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
            try
            {
                var result = await Data.ListAsync<TodoItem>(DefaultPartitions.UserDocuments);
                if (result.CurrentPage == null)
                {
                    return list;
                }

                list.AddRange(result.CurrentPage.Items.Select(x => x.DeserializedValue));
                while (result.HasNextPage)
                {
                    await result.GetNextPageAsync();
                    list.AddRange(result.CurrentPage.Items.Select(x => x.DeserializedValue));
                }
            }
            catch (Exception ex)
            {
                await (App.Current as App).MainPage.DisplayAlert("Error", ex.Message, "Close");
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
                if (string.IsNullOrEmpty(item.Id.ToString()))
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
                await (App.Current as App).MainPage.DisplayAlert("Error", ex.Message, "Close");
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

