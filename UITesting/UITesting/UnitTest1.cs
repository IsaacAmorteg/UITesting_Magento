using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Xml.Linq;

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

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

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

            var expected = "Welcome, Isaac Amortegui!";

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("logged-in"), expected));

            IWebElement welcomeMessage = _driver.FindElement(By.ClassName("logged-in"));

            //Assert
            var actual = welcomeMessage.Text;            

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
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
            wait.Until(driver => driver.FindElements(By.ClassName("product-item-link")).All(i =>i.Text != string.Empty));
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
        
        [Test]
        public void Scenario1()
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

            IWebElement gearButtonDropDownMenu = _driver.FindElement(By.Id("ui-id-6"));

            Actions actions = new Actions(_driver);

            actions.MoveToElement(gearButtonDropDownMenu).Perform();                 

            IWebElement watchesButtonByGearDropDownMenu = _driver.FindElement(By.XPath(".//a[@id= 'ui-id-27']"));
            watchesButtonByGearDropDownMenu.Click();

            IWebElement productBox = _driver.FindElement(By.XPath(".//li[@class='item product product-item']//a[contains(text(), 'Dash Digital Watch')]"));
            actions.MoveToElement(productBox).Build().Perform();

            IWebElement addToCartButton = _driver.FindElement(By.XPath(".//form[@data-product-sku= '24-MG02']/button[@class='action tocart primary']"));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
            jsExecutor.ExecuteScript("arguments[0].click();", addToCartButton);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            IWebElement showMiniCartCheckoutButton = _driver.FindElement(By.ClassName("minicart-wrapper"));
            showMiniCartCheckoutButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            IWebElement proceedToCheckoutButton = _driver.FindElement(By.Id("top-cart-btn-checkout"));
            proceedToCheckoutButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(10));

            //cONTINUE WORK HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}