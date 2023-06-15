using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace UITesting
{
    public class Tests
    {
        private IWebDriver _driver;
        private readonly string _baseUrl = "https://magento.softwaretestingboard.com/";
        private readonly string _gearPageUrl = "https://magento.softwaretestingboard.com/gear.html";

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
           // _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
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

            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement welcomeMessage = _driver.FindElement(By.ClassName("logged-in"));

            //Assert
            var actual = welcomeMessage.Text;
            var expected = "Welcome, Isaac Amortegui!";

            Assert.AreEqual(expected, actual);
                       
        }

        [Test]
        public void BasePageOpened_SignIn_MainPageNotSignInButton()
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

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var isSignInButtonDisplayedCollection = _driver.FindElements(By.ClassName("authorization-link")).Select(i => i.Displayed);

            //Assert
            Assert.IsFalse(isSignInButtonDisplayedCollection.Distinct().Single());                             

        }
        [Test]
        public void OpenGearPage_GetHotSellers_ProductsAreCorrect()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_gearPageUrl);

            //Action
            Thread.Sleep(TimeSpan.FromSeconds(1));
            IEnumerable<IWebElement> productNameCollection = _driver.FindElements(By.ClassName("product-item-link"));
            var actual = productNameCollection.Select(i => i.Text);
            var expected = new[]
            {
                "Fusion Backpack",
                "Push It Messenger Bag",
                "Affirm Water Bottle",
                "Sprite Yoga Companion Kit"
            };

            //Assert
            Assert.AreEqual(expected, actual);
        }
        /*
        [Test]
        public void Sandbox()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            var value = _driver.FindElements(By.Id("2345353454"));
        }
        */

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}