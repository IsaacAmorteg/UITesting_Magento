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
    internal class CheckoutPage : BasePage
    {
        private By _newAddressButtonLocator = By.XPath(".//button[contains(span/@data-bind, 'New Address')]");
        private By _shipHereButtonLocator = By.XPath(".//footer//span[contains(text(), 'Ship here')]");
        private By _firstNameInput = By.CssSelector("input[name='firstname']");
        private By _lastNameInput = By.CssSelector("input[name='lastname']");
        private By _streetInput = By.CssSelector("input[name='street[0]']");
        private By _cityInput = By.CssSelector("input[name='city']");
        private By _countryInput = By.CssSelector("select[name='country_id']");
        private By _zipPostalCodeInput = By.CssSelector("input[name ='postcode']");
        private By _phoneInput = By.CssSelector("input[name='telephone']");
        private By _nextButton = By.XPath(".//button[contains(span/@data-bind, 'Next')]");
        private By _placeOrderButton = By.XPath(".//button[contains(span/@data-bind, 'Place Order')]");

        public CheckoutPage(IWebDriver driver) : base(driver)
        {
        }

        public void isNewAddressButtonPresentClick()
        {
            bool isNewAddressButtonPresent = Driver.FindElements(_newAddressButtonLocator).Count > 0;

            if (isNewAddressButtonPresent)
            {
                IWebElement newAddressButton = Driver.FindElement(_newAddressButtonLocator);
                newAddressButton.Click();
            }

        }
        public void isShipHereButtonPresentClick()
        {
            bool isShipHereButtonPresent = Driver.FindElements(_shipHereButtonLocator).Count > 0;

            if (isShipHereButtonPresent)
            {
                IWebElement shipHereButton = Driver.FindElement(_shipHereButtonLocator);
                shipHereButton.Click();
            }

        }

        public void EnterFirstName(string firstName)
        {
            IWebElement input = Driver.FindElement(_firstNameInput);
            input.Clear();
            input.SendKeys(firstName);            
           
        }
        public void EnterLastName(string lastName)
        {
            IWebElement input = Driver.FindElement(_lastNameInput);
            input.Clear();
            input.SendKeys(lastName);
        }
        public void EnterStreetAddress(string streetAddress)
        {
            IWebElement input = Driver.FindElement(_streetInput);
            input.Clear();
            input.SendKeys(streetAddress);
        }
        public void EnterCity(string city)
        {
            IWebElement input = Driver.FindElement(_cityInput);
            input.Clear();
            input.SendKeys(city);
        }
        public void SelectCountry(string country)
        {
            IWebElement input = Driver.FindElement(_countryInput);
            SelectElement selectCountry = new SelectElement(input);
            selectCountry.SelectByText(country);            
        }
        public void EnterZipPostalCode(string zipPostalCode)
        {
            IWebElement input = Driver.FindElement(_zipPostalCodeInput);
            input.Clear();
            input.SendKeys(zipPostalCode);
        }
        public void EnterPhoneNumber(string phoneNumber)
        {
            IWebElement input = Driver.FindElement(_phoneInput);
            input.Clear();
            input.SendKeys(phoneNumber);
        }
        public string RandomPhoneNumberCreator()
        {
            Random random = new Random();
            string phoneNumber = "";
            for (int i = 0; i < 10; i++)
            {
                phoneNumber += random.Next(0, 10).ToString();
            }
            return phoneNumber;
        }
        public void ClickNextToPlaceOrder()
        {
           Driver.FindElement(_nextButton).Click();         
        }

        public void ClickPlaceOrder()
        {
            Driver.FindElement(_placeOrderButton).Click();            
        }





            public void FillInCheckoutData(string firstName, string lastName, string streetAddress, string city, string country, string zipPostalCode, string phoneNumber)
        {
            WebDriverWait waitNext = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            waitNext.Until(ExpectedConditions.ElementExists(_firstNameInput));

            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterStreetAddress(streetAddress);
            EnterCity(city);
            SelectCountry(country);
            EnterZipPostalCode(zipPostalCode);
            EnterPhoneNumber(phoneNumber);
        }
    }
}
