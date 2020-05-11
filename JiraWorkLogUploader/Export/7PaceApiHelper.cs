using System;
using System.IO;
using System.Net.Http;
using System.Text;
using JiraWorkLogUploader.Config;
using Newtonsoft.Json;

namespace JiraWorkLogUploader.Export
{
    public static class SevenPaceApiHelper
    {
        private static HttpClient httpClient = new HttpClient();

        public static void Append7paceAuthorizationHeader(this HttpRequestMessage httpRequestMessage, ExportSettings settings)
        {
            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ApiToken);
        }

        public static Uri Get7PaceUriFor(this ExportSettings settings, string command)
        {
            var ub = new UriBuilder(settings.Url);
            ub.Path = (ub.Path ?? "").TrimEnd('/') + "/" + (command ?? "").TrimStart('/');
            ub.Query = "api-version=3.0";
            return ub.Uri;
        }

        public static int LogWork(ExportSettings settings, DateTime date, double hours, string workItemId, string description)
        {
            var uri = settings.Get7PaceUriFor("/worklogs");

            var seconds = (int)(hours * 60 * 60);
            var data = new {
                workItemd = int.Parse(workItemId),
                timestamp = date,
                length = seconds,
                comment = description
            };

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local, DateFormatString = "yyyy-MM-ddTHH:mm:ss" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content };
            request.Append7paceAuthorizationHeader(settings);

            var result = httpClient.SendAsync(request).Result;

            return (int) result.StatusCode;
        }

    }
}
