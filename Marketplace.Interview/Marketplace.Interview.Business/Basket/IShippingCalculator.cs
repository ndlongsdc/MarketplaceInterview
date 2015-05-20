using Marketplace.Interview.Business.Shipping;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Interview.Business.Basket
{
    public interface IShippingCalculator
    {
        decimal CalculateShipping(Basket basket);
        decimal GetDiscount(Basket basket);
    }

    public class ShippingCalculator : IShippingCalculator
    {
        public decimal CalculateShipping(Basket basket)
        {
            foreach (var lineItem in basket.LineItems)
            {
                lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }
            return basket.LineItems.Sum(li => li.ShippingAmount) - GetDiscount(basket);
        }
        public decimal GetDiscount(Basket list)
        {
            List<LineItem> listline = list.LineItems.Where(m => m.Shipping.GetType() == typeof(ExpressShipping)).ToList();
            if (listline.Count > 1)
            {
                if (listline.Any(item => listline.Count(li => li.DeliveryRegion == item.DeliveryRegion && li.SupplierId == item.SupplierId) > 1))
                    return 0.5m;
            }
            return 0;
        }
    }
}