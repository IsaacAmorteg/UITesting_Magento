using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class CreateAccountPage : BasePage
    {
        private By _firstNameInput = By.CssSelector("input[name='firstname']");
        private By _lastNameInput = By.CssSelector("input[name='lastname']");
        private By _passwordInput = By.CssSelector("input[name='password']");
        private By _passwordConfirmInput = By.CssSelector("input[name='password_confirmation']");
        private By _createAnAccountButton = By.CssSelector("button[title='Create an Account']");
        public CreateAccountPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterFirstName(string firstName)
        {
            IWebElement input = Driver.FindElement(_firstNameInput);
            input.SendKeys(firstName);

        }
        public void EnterLastName(string lastName)
        {
            IWebElement input = Driver.FindElement(_lastNameInput);
            input.SendKeys(lastName);

        }
        public void EnterPassword(string value)
        {
            IWebElement input = Driver.FindElement(_passwordInput);
            IWebElement inputConfirm = Driver.FindElement(_passwordConfirmInput);
            input.SendKeys(value);
            inputConfirm.SendKeys(value);
        }
        public void ClickCreateAnAccountButton()
        {
            Driver.FindElement(_createAnAccountButton).Click();          

        }
        public string GetErrorMessage(string expectedErrorMessage)
        {                  
            return Driver.FindElement(By.XPath($".//div[@class='mage-error' and contains(text(), '{expectedErrorMessage}')]")).Text;

        }
        public void CreateAnAccount(string firstName, string lastName, string value)
        {
            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterPassword(value);
            ClickCreateAnAccountButton();
        }

    }
}
