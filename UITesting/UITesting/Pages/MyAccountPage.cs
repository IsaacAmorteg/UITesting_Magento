using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class MyAccountPage : BasePage
    {
        private By _myOrdersButton = By.XPath(".//a[contains(text(), 'My Orders')]");     
        private By _productName = By.XPath(".//td[@class='col name']/strong");
        private By _subtotalAmount = By.XPath(".//tr[@class='subtotal']//span");
        private By _shippingHandlingAmount = By.XPath(".//tr[@class='shipping']//span");
        private By _grandTotalAmount = By.XPath(".//tr[@class='grand_total']//span");
        public MyAccountPage(IWebDriver driver) : base(driver)
        {
        }

        public void ClickMyOrdersPage()
        {
            Driver.FindElement(_myOrdersButton).Click();           
        }

        public void ClickViewOrderByCreatedOrder(string orderNumber)
        {
            Driver.FindElement(By.XPath($".//td[contains(text(), '{orderNumber}')]/following-sibling::td/a[span[text()='View Order']]")).Click();            
        }

        public string GetProductNameInViewOrder()
        {
            return Driver.FindElement(_productName).Text;
        }
        public string GetSubtotalInViewOrder()
        {
            return Driver.FindElement(_subtotalAmount).Text;
        }
        public string GetShipphingHandlingInViewOrder()
        {
            return Driver.FindElement(_shippingHandlingAmount).Text;
        }
        public string GetGrandTotalInViewOrder()
        {
           return Driver.FindElement(_grandTotalAmount).Text;
        }
    }
}
