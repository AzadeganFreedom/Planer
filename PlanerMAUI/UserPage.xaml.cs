using Newtonsoft.Json;
using PlanerMAUI.Models;
using PlanerMAUI.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlanerMAUI;

public partial class UserPage : ContentPage
{

    private readonly HttpClient httpClient = new HttpClient();

    public UserPage()
	{
		InitializeComponent();

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

        // Fill input fields with current user data
        var currentUser = SessionService.CurrentUser;
        if (currentUser != null)
        {
            usernameEntry.Text = currentUser.UserName;
            emailEntry.Text = currentUser.Email;
            passwordEntry.Text = string.Empty;
        }

        mainLayout.Opacity = 0;
        await mainLayout.FadeTo(1, 1000);
    }

    // Edit user
    private async void OnEditUserClicked(object sender, EventArgs e)
    {
        var updateUser = new User
        {
            UserName = usernameEntry.Text,
            Email = emailEntry.Text,
            Password = passwordEntry.Text
        };

        try
        {
            var json = JsonConvert.SerializeObject(updateUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"api/User/UpdateUserByUserName?userName={SessionService.CurrentUser.UserName}", content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Succes", "you have succesfully edited your user!", "OK");

                await mainLayout.FadeTo(0, 500);

                SessionService.CurrentUser.UserName = updateUser.UserName;
                SessionService.CurrentUser.Email = updateUser.Email;

                // Clears the inputfields
                usernameEntry.Text = string.Empty;
                emailEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;

                await Shell.Current.GoToAsync("///MainPage");
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


    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await mainLayout.FadeTo(0, 500);

        // Clears the inputfields
        usernameEntry.Text = string.Empty;
        emailEntry.Text = string.Empty;
        passwordEntry.Text = string.Empty;

        await Shell.Current.GoToAsync("//MainPage");
    }
}