using CrunchyScroll.ViewModels;

namespace CrunchyScroll.Views;

public partial class OrderHistoryPage : ContentPage
{
    private readonly OrderHistoryViewModel _viewModel;

    public OrderHistoryPage()
    {
        InitializeComponent();
        _viewModel = new OrderHistoryViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadOrdersAsync();
    }
}