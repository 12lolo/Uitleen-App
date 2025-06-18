using uitleen_app.Models;

namespace uitleen_app.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiClient _apiClient;

        public LoginPage(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                ActivityIndicator.IsVisible = true;
                ActivityIndicator.IsRunning = true;

                var loginData = new
                {
                    email = EmailEntry.Text,
                    password = PasswordEntry.Text
                };

                var response = await _apiClient.PostAsync<LoginResponse>("/login", loginData);

                if (!string.IsNullOrEmpty(response?.Token))
                {
                    _apiClient.SetAuthToken(response.Token);
                    await DisplayAlert("Succes", "U bent ingelogd!", "OK");
                    await Navigation.PopAsync(); // Go back to previous page after login
                }
                else
                {
                    await DisplayAlert("Fout", "Inloggen mislukt. Controleer uw gegevens.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Inloggen mislukt: {ex.Message}", "OK");
            }
            finally
            {
                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Navigate to registration page
            // await Navigation.PushAsync(new RegisterPage(_apiClient));
            await DisplayAlert("Info", "Registratie functionaliteit komt binnenkort beschikbaar.", "OK");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}