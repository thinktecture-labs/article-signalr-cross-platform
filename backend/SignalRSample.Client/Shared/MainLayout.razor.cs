namespace SignalRSample.Client.Shared
{
    public partial class MainLayout
    {
        private bool _navMenuOpened;

        private void MenuButtonClicked()
        {
            _navMenuOpened = !_navMenuOpened;
            StateHasChanged();
        }
    }
}