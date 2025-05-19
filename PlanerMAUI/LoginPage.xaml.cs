using PlanerMAUI.Models;
using System.Linq.Expressions;
using System.Net.Http.Json;
using PlanerMAUI.Services;

namespace PlanerMAUI;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient httpClient;

	public LoginPage()
	{
		InitializeComponent();

        //httpClient = new HttpClient
        //{
        //    BaseAddress = new Uri("Https://10.0.2.2:7077/")
        //};

        #if ANDROID
                    // Only on Android: bypass certificate validation
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                    httpClient = new HttpClient(handler)
                    {
                        BaseAddress = new Uri("https://10.0.2.2:7077/")
                    };
        #else
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:7077/")
                };
        #endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        mainLayout.Opacity = 0;
        mainLayout.FadeTo(1, 300);
    }

    // Navigate to Register Page
    private async void OnNavigateToRegister(object sender, EventArgs e)
    {
        await mainLayout.FadeTo(0, 250);
        await Shell.Current.GoToAsync("///RegisterPage");
    }

    // Login method
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = usernameEntry.Text;
        var password = passwordEntry.Text;

        if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Validation Error", "PLease enter both username and password.", "OK");
            return;
        }

        try
        {
            var loginPayload = new 
            { 
                UserName = username, 
                Password = password 
            };

            var response = await httpClient.PostAsJsonAsync($"api/User/Login",loginPayload);

            if(response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();

                if (user != null)
                {
                    // Fade out
                    await mainLayout.FadeTo(0, 250);

                    // Store user in session service
                    SessionService.CurrentUser = user;

                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                    // Clears the inputfields
                    usernameEntry.Text = string.Empty;
                    passwordEntry.Text = string.Empty;

                    // Navigate
                    await Shell.Current.GoToAsync("//MainPage", true);
                }
                else
                {
                    await DisplayAlert("Login Failed", "User not found or incorrect credentials.", "OK");
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Login Failed", error, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }
}