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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ShoppingCart() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
