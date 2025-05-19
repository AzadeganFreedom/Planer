using PlanerMAUI.Models;
using PlanerMAUI.Services;

namespace PlanerMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SessionService.CurrentUser = null;

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
