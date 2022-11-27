namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string UserName {get; set;}
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
        public ShoppingCart() { }
        public decimal TotalPrice
        {
            get
            {
                decimal totalProce = 0;
                foreach (ShoppingCartItem item in Items)
                {
                    totalProce += item.Price * item.Quantity;
                }
                return totalProce;
            }
        }
    }
}
