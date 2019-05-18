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
            var result = await Data.ListAsync<TodoItem>(DefaultPartitions.UserDocuments);
            list.AddRange(result.CurrentPage.Items.Select(x => x.DeserializedValue));
            while (result.HasNextPage)
            {
                await result.GetNextPageAsync();
                list.AddRange(result.CurrentPage.Items.Select(x => x.DeserializedValue));
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
            if (string.IsNullOrEmpty(item.Id.ToString()))
            {
                throw new NotImplementedException();
            }
            else
            {
            await Data.CreateAsync(item.Id.ToString(), item, DefaultPartitions.UserDocuments);
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

