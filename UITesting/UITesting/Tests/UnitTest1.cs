using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Numerics;
using System.Xml.Linq;
using UITesting.Pages;

namespace UITesting.Tests
{
    public class Tests
    {
        [ThreadStatic]
        private static IWebDriver _driver;
        private readonly string _baseUrl = "https://magento.softwaretestingboard.com/";
        private readonly string _gearPageUrl = "https://magento.softwaretestingboard.com/gear.html";

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            _driver.Manage().Window.Maximize();
        }

        [TestCase("https://magento.softwaretestingboard.com/", "Home Page")]
        [TestCase("https://magento.softwaretestingboard.com/gear.html", "Gear")]
        public void NewBrowserWindow_OpenPage_TitleIsCorrect(string url, string expectedTitle)
        {

            _driver.Navigate().GoToUrl(url);
            Assert.AreEqual(expectedTitle, _driver.Title);

        }

        [TestCase("https://magento.softwaretestingboard.com/")]
        [TestCase("https://magento.softwaretestingboard.com/gear.html")]
        public void BasePageOpened_SignIn_WelcomeMessageIsDisplayed(string url)
        {
            //Precondition
            _driver.Navigate().GoToUrl(url);

            //Action
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInButton();

            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));

            wait.Until((driver) => driver.FindElement(By.ClassName("logged-in")).Text.StartsWith("Welcome, "));

            IWebElement welcomeMessage = _driver.FindElement(By.ClassName("logged-in"));

            //Assert
            var actual = welcomeMessage.Text;
            var expected = "Welcome, Isaac Amortegui!";

            Assert.AreEqual(expected, actual);

        }

        [Test]

        public void BasePageOpened_TrySignInWithNoPassword_ErrorMessageIsDisplayed()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_baseUrl);

            //Action
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInButton();

            var loginPage = new LoginPage(_driver);
            loginPage.EnterEmail("isaacamortegc@outlook.com");
            loginPage.ClickSignInButtonLogin();

            IWebElement errorMessage = _driver.FindElement(By.ClassName("fieldset"));

            //Assert

            var actual = errorMessage.GetAttribute("data-hasrequired");
            var expected = "* Required Fields";

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void BasePageOpened_SignIn_MainPageNotSignInButton()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_baseUrl);

            //Action
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInButton();

            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");

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
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            wait.Until(driver => driver.FindElements(By.ClassName("product-item-link")).Count == 4);
            wait.Until(driver => driver.FindElements(By.ClassName("product-item-link")).All(i => i.Text != string.Empty));
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
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInButton();

            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");

            WebDriverWait waitGearButton = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            waitGearButton.Until(ExpectedConditions.ElementExists(By.Id("ui-id-6")));

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

            //Keep below Thread Sleep until fixing below Commented Explicit waiter.
            Thread.Sleep(TimeSpan.FromSeconds(3));
            // WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            //wait.Until(ExpectedConditions.ElementExists(By.ClassName("minicart-wrapper")));

            IWebElement showMiniCartCheckoutButton = _driver.FindElement(By.ClassName("minicart-wrapper"));
            showMiniCartCheckoutButton.Click();

            WebDriverWait waitCheckout = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            waitCheckout.Until(ExpectedConditions.ElementToBeClickable(By.Id("top-cart-btn-checkout")));

            IWebElement proceedToCheckoutButton = _driver.FindElement(By.Id("top-cart-btn-checkout"));
            proceedToCheckoutButton.Click();

            WebDriverWait waitNext = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            waitNext.Until(ExpectedConditions.ElementExists(By.XPath(".//button[contains(span/@data-bind, 'Next')]")));

            bool isNewAddressButtonPresent = _driver.FindElements(By.XPath(".//button[contains(span/@data-bind, 'New Address')]")).Count > 0;

            if (isNewAddressButtonPresent)
            {
                IWebElement newAddressButton = _driver.FindElement(By.XPath(".//button[contains(span/@data-bind, 'New Address')]"));
                newAddressButton.Click();
            }


            IWebElement firstNameInput = _driver.FindElement(By.CssSelector("input[name='firstname']"));
            IWebElement lastNameInput = _driver.FindElement(By.CssSelector("input[name='lastname']"));
            IWebElement streetInput = _driver.FindElement(By.CssSelector("input[name='street[0]']"));
            IWebElement cityInput = _driver.FindElement(By.CssSelector("input[name='city']"));
            IWebElement countryInput = _driver.FindElement(By.CssSelector("select[name='country_id']"));
            SelectElement selectCountry = new SelectElement(countryInput);
            IWebElement zipPostalCodeInput = _driver.FindElement(By.CssSelector("input[name ='postcode']"));
            IWebElement phoneInput = _driver.FindElement(By.CssSelector("input[name='telephone']"));

            firstNameInput.Clear();
            firstNameInput.SendKeys("Isaac");

            lastNameInput.Clear();
            lastNameInput.SendKeys("Amortegui");

            streetInput.Clear();
            streetInput.SendKeys("Spur Road");

            cityInput.Clear();
            cityInput.SendKeys("London");

            selectCountry.SelectByText("United Kingdom");

            zipPostalCodeInput.Clear();
            zipPostalCodeInput.SendKeys("SW1A 1AA");

            Random random = new Random();
            string phoneNumber = "";
            for (int i = 0; i < 10; i++)
            {
                phoneNumber += random.Next(0, 10).ToString();
            }
            phoneInput.Clear();
            phoneInput.SendKeys(phoneNumber);

            bool isShipHereButtonPresent = _driver.FindElements(By.XPath(".//footer//span[contains(text(), 'Ship here')]")).Count > 0;

            if (isShipHereButtonPresent)
            {
                IWebElement shipHereButton = _driver.FindElement(By.XPath(".//footer//span[contains(text(), 'Ship here')]"));
                shipHereButton.Click();

            }

            IWebElement nextButton = _driver.FindElement(By.XPath(".//button[contains(span/@data-bind, 'Next')]"));
            nextButton.Click();

            //Keep below Thread Sleep until fixing below Commented Explicit waiter.
            Thread.Sleep(TimeSpan.FromSeconds(5));
            //WebDriverWait waitPlaceOrder = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            //waitPlaceOrder.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//button[contains(span/@data-bind, 'Place Order')]")));

            IWebElement placeOrderButton = _driver.FindElement(By.XPath(".//button[contains(span/@data-bind, 'Place Order')]"));
            placeOrderButton.Click();

            WebDriverWait waitOrderNumber = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            waitOrderNumber.Until(ExpectedConditions.ElementExists(By.CssSelector("a.order-number strong")));

            IWebElement orderNumberElement = _driver.FindElement(By.CssSelector("a.order-number strong"));
            string orderNumber = orderNumberElement.Text;

            IWebElement continueShoppingButton = _driver.FindElement(By.XPath(".//div[@class='primary']//span[contains(text(), 'Continue Shopping')]"));
            continueShoppingButton.Click();

            IWebElement customerMenuDropMenu = _driver.FindElement(By.CssSelector("span.customer-name"));
            customerMenuDropMenu.Click();

            IWebElement myAccountLink = _driver.FindElement(By.XPath(".//a[contains(text(), 'My Account')]"));
            myAccountLink.Click();

            IWebElement myOrdersLink = _driver.FindElement(By.XPath(".//a[contains(text(), 'My Orders')]"));
            myOrdersLink.Click();

            IWebElement viewOrderButtonForCreatedOrder = _driver.FindElement(By.XPath($".//td[contains(text(), '{orderNumber}')]/following-sibling::td/a[span[text()='View Order']]"));
            viewOrderButtonForCreatedOrder.Click();


            //Assert
            IWebElement productName = _driver.FindElement(By.XPath(".//td[@class='col name']/strong"));
            IWebElement subtotalAmount = _driver.FindElement(By.XPath(".//tr[@class='subtotal']//span"));
            IWebElement shippingHandlingAmount = _driver.FindElement(By.XPath(".//tr[@class='shipping']//span"));
            IWebElement grandTotalAmount = _driver.FindElement(By.XPath(".//tr[@class='grand_total']//span"));

            var actualProductName = productName.Text;
            var actualSubtotalAmount = subtotalAmount.Text;
            var actualShippingHandlingAmount = shippingHandlingAmount.Text;
            var actualGrandTotalAmount = grandTotalAmount.Text;

            var expectedProductName = "Dash Digital Watch";
            var expectedSubtotal = "$92.00";
            var expectedShippingHandling = "$5.00";
            var expectedGrandTotal = "$97.00";

            Assert.AreEqual(expectedProductName, actualProductName);
            Assert.AreEqual(expectedSubtotal, actualSubtotalAmount);
            Assert.AreEqual(expectedShippingHandling, actualShippingHandlingAmount);
            Assert.AreEqual(expectedGrandTotal, actualGrandTotalAmount);


        }

        [Test]
        public void Scenario2()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_baseUrl);

            //Action
            var mainPage = new MainPage(_driver);
            mainPage.OpenCreateAccountButton();

            IWebElement firstNameInput = _driver.FindElement(By.CssSelector("input[name='firstname']"));
            IWebElement lastNameInput = _driver.FindElement(By.CssSelector("input[name='lastname']"));

            IWebElement passwordInput = _driver.FindElement(By.CssSelector("input[name = 'password']"));
            IWebElement passwordConfirmInput = _driver.FindElement(By.CssSelector("input[name='password_confirmation']"));
            IWebElement createAccountButtonInRegistrationPage = _driver.FindElement(By.CssSelector("button[title='Create an Account']"));

            firstNameInput.SendKeys("Charlie");
            lastNameInput.SendKeys("Great");
            passwordInput.SendKeys("Test123Test");
            passwordConfirmInput.SendKeys("Test123Test");
            createAccountButtonInRegistrationPage.Click();

            //Assertion
            var expectedErrorMessage = "This is a required field.";
            IWebElement errorMessage = _driver.FindElement(By.XPath($".//div[@class='mage-error' and contains(text(), '{expectedErrorMessage}')]"));
            var actualErrorMessage = errorMessage.Text;

            Assert.AreEqual(expectedErrorMessage, actualErrorMessage);

        }


        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}