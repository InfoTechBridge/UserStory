using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UserStory.Extensions;

namespace UserStory.Tests
{
    [Story(
        AsA = "new user",
        IWantTo = "log in to the website",
        SoThatICan = "access the home page")]

    [TestClass]
    public class HomePageTest
    {
        [TestMethod]
        public void VerifyHomePage()
        {
            var result = this.Given("user navigates to the Website")
                .When(x => x.LoginWithValidUser())
                .Then(x => x.VerifyPageHeader())
                .And(x => x.VerifyPageFooter())
                .Run();

            Assert.AreEqual(Result.Success, result);
        }

        [TestMethod]
        public void VerifyHomePageWitCustomScenarioTitle()
        {
            var result = this.Scenario("As an existing user, I want to log in successfully")
                .Given("user navigate to the Website")
                .When(x => x.LoginWithValidUser())
                .Then(x => x.VerifyPageHeader())
                .And(x => x.VerifyPageFooter())
                .Run();

            Assert.AreEqual(Result.Success, result);
        }

        [TestMethod]
        public void VerifyHomePageByStepTitles()
        {            
            var result = this.Given("user navigates to the Website")
                .When("user login with valid username and password", x => x.LoginWithValidUser())
                .Then("verify page header", x => x.VerifyPageHeader())
                .And("verify page footer", x => x.VerifyPageFooter())
                .Run();

            Assert.AreEqual(Result.Success, result);
        }

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

        private void LoginWithValidUser()
        {
            
        }

        private void VerifyPageHeader()
        {
            
        }

        private void VerifyPageFooter()
        {
            
        }

        private void LoginWithInvalidUser()
        {

        }

        private void VerifyErrorMessage()
        {

        }

        private void NavigateToLoginPage()
        {
            
        }

        private void EnterUserNameAndPassword(string v1, string v2)
        {
            
        }

        private void ClickOnSignInButton()
        {
            
        }

        private void VerifyDashboardPage()
        {
            
        }

        private void VerifyInvalidPasswordMessage()
        {

        }
    }
}