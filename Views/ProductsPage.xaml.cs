using CrunchyScroll.ViewModels;

namespace CrunchyScroll.Views;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsViewModel _viewModel;

    public ProductsPage()
    {
        InitializeComponent();
        _viewModel = new ProductsViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}
