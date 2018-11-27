using Octokit;
using System;
using System.Collections.Generic;

namespace GitHubVisualisation.BackEnd.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            GitHubClient client = new GitHubClient(new Octokit.ProductHeaderValue("octokit.samples"));

            client.Credentials = new Credentials("f12ceda8615af3fb4f70b5255fb4326adf23e185");
            IIssuesClient issuesclient = client.Issue;
            RepositoryIssueRequest getAll = new RepositoryIssueRequest
            {
                State = ItemStateFilter.All
            };

            var allissues = issuesclient.GetAllForRepository("torvalds", "linux", getAll).GetAwaiter().GetResult();
            Console.WriteLine("Got stats");

            Dictionary<DateTime, int> dict = new Dictionary<DateTime, int>();
            foreach (Issue issue in allissues)
            {
                DateTime created = new DateTime(issue.CreatedAt.Year, issue.CreatedAt.Month, issue.CreatedAt.Day, 0, 0, 0);
                DateTimeOffset closedOffset = issue.ClosedAt ?? DateTimeOffset.Now.AddDays(1);
                DateTime closed = new DateTime(closedOffset.Year, closedOffset.Month, closedOffset.Day, 0, 0, 0);
                Console.WriteLine("New issue");
                while (created < closed)
                {
                    if (dict.ContainsKey(created))
                    {
                        dict.TryGetValue(created, out int value);
                        dict.Remove(created);
                        dict.Add(created, ++value);
                    }
                    else
                    {
                        dict.Add(created, 1);
                    }
                    created = created.AddDays(1);
                }
            }
            //Issue issue;
            //issue.CreatedAt();
            //issue.Number();
            foreach (var i in dict)
            {
                Console.WriteLine("[\"" + i.Key.Month + "/" + i.Key.Day + "/" + i.Key.Year + "\", " + i.Value + "],");
            }
            int x = 0;

        }
    }
}
