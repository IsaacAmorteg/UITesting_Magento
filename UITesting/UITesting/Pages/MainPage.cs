using OpenQA.Selenium;


namespace UITesting.Pages
{
    internal class MainPage
    {
        private By _signInButtonMainPage = By.XPath(".//li[@class='authorization-link']");
        private By _welcomeMessage = By.ClassName("logged-in");
        private By _createAnAccountButton = By.XPath(".//li/a[contains(text(), 'Create an Account')]");

        public IWebDriver Driver;

        public MainPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void OpenSignInButton()
        {
            Driver.FindElement(_signInButtonMainPage).Click();           

        }

        public void OpenCreateAccountButton()
        {
           Driver.FindElement(_createAnAccountButton).Click();            
        }
                
         public string GetWelcomeMessage()
        {
            return Driver.FindElement(_welcomeMessage).Text;            
        }
       
    }
}
