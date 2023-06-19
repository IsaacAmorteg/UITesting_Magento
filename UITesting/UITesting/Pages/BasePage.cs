using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UITesting.Pages
{
    abstract class BasePage
    {
        private By _signInButtonMainPage = By.XPath(".//li[@class='authorization-link']");
        private By _welcomeMessage = By.ClassName("logged-in");
        private By _createAnAccountButton = By.XPath(".//li/a[contains(text(), 'Create an Account')]");
        private By _signInButtonLocator = By.ClassName("authorization-link");
        private By _gearCategoryButtonLocator = By.Id("ui-id-6");
        private By _watchesPageByDropDownButtonLocator = By.XPath(".//a[@id= 'ui-id-27']");
        private By _customerMenuDropDown = By.CssSelector("span.customer-name");
        private By _myAccountButton = By.XPath(".//a[contains(text(), 'My Account')]");

        protected IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void OpenSignInPage()
        {
            Driver.FindElement(_signInButtonMainPage).Click();

        }

        public void OpenCreateAccountPage()
        {
            Driver.FindElement(_createAnAccountButton).Click();
        }
        public void OpenCustomerMenuDropDown()
        {
            Driver.FindElement(_customerMenuDropDown).Click();         

        }
        public void OpenMyAccountPage()
        {
            Driver.FindElement(_myAccountButton).Click();
        }


        public void  MoveToGearDropDownMenu()
        {
            WebDriverWait waitGearButton = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            waitGearButton.Until(ExpectedConditions.ElementToBeClickable(_gearCategoryButtonLocator));

            var element = Driver.FindElement(_gearCategoryButtonLocator);

            Actions actions = new Actions(Driver);

            actions.MoveToElement(element).Perform();

        }

        public void OpenWatchesPageByDropDownMenu()
        {
            Driver.FindElement(_watchesPageByDropDownButtonLocator).Click();
           
        }

        public string GetWelcomeMessage()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(2));
            wait.Until((driver) => driver.FindElement(By.ClassName("logged-in")).Text.StartsWith("Welcome, "));
            return Driver.FindElement(_welcomeMessage).Text;
        }

        public bool isSignInButtonDisplayed()
        {
            return Driver.FindElements(_signInButtonLocator).Select(i => i.Displayed).Distinct().Single();
        }

        public string GetSignInButtonText()
        {
            return Driver.FindElement(_signInButtonLocator).Text;
        }

    }
}
