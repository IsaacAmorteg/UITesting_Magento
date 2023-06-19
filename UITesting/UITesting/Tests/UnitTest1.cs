using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using UITesting.Pages;

namespace UITesting.Tests
{
    public class Tests
    {
        [ThreadStatic]
        private static IWebDriver _driver;
        private readonly string _baseUrl = "https://magento.softwaretestingboard.com/";
        private readonly string _gearPageUrl = "https://magento.softwaretestingboard.com/gear.html";
        private static MainPage _mainPage;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");

            _driver = new ChromeDriver(options);

            _driver.Manage().Window.Maximize();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            _driver.Navigate().GoToUrl(_baseUrl);            

            _mainPage = new MainPage(_driver);
        }

        [TestCase("https://magento.softwaretestingboard.com/", "Home Page")]
        [TestCase("https://magento.softwaretestingboard.com/gear.html", "Gear")]
        public void NewBrowserWindow_OpenPage_TitleIsCorrect(string url, string expectedTitle)
        {

            _driver.Navigate().GoToUrl(url);
            Assert.AreEqual(expectedTitle, _driver.Title);

        }
        [Test]
        public void LogoutUser_CheckSignInButtonText_IsSignIn()
        {           
            var actual = _mainPage.GetSignInButtonText();
            var expected = "Sign In";

            Assert.AreEqual(expected, actual);

        }
                
        [TestCase("https://magento.softwaretestingboard.com/gear.html")]
        public void BasePageOpened_SignIn_WelcomeMessageIsDisplayed(string url)
        {
            //Precondition
            _driver.Navigate().GoToUrl(url);

            //Action
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInPage();

            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");                            

            //Assert
            var actual = mainPage.GetWelcomeMessage();
            var expected = "Welcome, Isaac Amortegui!";

            Assert.AreEqual(expected, actual);

        }

        [Test]

        public void BasePageOpened_TrySignInWithNoPassword_ErrorMessageIsDisplayed()
        {
            //Precondition
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInPage();

            //Action
            var loginPage = new LoginPage(_driver);
            loginPage.EnterEmail("isaacamortegc@outlook.com");
            loginPage.ClickSignInButtonLogin();

            //Assert
            var actual = loginPage.GetErrorMessage();
            var expected = "* Required Fields";

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void BasePageOpened_SignIn_MainPageNotSignInButton()
        {
            //Precondition
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInPage();

            //Action
            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");

            var isSignInButtonDisplayed = mainPage.isSignInButtonDisplayed();

            //Assert
            Assert.IsFalse(isSignInButtonDisplayed);

        }
        [Test]
        public void OpenGearPage_GetHotSellers_ProductsAreCorrect()
        {
            //Precondition
            _driver.Navigate().GoToUrl(_gearPageUrl);

            var productCategoryPage = new ProductCategoryPage(_driver);

            //Action
            var actual = productCategoryPage.GetHotSellersNames();
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
            var mainPage = new MainPage(_driver);
            mainPage.OpenSignInPage();

            var loginPage = new LoginPage(_driver);
            loginPage.Login("isaacamortegc@outlook.com", "Boeing787");

            //Action
            mainPage.MoveToGearDropDownMenu();

            Actions actions = new Actions(_driver);

            mainPage.OpenWatchesPageByDropDownMenu();

            var watchesProductPage = new ProductCategoryPage(_driver);
            watchesProductPage.AddToCartDashDigitalWatchProduct();

            //Keep below Thread Sleep until fixing below Commented Explicit waiter.
            Thread.Sleep(TimeSpan.FromSeconds(3));
            // WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            //wait.Until(ExpectedConditions.ElementExists(By.ClassName("minicart-wrapper")));

            watchesProductPage.OpenMiniCart();

            watchesProductPage.ProceedToCheckOutFromMiniCart();

            var checkoutPage = new CheckoutPage(_driver);

            WebDriverWait waitNext = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            waitNext.Until(ExpectedConditions.ElementExists(By.XPath(".//button[contains(span/@data-bind, 'Next')]")));

            checkoutPage.isNewAddressButtonPresentClick();
            
            checkoutPage.FillInCheckoutData("Isaac", "Amortegui", "Spur Road", "London", "United Kingdom", "SW1A 1AA", checkoutPage.RandomPhoneNumberCreator());

            checkoutPage.isShipHereButtonPresentClick();

            checkoutPage.ClickNextToPlaceOrder();

            //Keep below Thread Sleep until fixing below Commented Explicit waiter.
            Thread.Sleep(TimeSpan.FromSeconds(6));
            //WebDriverWait waitPlaceOrder = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            //waitPlaceOrder.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//button[contains(span/@data-bind, 'Place Order')]")));

            checkoutPage.ClickPlaceOrder();

            var successPage = new SuccessPage(_driver);
            var orderNumber = successPage.SaveOrderNumber();
            successPage.ClickContinueShopping();


            mainPage.OpenCustomerMenuDropDown();

            mainPage.OpenMyAccountPage();

            var myAccountPage = new MyAccountPage(_driver);

            myAccountPage.ClickMyOrdersPage();


            myAccountPage.ClickViewOrderByCreatedOrder(orderNumber);

            //Assert

            var actualProductName = myAccountPage.GetProductNameInViewOrder();
            var actualSubtotalAmount = myAccountPage.GetSubtotalInViewOrder();
            var actualShippingHandlingAmount = myAccountPage.GetShipphingHandlingInViewOrder();
            var actualGrandTotalAmount = myAccountPage.GetGrandTotalInViewOrder();

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
            var mainPage = new MainPage(_driver);
            mainPage.OpenCreateAccountPage();

            //Action
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