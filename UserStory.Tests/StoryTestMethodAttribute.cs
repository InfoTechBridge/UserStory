using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStory.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StoryTestMethodAttribute : TestMethodAttribute
    {
        //public override TestResult[] Execute()
        //{
        //    FeatureFlagHelper.SetFeatureFlag(featureFlagName, featureFlagState);
        //    var results = base.Execute();
        //    FeatureFlagHelper.UnsetFeatureFlag(featureFlagName, featureFlagState);
        //    return results;
        //}

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var res = new List<TestResult>();

            var r = new TestResult
            {
                Outcome = UnitTestOutcome.Failed,
                TestFailureException = new AssertFailedException("tst message"),
                DisplayName = testMethod.TestClassName + " " + testMethod.TestMethodName + " - Iteration "
        };
            res.Add(r);

            for (int i = 0; i < 3; i++)
            {
                foreach (var x in base.Execute(testMethod))
                {
                    x.DisplayName = testMethod.TestClassName + " " + testMethod.TestMethodName + " - Iteration " + (i + 1);
                    res.Add(x);
                }
            }

            
            //testMethod.
            

            return res.ToArray();

            //var results = base.Execute(testMethod);

            
            //    foreach (var x in results)
            //    {
            //        x.DisplayName = testMethod.TestMethodName + " - Iteration ";
                    
            //    }

            //return results;

            //return new TestResult[1] { testMethod.Invoke(null) };
        }

        public StoryTestMethodAttribute()
        {
            
        }

        public StoryTestMethodAttribute(string displayName) 
            : base(displayName)
        {
        }

        
    }
}
