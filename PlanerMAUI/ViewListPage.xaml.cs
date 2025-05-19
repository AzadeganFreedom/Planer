using Newtonsoft.Json;
using PlanerMAUI.Models;
using PlanerMAUI.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace PlanerMAUI;

public partial class ViewListPage : ContentPage
{
    private readonly HttpClient httpClient = new HttpClient();
    public ViewListPage()
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
        await LoadListAsync();
        await mainLayout.FadeTo(1, 1000);

    }

    private ObservableCollection<Item> itemsCollection = new ObservableCollection<Item>();

    private async Task LoadListAsync()
    {
        try
        {
            int userId = SessionService.CurrentUser.Id;

            var response = await httpClient.GetAsync($"api/List/GetListByUserId?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var listData = JsonConvert.DeserializeObject<List>(json);

                if (listData?.Items == null || !listData.Items.Any())
                {
                    TaskLabel.Text = "No Tasks assigned!";
                    itemsCollection.Clear();
                }
                else
                {
                    TaskLabel.Text = "Your Task";
                    itemsCollection.Clear();
                    foreach (var item in listData.Items.OrderBy(i => i.Id))
                    {
                        itemsCollection.Add(item);
                    }
                }
                ItemsCollectionView.ItemsSource = itemsCollection;
            }
            else
            {
                await DisplayAlert("Error", "Failed to load list.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Item itemToDelete)
        {
            try
            {
                // Call your API to delete the item by Id
                var response = await httpClient.DeleteAsync($"api/Item/DeleteItemById={itemToDelete.Id}");
                if (response.IsSuccessStatusCode)
                {
                    itemsCollection.Remove(itemToDelete);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete task.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await mainLayout.FadeTo(0, 500);

        // Clears ItemsCollectionView
        ItemsCollectionView.ItemsSource = Array.Empty<Item>();

        await Shell.Current.GoToAsync("//MainPage");
    }
}