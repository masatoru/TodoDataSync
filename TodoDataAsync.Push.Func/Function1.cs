using System;
using System.IO;
using System.Threading.Tasks;
using Azure.WebJobs.Extensions.AppCenterPush;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TodoDataAsync.Push.Func
{
    public static class PushFunction
    {
        // Use IAsyncCollector<T>
        [FunctionName("Push")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [AppCenterPush(OwnerName = "<Your Owner Name>", AppName = "<Your App Name>")] IAsyncCollector<AppCenterPushMessage> collector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var title = data?.title;
            var body = data?.body;

            if (title == null || body == null)
            {
                return new BadRequestResult();
            }

            await collector.AddAsync(new AppCenterPushMessage
            {
                Content = new AppCenterPushContent
                {
                    Name = "pushtest",
                    Title = title,
                    Body = body
                }
            });

            return new OkResult();
        }
    }
}
