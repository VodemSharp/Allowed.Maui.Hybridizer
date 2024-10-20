using Microsoft.Maui.Controls;

namespace Allowed.Maui.Hybridizer.Contexts;

public class HwvContext
{
    public Page Page { get; set; } = null!;
    public HybridWebView HybridWebView { get; set; } = null!;

    public void Initialize(Page page, HybridWebView hybridWebView)
    {
        Page = page;
        HybridWebView = hybridWebView;
    }
}