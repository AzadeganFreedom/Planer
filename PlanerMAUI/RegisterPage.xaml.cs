using PlanerMAUI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace PlanerMAUI;

public partial class RegisterPage : ContentPage
{
    private readonly HttpClient httpClient;

    public RegisterPage()
    {
        InitializeComponent();
        //httpClient = new HttpClient
        //{
        //    BaseAddress = new Uri("https://10.0.2.2:7077/")
        //};

        // Avoids SSL Validation
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

        // Make sure main layout is reset and slides in smoothly
        mainLayout.Opacity = 0;
        mainLayout.FadeTo(1, 300);
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var newUser = new User
        {
            UserName = usernameEntry.Text,
            Email = emailEntry.Text,
            Password = passwordEntry.Text,
            
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("api/User/CreateUser", newUser);
            if (response.IsSuccessStatusCode)
            {

                // Clears the inputfields
                usernameEntry.Text = string.Empty;
                emailEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;

                await mainLayout.FadeTo(0, 250);
                await Shell.Current.GoToAsync("///LoginPage");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Registration failed: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }

    private async void OnNavigateToLogin(object sender, EventArgs e)
    {
        // Clears the inputfields
        usernameEntry.Text = string.Empty;
        emailEntry.Text = string.Empty;
        passwordEntry.Text = string.Empty;

        await mainLayout.FadeTo(0, 250);
        await Shell.Current.GoToAsync("///LoginPage");
    }
}
