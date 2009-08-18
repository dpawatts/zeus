using System;
using System.Collections.Generic;
using System.Web;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Services
{
    public class ShoppingCartService
    {
        private const string SESSION_STATE_KEY = "ZeusAddInsECommerceShoppingBasket";

        private readonly IWebContext _webContext;

        public ShoppingBasketService(IWebContext webContext)
        {
            _webContext = webContext;
        }

        public IShoppingBasket GetCurrentShoppingBasket(HttpSessionStateBase sessionState)
        {
            return GetCurrentShoppingBasketInternal(sessionState);
        }

        private static ShoppingBasket GetCurrentShoppingBasketInternal(HttpSessionStateBase sessionState)
        {
            ShoppingBasket shoppingBasket = sessionState[SESSION_STATE_KEY] as ShoppingBasket;
            if (shoppingBasket == null)
                sessionState[SESSION_STATE_KEY] = shoppingBasket = new ShoppingBasket();

            // If any of the cards in the shopping basket have been deleted in the admin site, remove them now.
            shoppingBasket.Items.RemoveAll(i => i.Card == null);

            return shoppingBasket;
        }

        public void AddItemToShoppingBasket(HttpSessionStateBase sessionState, Card card, int quantity)
        {
            if (quantity < 1)
                throw new ArgumentOutOfRangeException("quantity", "Quantity must be greater than or equal to 1.");

            quantity = GetCorrectQuantity(quantity);

            ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(sessionState);

            // If card is already in basket, just increment quantity, otherwise create a new item.
            ShoppingBasketItem item = shoppingBasket.Items.SingleOrDefault(i => i.Card == card);
            if (item == null)
            {
                item = new ShoppingBasketItem(card, quantity);
                shoppingBasket.Items.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void UpdateQuantity(HttpSessionStateBase sessionState, Card card, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentOutOfRangeException("newQuantity", "Quantity must be greater than or equal to 0.");

            newQuantity = GetCorrectQuantity(newQuantity);

            ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(sessionState);
            ShoppingBasketItem item = shoppingBasket.Items.SingleOrDefault(i => i.Card == card);

            if (item == null)
                return;

            if (newQuantity == 0)
                shoppingBasket.Items.Remove(item);
            else
                item.Quantity = newQuantity;
        }

        /// <summary>
        /// Trade customers can only buy cards in multiples of 6. This methods rounds the requested
        /// quantity up to the nearest 6.
        /// </summary>
        /// <param name="newQuantity"></param>
        /// <returns></returns>
        private int GetCorrectQuantity(int newQuantity)
        {
            if (!_webContext.User.Identity.IsAuthenticated)
                return newQuantity;

            int mod = newQuantity % 6;
            if (mod == 0)
                return newQuantity;

            return newQuantity + (6 - mod);
        }

        public decimal CalculateDeliveryPrice(ShoppingBasket shoppingBasket)
        {
            Shop shop = ((StartPage)Find.StartPage).ShopPage;

            // If user is logged in as a trade customer, different rules apply.
            if (_webContext.User.Identity.IsAuthenticated)
            {
                if (shoppingBasket.TotalItemPrice >= shop.TradeFreeDeliveryThreshold)
                    return 0;

                return shop.TradeDeliveryPrice;
            }

            // Otherwise user is a non-trade customer.
            NonTradeDeliveryPrice deliveryPrice = shop.NonTradeDeliveryPrices.SingleOrDefault(p => p.PostalRegion == shoppingBasket.PostalRegion);
            if (deliveryPrice != null)
                return deliveryPrice.Price;
            return 0;
        }
    }

    public interface IShoppingBasket
    {
        IEnumerable<ShoppingBasketItem> Items { get; }
        decimal TotalItemPrice { get; }
        decimal DeliveryPrice { get; }
        decimal TotalPrice { get; }

        string FullName { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string TownCity { get; set; }
        string Postcode { get; set; }
        string Country { get; set; }
        string PostalRegion { get; set; }
        string Email { get; set; }
        string Telephone { get; set; }

        void Reset();
    }

    public class ShoppingBasket : IShoppingBasket
    {
        public ShoppingBasket()
        {
            Items = new List<ShoppingBasketItem>();
        }

        public List<ShoppingBasketItem> Items { get; set; }

        IEnumerable<ShoppingBasketItem> IShoppingBasket.Items
        {
            get { return Items; }
        }

        public decimal TotalItemPrice
        {
            get { return Items.Sum(i => i.Card.GetCurrentPrice() * i.Quantity); }
        }

        public decimal DeliveryPrice
        {
            get { return Context.Current.Resolve<ShoppingBasketService>().CalculateDeliveryPrice(this); }
        }

        public decimal TotalPrice
        {
            get { return TotalItemPrice + DeliveryPrice; }
        }

        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string TownCity { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string PostalRegion { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public void Reset()
        {
            Items.Clear();
            FullName = string.Empty;
            AddressLine1 = string.Empty;
            AddressLine2 = string.Empty;
            TownCity = string.Empty;
            Postcode = string.Empty;
            Country = string.Empty;
            PostalRegion = string.Empty;
            Email = string.Empty;
            Telephone = string.Empty;
        }
    }

    public class ShoppingBasketItem
    {
        public ShoppingBasketItem(Card card, int quantity)
        {
            CardID = card.ID;
            Quantity = quantity;
        }

        public int CardID { get; set; }
        public Card Card
        {
            get { return Context.Persister.Get<Card>(CardID); }
        }

        public int Quantity { get; set; }
    }
}