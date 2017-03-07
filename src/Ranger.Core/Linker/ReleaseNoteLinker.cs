using System;
using System.Collections.Generic;
using System.Linq;
using Ranger.Core.Models;
using Ranger.Core.Models.Binder;
using Ranger.Core.Models.IssueTracker;
using Ranger.Core.Models.SourceControl;

namespace Ranger.Core.Linker
{
    public class ReleaseNoteLinker : IReleaseNoteLinker
    {

        public List<ReleaseNoteEntry> Link(List<Commit> commits, List<Issue> issues)
        {
            var entries = new List<ReleaseNoteEntry>();
            entries.AddRange(GetOnlyCommitedItems(commits,issues));
            entries.AddRange(GetOnlyInIssuesTracker(commits, issues));
            entries.AddRange(GetCommitedAndAttachedItems(commits, issues));
            entries.AddRange(GetUnknownCommits(commits, issues));
            return entries.OrderBy(x => x.Id).ToList();
        }

        private List<ReleaseNoteEntry> GetUnknownCommits(List<Commit> commits, List<Issue> issues)
        {
            return commits.Where(x => !x.HasExtractedKey).Select(x => new ReleaseNoteEntry
            {
                Id = "Unknown",
                Title = x.Title,
                Authors = x.Authors,
                CommitUrl = x.Url,
                Project = x.Project,
                Status = Status.OnlyCommited
            }).ToList();
        }

        private List<ReleaseNoteEntry> GetCommitedAndAttachedItems(List<Commit> commits, List<Issue> issues)
        {
            var entries = from i in issues
                join c in commits on i.Id.ToLowerInvariant() equals c.Id.ToLowerInvariant()
                select new ReleaseNoteEntry()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Authors = c.Authors,
                    CommitUrl = c.Url,
                    Project = c.Project,
                    Status = Status.Ok,
                    AdditionalData = i.AdditionalData
                };

            return entries.ToList();
        }


        private List<ReleaseNoteEntry> GetOnlyCommitedItems(List<Commit> commits, List<Issue> issues)
        {
            var issueKeys = issues.Select(x => x.Id).ToList<string>();

            return commits.Where(x => !issueKeys.Contains(x.Id, StringComparer.InvariantCultureIgnoreCase) && x.HasExtractedKey).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Authors = x.Authors,
                CommitUrl = x.Url,
                Project = x.Project,
                Status = Status.OnlyCommited,
                AdditionalData = x.AdditionalData
            }).ToList();
        }

        private List<ReleaseNoteEntry> GetOnlyInIssuesTracker(List<Commit> commits, List<Issue> issues)
        {
            var commitKeys = commits.Select(x => x.Id).ToList<string>();

            return issues.Where(x => !commitKeys.Contains(x.Id, StringComparer.InvariantCultureIgnoreCase)).Select(x => new ReleaseNoteEntry
            {
                Id = x.Id,
                Title = x.Title,
                Status = Status.OnlyAttachedInIssueTracker,
                AdditionalData = x.AdditionalData
            }).ToList();
        }
    }
}