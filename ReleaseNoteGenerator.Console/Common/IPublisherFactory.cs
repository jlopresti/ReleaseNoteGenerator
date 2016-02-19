using Atlassian.Jira;
using Newtonsoft.Json.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IPublisherFactory
    {
        IPublisher GetProvider(Config settings);
    }
}