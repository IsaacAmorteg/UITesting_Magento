using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class BagsPage : BasePage
    {
        private By _productItemInfoElementLocator = By.ClassName("product-item-info");
        private By _productInfoNames = By.ClassName("product-item-link");
        private By _thirdProductPage = By.XPath(".//a[contains(text(), 'Driven Backpack')]");

        public BagsPage(IWebDriver driver) : base(driver)
        {
        }

        public IEnumerable<string> GetProductInfoNames()
        {
            var elements = Driver.FindElements(_productItemInfoElementLocator);

            IEnumerable<IWebElement> productInfoNames = elements
                .Select(i => i.FindElement(_productInfoNames));

            IEnumerable<string> actual = productInfoNames.Select(i => i.Text);
            return actual;
        }

        public void AddFirstProductToCart()
        {
            IWebElement productBox = Driver.FindElement(By.XPath($".//a[contains(text(), 'Push It Messenger Bag')]"));
            Actions actions = new Actions(Driver);
            actions.MoveToElement(productBox).Build().Perform();

            IWebElement addToCartButton = Driver.FindElement(By.XPath(".//form[@data-product-sku= '24-WB04']/button[@class='action tocart primary']"));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript("arguments[0].click();", addToCartButton);
        }


        public void AddSecondProductToCart()
        {            
            IWebElement productBox = Driver.FindElement(By.XPath($".//a[contains(text(), 'Overnight Duffle ')]"));
            Actions actions = new Actions(Driver);
            actions.MoveToElement(productBox).Build().Perform();

            IWebElement addToCartButton = Driver.FindElement(By.XPath(".//form[@data-product-sku= '24-WB07']/button[@class='action tocart primary']"));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript("arguments[0].click();", addToCartButton);
        }

        public void OpenThirdProductPage()
        {
            Driver.FindElement(_thirdProductPage).Click();
        }

        public void AddFirstTwoProductsAndOpenThirdProductPage()
        {
            AddFirstProductToCart();
            AddSecondProductToCart();
            OpenThirdProductPage();
        }
    }
}
