using CrunchyScroll.ViewModels;

namespace CrunchyScroll.Views;

public partial class OrderPage : ContentPage
{
    private readonly OrderViewModel _viewModel;

    public OrderPage()
    {
        InitializeComponent();
        _viewModel = new OrderViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCart();
    }
}
