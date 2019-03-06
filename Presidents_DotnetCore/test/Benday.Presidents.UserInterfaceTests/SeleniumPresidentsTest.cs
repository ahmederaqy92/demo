using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.PhantomJS;
using System.Configuration;

namespace Benday.Presidents.UserInterfaceTests
{
    [TestClass]
    public partial class SeleniumPresidentsTest
    {
        private string _BaseUrl = "http://azdemo17-docker.demo.local";
        private RemoteWebDriver _Driver;

        [TestInitialize]
        public void OnTestInitialize()
        {
            _Driver = null;
            _Driver = new PhantomJSDriver();
            // _Driver = new ChromeDriver();

            _Driver.Manage().Window.Maximize();

            CheckAppConfigForUpdatedBaseUrl();

            _Driver.Navigate().GoToUrl(_BaseUrl);
        }

        void CheckAppConfigForUpdatedBaseUrl()
        {
            var value = ConfigurationManager.AppSettings["Presidents.Selenium.BaseUrl"];

            if (value != null)
            {
                Console.WriteLine("Found updated 'Presidents.Selenium.BaseUrl' value in app.config.");
                _BaseUrl = value;
            }
            else
            {
                Console.WriteLine("No 'Presidents.Selenium.BaseUrl' value found in app.config.");
            }

            Console.WriteLine("Base url: {0}", _BaseUrl);
        }

        [TestMethod]
        public void Selenium_PopulateDatabase()
        {
            var linkResetDatabase = _Driver.FindElementByLinkText("Reset Database");

            linkResetDatabase.Click();

            var wait = new WebDriverWait(_Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(By.TagName("footer")));

            var pageSource = _Driver.PageSource;

            StringAssert.Contains(pageSource, "Adams", "John Adams should be visible.");
        }

        [TestMethod]
        public void Selenium_EnableFeatureFlag()
        {
            SetFeatureFlagAndAssert("Search", false);
            SetFeatureFlagAndAssert("Search", true);
        }

        [TestMethod]
        public void Selenium_DisableFeatureFlag()
        {
            SetFeatureFlagAndAssert("Search", true);
            SetFeatureFlagAndAssert("Search", false);
        }

        [TestMethod]
        public void Selenium_SearchIsNotVisibleWhenFeatureFlagIsTurnedOff()
        {
            SetFeatureFlagAndAssert("Search", false);

            var matches = _Driver.FindElementsByLinkText("Search");

            Assert.AreEqual<int>(0, matches.Count, "Search link should not be visible.");
        }

        [TestMethod]
        public void Selenium_SearchIsVisibleWhenFeatureFlagIsTurnedOn()
        {
            SetFeatureFlagAndAssert("Search", true);

            var link = _Driver.FindElementByLinkText("Search");

            Assert.IsNotNull(link, "Search link should not be visible.");
        }

        public void AssertFeatureFlagValue(Dictionary<string, FeatureTableRow> features,
            string featureName,
            bool expectedIsEnabled)
        {
            AssertFeatureFlagExists(features, featureName);

            var feature = features[featureName];

            Assert.AreEqual<bool>(expectedIsEnabled, feature.IsEnabled,
                "IsEnabled value for feature '{0}' was wrong.", featureName);
        }

        public void AssertFeatureFlagExists(
            Dictionary<string, FeatureTableRow> features,
            string expectedFeatureName)
        {
            Assert.IsNotNull(features, "Features collection was null.");
            Assert.IsTrue(features.ContainsKey(expectedFeatureName),
                "Feature '{0}' does not exist in table.",
                expectedFeatureName);
        }

        private IWebElement FindSaveButton()
        {
            var inputElements = _Driver.FindElementsByTagName("input");

            foreach (var item in inputElements)
            {
                if (item.GetAttribute("type") == "submit" &&
                    item.GetAttribute("value") == "Save")
                {
                    return item;
                }
            }

            return null;
        }

        public void SetFeatureFlagAndAssert(string featureName, bool isEnabled)
        {
            var linkFeatures = _Driver.FindElementByLinkText("Features");

            linkFeatures.Click();
            WaitUntilFooterIsVisible();

            StringAssert.EndsWith(_Driver.Url, "/Feature", "Should be on features url.");
            AssertPageSourceContains("Feature Flag Manager: Overview");

            var features = ParseFeatureFlagOverviewTable();

            AssertFeatureFlagExists(features, featureName);

            var feature = features[featureName];

            if (feature.IsEnabled == isEnabled)
            {
                Console.WriteLine("Feature '{0}' is already set to '{1}'.",
                    featureName,
                    feature.IsEnabled);
            }
            else
            {
                feature.EditLink.Click();
                WaitUntilFooterIsVisible();

                var isEnabledCheckbox = _Driver.FindElementById("IsEnabled");

                var saveButton = FindSaveButton();

                Assert.IsNotNull(isEnabledCheckbox, "Could not locate checkbox for feature.");
                Assert.IsNotNull(saveButton, "Could not locate save button.");

                if (isEnabledCheckbox.Selected != isEnabled)
                {
                    isEnabledCheckbox.Click();
                }

                saveButton.Click();
                WaitUntilFooterIsVisible();

                AssertFeatureFlagValue(ParseFeatureFlagOverviewTable(),
                    featureName,
                    isEnabled);
            }
        }

        private Dictionary<string, FeatureTableRow> ParseFeatureFlagOverviewTable()
        {
            var rows = _Driver.FindElementsByClassName("feature-row");

            var features = new Dictionary<string, FeatureTableRow>();

            foreach (var row in rows)
            {
                var temp = new FeatureTableRow(row);

                if (temp.IsValid == true)
                {
                    features.Add(temp.FeatureName, temp);
                }
            }

            return features;
        }

        private void AssertPageSourceContains(string expectedText)
        {
            var pageSource = _Driver.PageSource;

            Assert.IsTrue(pageSource.Contains(expectedText),
                "Page source did not contain expected text '{0}'.",
                expectedText);
        }

        private void WaitUntilFooterIsVisible()
        {
            var wait = new WebDriverWait(_Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(By.TagName("footer")));
        }

        [TestCleanup]
        public void OnTestCleanup()
        {
            _Driver.Quit();
        }

        public TestContext TestContext
        {
            get;
            set;
        }
    }
}
