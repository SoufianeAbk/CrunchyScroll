using CrunchyScroll.Helpers;
using CrunchyScroll.Models;
using CrunchyScroll.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CrunchyScroll.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly OrderService _orderService;

        private ObservableCollection<OrderItem> _cartItems = new();
        private string _customerName = string.Empty;
        private string _customerEmail = string.Empty;
        private string _deliveryAddress = string.Empty;

        public ObservableCollection<OrderItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        public string CustomerEmail
        {
            get => _customerEmail;
            set => SetProperty(ref _customerEmail, value);
        }

        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set => SetProperty(ref _deliveryAddress, value);
        }

        public decimal CartTotal => CartItems.Sum(item => item.SubTotal);
        public int CartItemCount => CartItems.Sum(item => item.Quantity);

        public ICommand LoadCartCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand UpdateQuantityCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand ClearCartCommand { get; }

        public OrderViewModel()
        {
            _orderService = new OrderService();

            Title = "Winkelwagen";

            LoadCartCommand = new Command(LoadCart);
            RemoveItemCommand = new Command<OrderItem>(OnRemoveItem);
            UpdateQuantityCommand = new Command<(int productId, int quantity)>(OnUpdateQuantity);
            PlaceOrderCommand = new Command(async () => await OnPlaceOrderAsync());
            ClearCartCommand = new Command(OnClearCart);
        }

        public void LoadCart()
        {
            var items = _orderService.GetCartItems();

            CartItems.Clear();
            foreach (var item in items)
            {
                CartItems.Add(item);
            }

            OnPropertyChanged(nameof(CartTotal));
            OnPropertyChanged(nameof(CartItemCount));
        }

        private void OnRemoveItem(OrderItem item)
        {
            if (item == null)
                return;

            _orderService.RemoveFromCart(item.ProductId);
            LoadCart();
        }

        private void OnUpdateQuantity((int productId, int quantity) data)
        {
            _orderService.UpdateQuantity(data.productId, data.quantity);
            LoadCart();
        }

        private async Task OnPlaceOrderAsync()
        {
            if (IsBusy)
                return;

            // Validatie
            if (!CartItems.Any())
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Lege winkelwagen",
                    "Je winkelwagen is leeg",
                    "OK")!;
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomerName) ||
                string.IsNullOrWhiteSpace(CustomerEmail) ||
                string.IsNullOrWhiteSpace(DeliveryAddress))
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Vereiste velden",
                    "Vul alle velden in om je bestelling te plaatsen",
                    "OK")!;
                return;
            }

            try
            {
                IsBusy = true;

                var order = await _orderService.CreateOrderAsync(
                    CustomerName,
                    CustomerEmail,
                    DeliveryAddress);

                if (order != null)
                {
                    await Application.Current?.MainPage?.DisplayAlert(
                        "Bestelling geplaatst!",
                        $"Je bestelling #{order.Id} is succesvol geplaatst. Totaal: €{order.TotalAmount:F2}",
                        "OK")!;

                    // Reset form
                    CustomerName = string.Empty;
                    CustomerEmail = string.Empty;
                    DeliveryAddress = string.Empty;
                    LoadCart();

                    // Navigate terug naar home
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert(
                        "Fout",
                        "Er is iets misgegaan bij het plaatsen van je bestelling",
                        "OK")!;
                }
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Fout",
                    $"Er is een fout opgetreden: {ex.Message}",
                    "OK")!;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnClearCart()
        {
            bool confirm = await Application.Current?.MainPage?.DisplayAlert(
                "Winkelmand leegmaken",
                "Weet je zeker dat je de winkelwagen wilt leegmaken?",
                "Ja",
                "Nee")!;

            if (confirm)
            {
                _orderService.ClearCart();
                LoadCart();
            }
        }
    }
}
