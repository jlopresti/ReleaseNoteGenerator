using System;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.IssueTracker;
using ReleaseNoteGenerator.Console.SourceControl;

namespace ReleaseNoteGenerator.Console.Helpers
{
    public static class Guard
    {
        public static void IsNotNullOrEmpty(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
        }

        public static void IsNotNull<T>(T value)
        {
           if(value == null)
                throw new ArgumentNullException(nameof(value));
        }

        public static void IsNotNull(params object[] values)
        {
            foreach (var value in values)
            {
                Guard.IsNotNull<object>(value);
            }
        }
    }
}