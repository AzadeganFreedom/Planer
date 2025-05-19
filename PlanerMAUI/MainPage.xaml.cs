using PlanerMAUI.Services;

namespace PlanerMAUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            var user = SessionService.CurrentUser;
            //Title = $"{user?.UserName ?? "Guest"}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SessionService.CurrentUser != null)
            {
                // Set page title to the username
                Title = $"{SessionService.CurrentUser.UserName}";
            }
            else
            {
                Title = "Guest"; 
            }

            // Check orientation on appearing
            AdjustLayoutForOrientation();

            // Subscribe to orientation changes
            this.SizeChanged += OnSizeChanged;

            mainLayout.Opacity = 0;
            mainLayout.FadeTo(1, 1000);
        }

        private void AdjustLayoutForOrientation()
        {
            bool isLandscape = this.Width > this.Height;

            // Image visibility
            hovercraftImage.IsVisible = !isLandscape;

            // Adjust button widths
            double buttonWidth = isLandscape ? 256 : 128;

            viewTasksButton.WidthRequest = buttonWidth;
            addTasksButton.WidthRequest = buttonWidth;
            editUserButton.WidthRequest = buttonWidth;
            logoutButton.WidthRequest = buttonWidth;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            AdjustLayoutForOrientation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.SizeChanged -= OnSizeChanged;
        }

        // Navigate to ViewListPage
        private async void OnViewListClicked(object sender, EventArgs e)
        {
            await mainLayout.FadeTo(0, 500);
            await Shell.Current.GoToAsync("//ViewListPage");
        }

        // Navigate to AddPage
        private async void OnAddToListClicked(object sender, EventArgs e)
        {
            await mainLayout.FadeTo(0, 500);
            await Shell.Current.GoToAsync("//AddPage");
        }

        // Navigate to UserPage
        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            await mainLayout.FadeTo(0, 500);
            await Shell.Current.GoToAsync("//UserPage");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await mainLayout.FadeTo(0, 500);
            SessionService.CurrentUser = null;

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
