using PlanerMAUI.Models;
using PlanerMAUI.Services;
using System.Linq.Expressions;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlanerMAUI;

public partial class AddPage : ContentPage
{

    private readonly HttpClient httpClient;

    public AddPage()
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

        mainLayout.Opacity = 0;
        mainLayout.FadeTo(1, 300);
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await mainLayout.FadeTo(0, 250);
        taskEntry.Text = string.Empty;
        await Shell.Current.GoToAsync("///MainPage");
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var newItem = new Item
        {
            Description = taskEntry.Text,
            Created = DateTime.Now,
            ListId = SessionService.CurrentUser.Id
        };
        var task = taskEntry.Text;

        if (string.IsNullOrWhiteSpace(task))
        {
            await DisplayAlert("Validation Error", "PLease enter a task.", "OK");
            return;
        }

        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/Item/CreateItem?ListId={SessionService.CurrentUser.Id}", newItem);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Succes", $"Task added to your list!", "OK");

                // Clears inputfield
                taskEntry.Text = string.Empty;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Adding task failed: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }
}