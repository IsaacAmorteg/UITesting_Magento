using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITesting.Pages
{
    internal class LoginPage
    {
        private By _emailInput = By.Id("email");
        private By _passwordInput = By.Name("login[password]");
        private By _signInButtonLogin = By.Id("send2");
        private By _errorMessage = By.ClassName("fieldset");
       
        public IWebDriver Driver;

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void EnterEmail(string email)
        {
            IWebElement input = Driver.FindElement(_emailInput);
            input.SendKeys(email);

        }
        public void EnterPassword(string value)
        {
            IWebElement input = Driver.FindElement(_passwordInput);
            input.SendKeys(value);

        }

        public void ClickSignInButtonLogin()
        {
            IWebElement button = Driver.FindElement(_signInButtonLogin);
            button.Click();

        }
        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickSignInButtonLogin();

        }

        public string GetErrorMessage()
        {
            IWebElement errorMesage = Driver.FindElement(_errorMessage);
            return errorMesage.GetAttribute("data-hasrequired");

        }
       
    }
}
