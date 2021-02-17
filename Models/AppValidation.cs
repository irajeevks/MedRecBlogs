using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MedrecTechnologies.Blog.Models
{
    public class AppValidation
    {
        public const string SoftwareTitle = "MedRec Blogs",
            CompanyName = "MEDREC TECHNOLOGIES";

        public static string ConnectionString = ConfigurationManager.AppSettings["dbconnection"].ToString();
        public bool IsValidDecimal(string value)
        {
            Regex rx = new Regex(@"^\d*\.?\d*$");
            if (!string.IsNullOrEmpty(value))
            {
                if (!rx.IsMatch(value))
                    return false;
            }
            else
                return false;

            return true;
        }
        public bool IsValidNumber(string value)
        {
            Regex rx = new Regex(@"^\d{1,9}$");
            if (!string.IsNullOrEmpty(value))
            {
                if (!rx.IsMatch(value))
                    return false;
            }
            else
                return false;
            return true;
        }
        public bool IsValidEmail(string value)
        {
            Regex rx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (!string.IsNullOrEmpty(value))
            {
                if (!rx.IsMatch(value))
                    return false;
            }
            else
                return false;
            return true;
        }
        /// <summary>
        /// Must meet the following requirements:
        /// At least one lower case letter,
        /// At least one upper case letter,
        /// At least special character,
        /// At least one number,
        /// At least 8 characters length
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValidPassword(string value)
        {
            //Regex rx = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!\#$%&'()*+,\\-./:;<=>?@[\\]^_`{|}~]).*$");
            Regex rx = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_`~.,():;<>]).*$");
            if (!string.IsNullOrEmpty(value))
            {
                if (!rx.IsMatch(value))
                    return false;
            }
            else
                return false;
            return true;
        }

        public DateTime GetDateTime()
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiaTime;
            return indiaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            //return DateTime.Now;
        }
        public enum MessageClass
        {
            Success,
            Failed,
            Warning,
            Clear,
            Info
        }
        public string FormatMessage(MessageClass msgClass, string Msg)
        {
            string _formatMessage = "", _class = "";

            switch (msgClass)
            {
                case MessageClass.Success:
                    _class = "alert alert-success text-left";
                    break;
                case MessageClass.Failed:
                    _class = "alert alert-danger text-left";
                    break;
                case MessageClass.Warning:
                    _class = "alert alert-warning text-left";
                    break;
                case MessageClass.Info:
                    _class = "alert alert-info text-left";
                    break;
            }
            _formatMessage = "<div class=\"" + _class + "\">" + Msg + "</div>";
            return _formatMessage;
        }
        public enum EmailConfigKey
        {
            EMailDefaultConfig = 1
        }
    }
}