using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SignalRSample.Client.Components
{
    public partial class HistoryButton
    {
        [Parameter] public EventCallback<MouseEventArgs> OnHistoryClicked { get; set; }
    }
}