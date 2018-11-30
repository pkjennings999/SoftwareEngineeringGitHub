using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace GitHubWebProject.Controllers
{
    public class IssuesController : ApiController
    {
        GitHubClient client;
        IIssuesClient issuesclient;
        RepositoryIssueRequest getAll = new RepositoryIssueRequest
        {
            State = ItemStateFilter.All
        };

        private void Setup()
        {
            client = new GitHubClient(new Octokit.ProductHeaderValue("GitHubDataVisProject"));
            // Uncomment and add in your OAuth Token
            //client.Credentials = new Credentials(OAuth Token);
            issuesclient = client.Issue;
        }

        [HttpGet]
        [Route("api/issues/url")]
        public (List<KeyValuePair<DateTime, int>>, string) GetAllIssues(string repoUrl)
        {
            if (issuesclient == null)
            {
                Setup();
            }
            repoUrl = Regex.Replace(repoUrl, ".*github.com/", "");
            string user = Regex.Split(repoUrl, "/")[0];
            string repo = Regex.Split(repoUrl, "/")[1];
            var allissues = issuesclient.GetAllForRepository(user, repo, getAll).GetAwaiter().GetResult();

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
            var myList = dict.ToList();

            myList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));
            DateTime first = myList.ElementAt(0).Key;

            while (first <= DateTime.Now)
            {
                if (!dict.ContainsKey(first))
                {
                    myList.Add(new KeyValuePair<DateTime, int>(first, 0));
                }
                first = first.AddDays(1);
            }
            myList.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            return (myList, repo);
        }
    }
}
