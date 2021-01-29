using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace SignalRSample.Client.Components
{
    public partial class Login
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private SignOutSessionStateManager SignOutManager { get; set; }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();

            Navigation.NavigateTo("authentication/logout");
        }
    }
}