using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using JiraWorkLogUploader.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace JiraWorkLogUploader.Export
{
    public static class JiraApiHelper
    {
        private static HttpClient httpClient = new HttpClient();

        public static void Login(ExportSettings jira)
        {
            // Bypassing it...
            return;

            var uri = new Uri(new Uri(jira.Url), "/rest/auth/1/session");
            var data = new { username = jira.User, password = jira.Password };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = httpClient.SendAsync(new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content }).Result;
            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception("Login exception: " + result.ReasonPhrase);
        }

        public static void AppendAuthorizationHeader(this HttpRequestMessage httpRequestMessage, ExportSettings jira)
        {
            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{jira.UserEmail}:{jira.ApiToken}")));
        }

        public static int LogWork(ExportSettings jira, DateTime date, double hours, string issue, string description)
        {
            // NOTE: thought this will work...but doesn't...
            // var alteredDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);

            // HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zone branch of the registry.
            var jiraTimezone = TimeZoneInfo.FindSystemTimeZoneById(jira.Timezone);
            var alteredDate = TimeZoneInfo.ConvertTime(date, jiraTimezone, TimeZoneInfo.Local);

            var uri = new Uri(new Uri(jira.Url), "/rest/api/latest/issue/" + Uri.EscapeUriString(issue) + "/worklog");
            var seconds = (int)(hours * 60 * 60);
            var data = new { started = alteredDate, timeSpentSeconds = seconds, comment = description };

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local, DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffzz00" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content };
            request.AppendAuthorizationHeader(jira);

            var result = httpClient.SendAsync(request).Result;

            //if (result.StatusCode != HttpStatusCode.Created)
            //    throw new Exception("Work log exception: " + result.ReasonPhrase);

            return (int)result.StatusCode;
        }

        public static IEnumerable<string> GetIssuesWithModifiedWorklogOn(ExportSettings jira, DateTime includedStartDay, DateTime? excludedEndDay = null)
        {
            var uri = new Uri(new Uri(jira.Url), "/rest/api/latest/search/");
            var dateFrom = includedStartDay.Date;
            var dateTo = (excludedEndDay ?? dateFrom.AddDays(1)).Date;
            var data = new
            {
                jql = $"worklogDate >= '{dateFrom:yyyy-MM-dd}' and workLogDate < '{dateTo:yyyy-MM-dd}' and worklogAuthor=currentUser()"
            };

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local, DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffzz00" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = uri, Content = content };
            request.AppendAuthorizationHeader(jira);

            var result = httpClient.SendAsync(request).Result;
            var jsonOut = result.Content.ReadAsStringAsync().Result;

            var jo = JObject.Parse(jsonOut);
            var issueKeys = jo["issues"].Select(i => i["key"].ToObject<string>()).ToList();

            return issueKeys;
        }

        public class Worklog
        {
            public string Self { get; set; }

            public string Issue { get; set; }

            [JsonIgnore]
            public string Author { get; set; }

            public int Id { get; set; }
            public string Comment { get; set; }
            public DateTime Started { get; set; }
            public int TimeSpentSeconds { get; set; }

            public override string ToString()
            {
                return $"{Started:yyyy/MMMM/dd HH:mm:ss} {TimeSpan.FromSeconds(TimeSpentSeconds).TotalHours:N2} {Comment}";
            }
        }

        public static IEnumerable<Worklog> GetWorklogsFor(ExportSettings jira, string issue)
        {
            var uri = new Uri(new Uri(jira.Url), "/rest/api/latest/issue/" + Uri.EscapeUriString(issue) + "/worklog");

            var request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = uri };
            request.AppendAuthorizationHeader(jira);

            var result = httpClient.SendAsync(request).Result;
            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception("Login exception: " + result.ReasonPhrase);

            var json = result.Content.ReadAsStringAsync().Result;
            var jo = JObject.Parse(json);

            var jiraTimeZone = TimeZoneInfo.FindSystemTimeZoneById(jira.Timezone);

            var q = jo["worklogs"].Select(wl =>
            {
                var res = wl.ToObject<Worklog>();
                res.Author = wl["author"]["key"].ToObject<string>();
                res.Issue = issue;

                var date = wl["started"].ToObject<DateTime>();

                var alteredDate = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, jiraTimeZone);
                res.Started = alteredDate;

                return res;
            })
            .ToList();

            return q;
        }

        public static IEnumerable<Worklog> GetWorklogsModifiedOn(ExportSettings jira, DateTime includedStartDay, DateTime? excludedEndDay = null)
        {
            var dateFrom = includedStartDay.Date;
            var dateTo = (excludedEndDay ?? dateFrom.AddDays(1)).Date;

            var issueKeys = GetIssuesWithModifiedWorklogOn(jira, dateFrom, dateTo);

            var worklogs = new List<Worklog>();

            foreach (var issueKey in issueKeys)
            {
                var wl = GetWorklogsFor(jira, issueKey);
                worklogs.AddRange(wl);
            }

            var filteredWorklogs = worklogs
                .Where(i => i.Started >= dateFrom && i.Started <= dateTo)
                .OrderBy(i=>i.Started)
                .ThenBy(i=>i.Issue)
                .ToList();

            return filteredWorklogs;
        }

        public static int DeleteWorklog(ExportSettings jira, string worklogUrl)
        {
            var request = new HttpRequestMessage() { Method = HttpMethod.Delete, RequestUri = new Uri(worklogUrl) };
            request.AppendAuthorizationHeader(jira);

            var result = httpClient.SendAsync(request).Result;
            if (result.StatusCode != HttpStatusCode.NoContent)
                throw new Exception("Delete failed: " + result.ReasonPhrase);

            return (int) result.StatusCode;
        }
    }
}
