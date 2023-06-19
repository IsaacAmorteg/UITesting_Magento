using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class SuccessPage : BasePage
    {
        private By _orderNumberLocator = By.CssSelector("a.order-number strong");
        private By _continueShoppingButton = By.XPath(".//div[@class='primary']//span[contains(text(), 'Continue Shopping')]");
        public SuccessPage(IWebDriver driver) : base(driver)
        {
        }

        public string SaveOrderNumber()
        {
            WebDriverWait waitOrderNumber = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            waitOrderNumber.Until(ExpectedConditions.ElementExists(_orderNumberLocator));

            IWebElement orderNumberElement = Driver.FindElement(_orderNumberLocator);
            string result = orderNumberElement.Text;
            return result;

        }

        public void ClickContinueShopping()
        {
            IWebElement continueShoppingButton = Driver.FindElement(_continueShoppingButton);
            continueShoppingButton.Click();
        }

    }
}
