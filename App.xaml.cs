using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration;
using uitleen_app.Views;

namespace uitleen_app
{
    public partial class App : Application
    {
        public App()    
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Get the LandingPage from the service provider
            var landingPage = Handler?.MauiContext?.Services.GetService<LandingPage>();
            
            // Create a navigation page with LandingPage as the root
            var navPage = new NavigationPage(landingPage);
            
            // Return a new window with the navigation page
            return new Window(navPage);
        }
    }
}