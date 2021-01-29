using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SignalRSample.Client.Pages
{
    public partial class Index
    {
        [Inject] private NavigationManager Navigation { get; set; }

        private void BeginSignIn(MouseEventArgs args)
        {
            Navigation.NavigateTo("authentication/login");
        }
    }
}