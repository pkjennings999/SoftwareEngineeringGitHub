# GitHub data visualisation project

This program takes a github repository and graphs the open issues over time.

## Dependencies

This program requires an OAuth token from github to work. [Help found here](https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/)

## Usage

Clone the repository and edit the file IssuesController.cs to include your OAuth token.

When the program is run, it will be located at http://localhost:15103/ by default.

Find a repository url, like https://github.com/github-tools/github, and enter it into the search bar.

![alt text](https://github.com/pkjennings999/SoftwareEngineeringGitHub/blob/master/Images/Search.png "Search")

Then click search.


An example of the program working can be seen here, plotting https://github.com/torvalds/linux

![alt text](https://github.com/pkjennings999/SoftwareEngineeringGitHub/blob/master/Images/Linux.png "Linux Issues over time")

### Note
The commit history for this repository seems to have messed up slightly, but I don't know how I would fix it unfortunately.
