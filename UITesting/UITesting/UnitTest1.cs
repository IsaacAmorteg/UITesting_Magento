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

            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement showMiniCartCheckoutButton = _driver.FindElement(By.ClassName("minicart-wrapper"));
            showMiniCartCheckoutButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement proceedToCheckoutButton = _driver.FindElement(By.Id("top-cart-btn-checkout"));
            proceedToCheckoutButton.Click();

            Thread.Sleep(TimeSpan.FromSeconds(5));

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
                   

            Thread.Sleep(TimeSpan.FromSeconds(5));

            IWebElement placeOrderButton = _driver.FindElement(By.XPath(".//button[contains(span/@data-bind, 'Place Order')]"));
            placeOrderButton.Click();

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


        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}