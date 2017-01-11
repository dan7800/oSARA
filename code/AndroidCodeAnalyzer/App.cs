using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class App
    {
        long id;
        string name, friendlyName, summary, category, website, source, license, repoType, issueTracker;

        public string Category { get => category; set => category = value; }
        public string Summary { get => summary; set => summary = value; }
        public long Id { get => id; set => id = value; }
        public string License { get => license; set => license = value; }
        public string Name { get => name; set => name = value; }
        public string Source { get => source; set => source = value; }
        public string Website { get => website; set => website = value; }
        public string FriendlyName { get => friendlyName; set => friendlyName = value; }
        public string RepoType { get => repoType; set => repoType = value; }
        public string IssueTracker { get => issueTracker; set => issueTracker = value; }

        public App()
        {
            this.name = string.Empty;
            this.friendlyName = string.Empty;
            this.summary = string.Empty;
            this.category = string.Empty;
            this.website = string.Empty;
            this.source = string.Empty;
            this.license = string.Empty;
            this.repoType = string.Empty;
            this.issueTracker = string.Empty;
        }
    }
}
