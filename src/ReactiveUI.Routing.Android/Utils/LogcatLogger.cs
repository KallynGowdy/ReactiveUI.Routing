using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Routing.Android.Utils
{
    public class LogcatLogger : ILogger
    {
        public string Tag { get; set; }

        public LogLevel Level { get; set; }

        public LogcatLogger()
            : this(AppDomain.CurrentDomain.FriendlyName)
        {
        }

        public LogcatLogger(string tag)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }

        public void Write(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Log.Debug(Tag, message);
                    break;
                case LogLevel.Info:
                    Log.Info(Tag, message);
                    break;
                case LogLevel.Warn:
                    Log.Warn(Tag, message);
                    break;
                case LogLevel.Error:
                case LogLevel.Fatal:
                    Log.Error(Tag, message);
                    break;
            }
        }
    }
}