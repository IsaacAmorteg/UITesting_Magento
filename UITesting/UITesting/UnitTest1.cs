using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UITesting
{
    public class Tests
    {
        private IWebDriver _driver;
        private readonly string _baseUrl = "https://magento.softwaretestingboard.com/";

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [TestCase("https://magento.softwaretestingboard.com/", "Home Page")]
        [TestCase("https://magento.softwaretestingboard.com/gear.html", "Gear")]
        public void NewBrowserWindow_OpenPage_TitleIsCorrect(string url, string expectedTitle)
        {

            _driver.Navigate().GoToUrl(url);
            Assert.AreEqual(expectedTitle, _driver.Title);

        }
        [Test]

        public void BasePageOpened_SignIn_WelcomeMessageIsDisplayed()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_baseUrl);

            //Action
            IWebElement signInButton = _driver.FindElement(By.XPath(".//li[@class='authorization-link']"));
            signInButton.Click();

            IWebElement emailInput = _driver.FindElement(By.Id("email"));
            IWebElement passwordInput = _driver.FindElement(By.Name("login[password]"));

            emailInput.SendKeys("isaacamortegc@outlook.com");
            passwordInput.SendKeys("Boeing787");

            IWebElement signInButtonLoginPage = _driver.FindElement(By.Id("send2"));

            signInButtonLoginPage.Click();

            Thread.Sleep(TimeSpan.FromSeconds(3));

            IWebElement welcomeMessage = _driver.FindElement(By.ClassName("logged-in"));

            //Assert
            var actual = welcomeMessage.Text;
            var expected = "Welcome, Isaac Amortegui!";

            Assert.AreEqual(expected, actual);

        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}