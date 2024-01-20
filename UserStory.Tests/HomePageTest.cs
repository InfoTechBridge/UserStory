using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UserStory.Extensions;

namespace UserStory.Tests
{
    [TestClass]
    public class HomePageTest
    {
        [TestMethod]
        public void VerifyHomePage()
        {
            this.Given("user navigate to the Website")
                .When(x => x.LoginWithValidUser())
                .Then("verify page header", x => x.VerifyPageHeader())
                .And("verify page header", x => x.VerifyPageHeader())
                .And(null, x => x.VerifyPageHeader())
                .Then(null, (x) => VerifyPageHeader())
                .Then((x) => VerifyPageHeader())
                .And(x => x.VerifyPageFooter())
                .Run();
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

        
    }
}