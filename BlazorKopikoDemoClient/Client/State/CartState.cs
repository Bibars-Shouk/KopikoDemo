using Blazored.LocalStorage;
using BlazorKopikoDemoClient.Client.DataModels;

namespace BlazorKopikoDemoClient.Client.State
{
    public class CartState
    {
        private readonly ILocalStorageService _localstorage;
        public event Action OnChange;

        private List<CartItem> _cartItems = new List<CartItem>();
        public List<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnChange?.Invoke();
            }
        }

        public CartState(ILocalStorageService localstorage)
        {
            _localstorage = localstorage;
        }

        public async Task InitUserCart()
        {
            var userHasCart = await _localstorage.ContainKeyAsync("cart");
            if (!userHasCart)
            {
                CartItems = new List<CartItem>();
                await _localstorage.SetItemAsync<List<CartItem>>("cart", CartItems);
            }
            else
            {
                CartItems = await _localstorage.GetItemAsync<List<CartItem>>("cart");
            }

            OnChange?.Invoke();
        }

        private async Task _updateCartOnLocalStorage()
        {
            await _localstorage.SetItemAsync<List<CartItem>>("cart", CartItems);
        }

        public async Task AddItemToCart(CartItem item)
        {
            CartItem existingItem = CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                CartItems.Add(item);
            }

            await _updateCartOnLocalStorage();
            OnChange?.Invoke();
        }

        public async Task RemoveItemFromCart(long itemId)
        {
            CartItem existingItem = CartItems.FirstOrDefault(i => i.ProductId == itemId);
            CartItems.Remove(existingItem);
            await _updateCartOnLocalStorage();
            OnChange?.Invoke();
        }

        public async Task ClearCart()
        {
            CartItems = new List<CartItem>();
            await _localstorage.SetItemAsync<List<CartItem>>("cart", CartItems);
            OnChange?.Invoke();
        }
    }

}
