using System;
using Microsoft.AspNetCore.Components;

namespace SignalRSample.Client.Components
{
    public partial class RedirectToLogin
    {
        [Inject] private NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
        }
    }
}