## <a id="how-to-contribute">How to contribute?</a>
Your contributions to this project are very welcome.
If you find a bug, please raise it as an issue.
Even better fix it and send a pull request.
If you like to help out with existing bugs and feature requests just check out the list of [issues](https://github.com/InfoTechBridge/UserStory/issues) and grab and fix one.
Some of the issues are labeled as as `jump in`. These issues are generally low hanging fruit so you can start with easier tasks.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community. 
For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

### <a id="getting-started">Getting started</a>
This project uses C# language features, so you'll need any edition of [Visual Studio](https://www.visualstudio.com/downloads/download-visual-studio-vs) to open and compile the project. The free [Community Edition](https://go.microsoft.com/fwlink/?LinkId=532606&clcid=0x409) will work.

### <a id="contribution-guideline">Contribution guideline</a>
This project uses [GitHub flow](http://scottchacon.com/2011/08/31/github-flow.html) for pull requests.
So if you want to contribute, clone the repo, preferably create a local feature/bug branch, based off of the `main` branch, to avoid conflicts with other activities, fix an issue, build and test it then send a PR if all tests are green.

Pull requests are code reviewed. Here is a checklist you should tick through before submitting a pull request:

 - Implementation is clean
 - Code adheres to the existing coding standards; e.g. no curlies for one-line blocks, no redundant empty lines between methods or code blocks, spaces rather than tabs, etc. There is an `.editorconfig` file that must be respected.
 - No ReSharper warnings
 - Don't forget to write proper unit tests
 - If the code is copied from StackOverflow (or a blog or OSS) full disclosure is included. That includes required license files and/or file headers explaining where the code came from with proper attribution
 - Try to not have many comments as comments shouldn't be needed if you write clean code
 - PR is (re)based on top of the latest commits from the `main` branch (more info below)
 - Link to the issue(s) you're fixing from your PR description. Use `fixes #<the issue number>`
 - Readme is updated if you change an existing feature or add a new one
 - Run either `build.cmd` or `build.ps1` and ensure there are no test failures

Please rebase your code on top of the latest `main` branch commits.
Before working on your fork make sure you pull the latest so you work on top of the latest commits to avoid merge conflicts.
Also before sending the pull request please rebase your code as there is a chance there have been new commits pushed after you pulled last.
Please refer to [this guide](https://gist.github.com/jbenet/ee6c9ac48068889b0912#the-workflow) if you're new to git.
