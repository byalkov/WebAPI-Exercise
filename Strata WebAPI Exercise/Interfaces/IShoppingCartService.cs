using Strata_WebAPI_Exercise.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strata_WebAPI_Exercise.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCart GetShoppingCart(int userId);
        ShoppingCart UpdateProduct(int shoppingCartId, int productId, int quantity);
        bool CanUserBuyShoppingCart(int shoppingCartId, int userId);
        ShoppingCart BuyShoppingCart(int shoppingCartId, int userId);
    }
}
