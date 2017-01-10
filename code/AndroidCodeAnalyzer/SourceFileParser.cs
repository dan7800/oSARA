using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class SourceFileParser
    {
        string directory;
        FileInfo[] files;

        public FileInfo[] Files
        {
            get
            {
                if (files == null)
                {
                    GetTextFiles();                    
                }
                return files;
            }
        }

        public SourceFileParser(string Directory)
        {
            this.directory = Directory;
        }

        private void GetTextFiles()
        {
            DirectoryInfo dinfo = new DirectoryInfo(directory);
            files = dinfo.GetFiles("*.txt");
        }

        public List<App> ParseFiles()
        {
            List <App> apps = new List<App>();
            Regex regex_website = new Regex(@"^Web Site:");
            Regex regex_categories = new Regex(@"^Categories:"); //^Categories:.*?(\n|\r|\r\n)
            Regex regex_sourceCode = new Regex(@"^Repo:");
            Regex regex_license = new Regex(@"^License:");
            Regex regex_summary = new Regex(@"^Summary:");
            Regex regex_autoName = new Regex(@"^Auto Name:");
            Regex regex_repoType = new Regex(@"^Repo Type:");
            Regex regex_issueTracker = new Regex(@"^Issue Tracker:");

            App app;
            FileStream fileStream;
            foreach (FileInfo file in files)
            {
                fileStream = file.OpenRead();
                app = new App();
                app.Name = file.Name.Substring(0, file.Name.Length - 4);
                using (StreamReader sr = new StreamReader(fileStream))
                {                    
                    while (!sr.EndOfStream)
                    {
                        var text = sr.ReadLine();
                        if (regex_website.IsMatch(text))
                        {
                            app.Website = text.Substring(9);
                            continue;
                        }
                        if (regex_categories.IsMatch(text))
                        {
                            app.Category = text.Substring(11);
                            continue; ;
                        }
                        if (regex_sourceCode.IsMatch(text))
                        {
                            app.Source = text.Substring(5);
                            continue;
                        }
                        if (regex_license.IsMatch(text))
                        {
                            app.License = text.Substring(8);
                            continue;
                        }
                        if (regex_summary.IsMatch(text))
                        {
                            app.Summary = text.Substring(8);
                            continue;
                        }
                        if (regex_autoName.IsMatch(text))
                        {
                            app.FriendlyName = text.Substring(10);
                            continue;
                        }
                        if (regex_repoType.IsMatch(text))
                        {
                            app.RepoType = text.Substring(10);
                            continue;
                        }
                        if (regex_issueTracker.IsMatch(text))
                        {
                            app.IssueTracker = text.Substring(14);
                            continue;
                        }

                        //Console.WriteLine(sr.ReadLine());
                    }
                }

                apps.Add(app);
            }


            return apps;

        }

    }
}
