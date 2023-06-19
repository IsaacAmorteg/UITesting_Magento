﻿using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class ProductCategoryPage : BasePage
    {
        private By _dashDigitalWatchProductLocator = By.XPath(".//li[@class='item product product-item']//a[contains(text(), 'Dash Digital Watch')]");
        private By _miniCart = By.ClassName("minicart-wrapper");
        private By _proceedToCheckoutFromMiniCart = By.Id("top-cart-btn-checkout");
        private By _hotSellersLocator = By.ClassName("product-item-link");
        public ProductCategoryPage(IWebDriver driver) : base(driver)
        {
        }

        public IEnumerable<string> GetHotSellersNames()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(2));
            wait.Until(driver => driver.FindElements(_hotSellersLocator).Count == 4);
            wait.Until(driver => driver.FindElements(_hotSellersLocator).All(i => i.Text != string.Empty));
            IEnumerable<IWebElement> productNameCollection = Driver.FindElements(_hotSellersLocator);
            var result = productNameCollection.Select(i => i.Text);
            return result;
        }

        public void AddToCartDashDigitalWatchProduct()
        {
            IWebElement productBox = Driver.FindElement(_dashDigitalWatchProductLocator);
            Actions actions = new Actions(Driver);
            actions.MoveToElement(productBox).Build().Perform();

            IWebElement addToCartButton = Driver.FindElement(By.XPath(".//form[@data-product-sku= '24-MG02']/button[@class='action tocart primary']"));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript("arguments[0].click();", addToCartButton);
        }

        public void OpenMiniCart()
        {
            IWebElement showMiniCartCheckoutButton = Driver.FindElement(_miniCart);
            showMiniCartCheckoutButton.Click();
        }
        public void ProceedToCheckOutFromMiniCart()
        {
            WebDriverWait waitCheckout = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            waitCheckout.Until(ExpectedConditions.ElementToBeClickable(_proceedToCheckoutFromMiniCart));

            IWebElement proceedToCheckoutButton = Driver.FindElement(_proceedToCheckoutFromMiniCart);
            proceedToCheckoutButton.Click();
        }
    }
}
