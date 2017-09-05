using System;

namespace Omichron
{
    public class TimeLog
    {
        public TimeLog(string issueId, DateTime started, TimeSpan duration)
        {
            IssueId = issueId;
            Started = started;
            Duration = duration;
        }

        public string IssueId { get; }
        public string Title => $"Summary title of {IssueId}";
        public DateTime Started { get; }
        public TimeSpan Duration { get; }
    }
}
