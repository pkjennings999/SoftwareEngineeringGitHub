using Octokit;
using System;
using System.Collections.Generic;
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
            client = new GitHubClient(new Octokit.ProductHeaderValue("octokit.samples"));
            client.Credentials = new Credentials("f12ceda8615af3fb4f70b5255fb4326adf23e185");
            issuesclient = client.Issue;
        }

        [HttpGet]
        [Route("api/issues/url")]
        public Dictionary<DateTime, int> GetAllIssues(string repoUrl)
        {
            if (issuesclient == null)
            {
                Setup();
            }
            repoUrl = Regex.Replace(repoUrl, ".*github.com/", "");
            string user = Regex.Split(repoUrl, "/")[0];
            string repo = Regex.Split(repoUrl, "/")[1];
            var allissues = issuesclient.GetAllForRepository(user, repo, getAll).GetAwaiter().GetResult();
            //ProjectIssue[] issues = new ProjectIssue[allissues.Count];
            //int i = 0;
            //foreach (Issue issue in allissues)
            //{
            //    issues[i++] = new ProjectIssue { Number = issue.Number, CreatedAt = issue.CreatedAt };
            //}

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

            return dict;
        }

        //[HttpGet]
        //[Route("api/issues/{id}")]
        //public IHttpActionResult GetIssue(int id)
        //{
        //    //var issue = issues.FirstOrDefault((p) => p.Id == id);
        //    //if (issue == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    return Ok(issue);
        //}
    }
}
