# UserStory

UserStory is a simple .Net framework for Behavioral-Driven Development (BDD) approach that used for writing Given-When-Then user stories.

It allows to define and run User Stories or standalone scenarios inside any type of .Net projects specially unit test and UI automation test projects.


### Table of contents
 - [Features](#features)
 - [Install](#install)
 - [Usage](#usage)
    - [Example 1: Standalone Given-When-Then Scenario](#example1)
    - [Example 2: Inside Unit/Integration Test Projects](#example2)
    - [Example 3: With Custom Description for each step](#example3)
    - [Example 4: Full User Story Defination](#example4)
 - [Main contributors](#main-contributors)
 - [License](#license)
 
## <a id="features">Features</a>

UserStory is very simple framework to use! 
 - It can be used in any type of .Net projects including unit test, UI automation test, Console App, Web App and so on. Actually you don't have to use a testing framework at all.
 - It does not need to have a separate (test) runner. It can be run by itself or by your runner of choice and you will get the same result regardless of the runner.
 - It supports stories and also can run standalone scenarios and it is not necessarily have to have or make up a story to use it.
 - It automaticaly takes stories/scenarios/steps title/description from method names and you don't need to explain them in string but you have full control over what gets printed into console and reports if you want.
 - It supports underscored/pascal/camel cased method names for your steps title generation.
 - It is very simple and extensible.

## <a id="install">Install</a>

You can install UserStory nuget package from [this location](https://www.nuget.org/packages/UserStory) or by following command:

```
Install-Package UserStory
```

This commands adds UserStory assembly and its dependencies to your target project. 

## <a id="usage">Usage</a>
After adding UserStory package to project we can start using it like following examples:

### <a id="example1">Example 1: Standalone Given-When-Then Scenario</a>

We can use the standalone Given-When-Then scenario whenever we like in any type of .Net projects:

```C#
using UserStory.Extensions;
...
this.Scenario("As an existing user, I want to log in successfully")
    .Given("user navigate to the Website")
    .When(x => x.LoginWithValidUser())
    .Then(x => x.VerifyPageHeader())
    .And(x => x.VerifyPageFooter())
    .Run();
...
```

This example generates the following console report:

```
Scenario #1: As an existing user, I want to log in successfully
	    Given user navigate to the Website
	    When login with valid user
	    Then verify page header
	    And verify page footer
	    Result: SUCCESS
```

Note: Remember to define the following methods and implemane them accordingly based on your logic. These methods will be executed during scenario execution.

```C#
private void LoginWithValidUser()
{
    ...
}

private void VerifyPageHeader()
{
    ...
}

private void VerifyPageFooter()
{
    ...
}
```

### <a id="example2">Example 2: Inside Unit/Integration Test Projects</a>

We can use same Fluent API syntax inside the any unit test projects specialy inside UI automation test methods.

```C#
using UserStory.Extensions;
...
[TestMethod]
public void VerifyHomePage()
{
    var result = this.Given("user navigate to the Website")
        .When(x => x.LoginWithValidUser())
        .Then(x => x.VerifyPageHeader())
        .And(x => x.VerifyPageFooter())
        .Run();

    Assert.AreEqual(Result.Success, result);
}
```

This gives you a console report like:

```
Scenario #1: Verify home page
	Given user navigate to the Website
	When login with valid user
	Then verify page header
	And verify page footer
	Result: SUCCESS
```

Note: It is not mandatory to start the Fluent API by this.Scenario(...). We can start it from this.Given(...) like above example. In this case it takes the title of scenario from the method that scenario defined inside it. For example here it uses VerifyHomePageByTitles() name for title of scenario.

### <a id="example3">Example 3: With Custom Description for each step</a>

Here is another example that shows how we can control the description of each step inside report: 

```C#
using UserStory.Extensions;
...
[TestMethod]
public void VerifyHomePageWitCustomDescription()
{
    var result = this.Scenario("As an existing user, I want to log in successfully")
        .Given("user navigates to the Login page", x => x.NavigateToLoginPage())
        .When("user enters the valid username and password", x => x.EnterUserNameAndPassword("user1", "pass1"))
        .And("user clicks on sign-in button", x => x.ClickOnSignInButton())
        .Then("user successfully navigates to the dashboard page", x => x.VerifyDashboardPage())
        .Run();

    Assert.AreEqual(Result.Success, result);
}
```

This gives you a console report like:

```
Scenario #5: As an existing user, I want to log in successfully
	Given user navigates to the Login page
	When user enters the valid username and password
	And user clicks on sign-in button
	Then user successfully navigates to the dashboard page
	Result: SUCCESS
```

### <a id="example4">Example 4: Full User Story Defination</a>

```C#
using UserStory.Extensions;
...
[TestMethod]
public void VerifyDashboardPageFullUserStory()
{
    var story = this.Story(
        title: "Login to the site",
        id: "#JIRA123",
        description: "The user shall be able to login to site after entering the correct username and password.",
        asA: "new user", 
        iWantTo: "log in to the website", 
        soThatICan: "access the dashboard page");

    story.AddScenario(this
        .Scenario("Login as a valid user with valid data")
        .Given("user navigates to the Login page", x => x.NavigateToLoginPage())
        .When("user enters the valid username and password", x => x.EnterUserNameAndPassword("user1", "pass1"))
        .And("user clicks on sign-in button", x => x.ClickOnSignInButton())
        .Then("user successfully navigates to the dashboard page", x => x.VerifyDashboardPage())
        .And(x => x.VerifyPageHeader())
        .And(x => x.VerifyPageFooter())
    );

    story.AddScenario(this
        .Scenario("Login as a valid user user by entering an invalid password")
        .Given("user navigates to the Login page", x => x.NavigateToLoginPage())
        .When("user enters the invalid username and password", x => x.EnterUserNameAndPassword("user1", "invalidpass1"))
        .And("user clicks on sign-in button", x => x.ClickOnSignInButton())
        .Then("invalid password message should be displayed", x => x.VerifyInvalidPasswordMessage())
    );

    story.Run();

    Assert.AreEqual(Result.Success, story.Result);
}
```

And this gives you a report like:

```
Story #JIRA123: Login to the site
Description: The user shall be able to login to site after entering the correct username and password.
	As a new user
	I want to log in to the website
	So that I can access the dashboard page

Scenario #1: Login as a valid user with valid data
	Given user navigates to the Login page
	When user enters the valid username and password
	And user clicks on sign-in button
	Then user successfully navigates to the dashboard page
	And verify page header
	And verify page footer
	Result: SUCCESS

Scenario #2: Login as a valid user user by entering an invalid password
	Given user navigates to the Login page
	When user enters the invalid username and password
	And user clicks on sign-in button
	Then invalid password message should be displayed
	Result: SUCCESS

Result: SUCCESS
```	


## <a id="how-to-contribute">How to contribute?</a>

Please see <a href="https://github.com/InfoTechBridge/UserStory/blob/master/CONTRIBUTING.md">CONTRIBUTING.md</a>.

## <a id="main-contributors">Main contributors</a>
 - Amir Arayeshi ([AArayeshi](https://github.com/AArayeshi))

## <a id="license">License</a>
UserStory is released under the [MIT](https://opensource.org/licenses/MIT) License. See the [LICENSE](https://github.com/InfoTechBridge/UserStory/blob/master/LICENSE.txt) file for details.
