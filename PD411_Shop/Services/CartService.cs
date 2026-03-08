using PD411_Shop.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace PD411_Shop.Services
{
    public static class CartService
    {
        public static void AddToCart(ISession session, int productId)
        {
            if (!IsInCart(session, productId))
            {
                var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
                var newItem = new CartItemVM { ProductId = productId };
                items.Add(newItem);
                session.Set(items);
            }

        }
        public static void RemoveFromCart(ISession session, int productId)
        {
            if (IsInCart(session, productId))
            {
                var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
                items = items.Where(i => i.ProductId != productId).ToList();
                session.Set(items);
            }
        }
        public static bool IsInCart(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            return items.Any(x => x.ProductId == productId);
        }

        public static int ItemsCount(ISession session)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            return items.Count();
        }

        public static void Increment(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            var item = items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Count += 1;
                session.Set(item);
            }
        }
        public static void Decrement(ISession session, int productId)
        {
            var items = session.Get<List<CartItemVM>>() ?? new List<CartItemVM>();
            var item = items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Count -= 1;
                session.Set(item);
            }
        }

    }
}
