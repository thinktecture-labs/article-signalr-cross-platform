using MatBlazor;
using Microsoft.JSInterop;

namespace SignalRSample.Client
{
    public partial class App
    {
        private readonly MatTheme _theme = new MatTheme()
        {
            Primary = MatThemeColors.Orange._500.Value,
            Secondary = MatThemeColors.BlueGrey._500.Value
        };

        [JSInvokable("NotifyError")]
        public static void NotifyError(string error)
        {
            // TODO: Show Dialog.
        }
    }
}