using System;
using System.Net;
using System.Net.Http;
using System.Text;
using JiraWorkLogUploader.Config;
using Newtonsoft.Json;

namespace JiraWorkLogUploader.Jira
{
    public class JiraApiHelper
    {
        private static HttpClient httpClient = new HttpClient();

        public static void Login(JiraSetting jira)
        {
            var uri = new Uri(new Uri(jira.Url), "/rest/auth/1/session");
            var data = new { username = jira.User, password = jira.Password };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = httpClient.SendAsync(new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content }).Result;
            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception("Login exception: " + result.ReasonPhrase);
        }

        public static int LogWork(JiraSetting jira, DateTime date, double hours, string issue, string description)
        {
            // NOTE: thought this will work...but doesn't...
            // var alteredDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);

            // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone branch of the registry.
            var destinationTimezone = TimeZoneInfo.FindSystemTimeZoneById(jira.Timezone);
            var alteredDate = TimeZoneInfo.ConvertTime(date, destinationTimezone, TimeZoneInfo.Local);

            var uri = new Uri(new Uri(jira.Url), "/rest/api/2/issue/" + Uri.EscapeUriString(issue) + "/worklog");
            var seconds = (int)(hours * 60 * 60);
            var data = new { started = alteredDate, timeSpentSeconds = seconds, comment = description };

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local, DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffzz00" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = httpClient.SendAsync(new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content }).Result;

            //if (result.StatusCode != HttpStatusCode.Created)
            //    throw new Exception("Work log exception: " + result.ReasonPhrase);

            return (int)result.StatusCode;
        }
    }
}
