using Atlassian.Jira;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    internal interface IPublisherFactory
    {
        IPublisher GetProvider(Settings settings);
    }
}