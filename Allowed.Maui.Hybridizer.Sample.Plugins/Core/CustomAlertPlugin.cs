using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace Allowed.Maui.Hybridizer.Sample.Plugins.Core;

[HwvPlugin(Name = "SampleAlert")]
public class CustomAlertPlugin
{
    [HwvMethod(Name = "ShowAlert")]
    public void Show(Page page)
    {
        page.Dispatcher.Dispatch(() =>
        {
            page.DisplayAlert("Title", "Custom alert", "Accept", "Cancel", FlowDirection.RightToLeft);
        });
    }
}