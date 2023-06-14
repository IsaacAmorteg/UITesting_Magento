using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UITesting
{
    public class Tests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [TestCase("https://google.com", "Google")]
        [TestCase("https://google.com/doodles", "Google Doodles")]
        public void NewBrowserWindow_OpenPage_TitleIsCorrect(string url, string expectedTitle)
        {

            _driver.Navigate().GoToUrl(url);
            Assert.AreEqual(expectedTitle, _driver.Title);

        }
        [Test]

        public void BasePageOpened_SignIn_WelcomeMessageIsDisplayed()
        {

        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}