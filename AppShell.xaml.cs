using CrunchyScroll.Views;

namespace CrunchyScroll
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
        }
    }
}
