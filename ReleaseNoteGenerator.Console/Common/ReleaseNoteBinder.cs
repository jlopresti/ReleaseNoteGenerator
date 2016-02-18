using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNoteGenerator.Console.Models;

namespace ReleaseNoteGenerator.Console.Common
{
    public class ReleaseNoteBinder
    {
        private readonly List<Commit> _commits;
        private readonly List<Issue> _issues;

        public ReleaseNoteBinder(List<Commit> commits, List<Issue> issues)
        {
            _commits = commits;
            _issues = issues;
        }

        public List<ReleaseNoteEntry> Bind()
        {
            var entries = new List<ReleaseNoteEntry>();
            entries.AddRange(GetOnlyCommitedItems());
            entries.AddRange(GetOnlyInIssuesTracker());
            entries.AddRange(GetCommitedAndAttachedItems());
            entries.AddRange(GetUnknownCommits());
            return entries;
        }

        private List<ReleaseNoteEntry> GetUnknownCommits()
        {
            return _commits.Where(x => !x.HasExtractedKey).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                CommitUrl = x.Url,
                Status = Status.OnlyCommited
            }).ToList();
        }

        private List<ReleaseNoteEntry> GetCommitedAndAttachedItems()
        {
            var entries = from i in _issues
                join c in _commits on i.Id.ToLowerInvariant() equals c.Id.ToLowerInvariant()
                select new ReleaseNoteEntry()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Author = c.Author,
                    CommitUrl = c.Url,
                    Status = Status.Ok
                };

            return entries.ToList();
        }


        private List<ReleaseNoteEntry> GetOnlyCommitedItems()
        {
            var issueKeys = _issues.Select(x => x.Id).ToList();

            return _commits.Where(x => !issueKeys.Contains(x.Id, StringComparer.InvariantCultureIgnoreCase) && x.HasExtractedKey).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                CommitUrl = x.Url,
                Status = Status.OnlyCommited,
            }).ToList();
        }

        private List<ReleaseNoteEntry> GetOnlyInIssuesTracker()
        {
            var commitKeys = _commits.Select(x => x.Id).ToList();

            return _issues.Where(x => !commitKeys.Contains(x.Id, StringComparer.InvariantCultureIgnoreCase)).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Author = string.Empty,
                Status = Status.OnlyAttachedInIssueTracker
            }).ToList();
        }
    }
}