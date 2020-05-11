using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JiraWorkLogUploader.Ui;
using Newtonsoft.Json;

namespace JiraWorkLogUploader.Config
{
    [TypeConverter(typeof(SerializableExpandableObjectConverter))]
    public class ExportSettings
    {
        public string Display { get; set; }
        public string User { get; set; }

        [JsonProperty("Password")] private string _password;

        [JsonIgnore]
        [PasswordPropertyText(true)]
        public string Password
        {
            get { return string.IsNullOrEmpty(_password) ? "" : SafeUnprotect(_password); }
            set { _password = Protect(value); }
        }

        [JsonProperty("UserEmail")] private string _userEmail;

        [JsonIgnore]
        [PasswordPropertyText(true)]
        public string UserEmail
        {
            get { return string.IsNullOrEmpty(_userEmail) ? "" : SafeUnprotect(_userEmail); }
            set { _userEmail = Protect(value); }
        }

        [JsonProperty("ApiToken")] private string _apiToken;

        [JsonIgnore]
        [PasswordPropertyText(true)]
        public string ApiToken
        {
            get { return string.IsNullOrEmpty(_apiToken) ? "" : SafeUnprotect(_apiToken); }
            set { _apiToken = Protect(value); }
        }

        public string Url { get; set; }

        [TypeConverter(typeof(GenericEnumerableConverter))]
        public string Timezone { get; set; }

        public string Column { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public IEnumerable<string> ItemsFor_Timezone
        {
            get { return TimeZoneInfo.GetSystemTimeZones().Select(i => i.Id).ToList(); }
        }

        [Browsable(false)]
        [JsonIgnore]
        public int ColumnNo => Column.ToUpper()[0] - 'A';

        private string Protect(string clearText)
        {
            var textBytes = Encoding.Unicode.GetBytes(clearText);
            var cryptedBytes = ProtectedData.Protect(textBytes, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(cryptedBytes);
        }

        private string Unprotect(string cryptedText)
        {
            var cryptedBytes = Convert.FromBase64String(cryptedText);
            var textBytes = ProtectedData.Unprotect(cryptedBytes, null, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(textBytes);
        }

        private string SafeUnprotect(string cryptedText)
        {
            try
            {
                return Unprotect(cryptedText);
            }
            catch (FormatException)
            {
                // not base 64
                return string.Empty;
            }
            catch (CryptographicException)
            {
                // problem decrypting
                return string.Empty;
            }
        }
    }
}
