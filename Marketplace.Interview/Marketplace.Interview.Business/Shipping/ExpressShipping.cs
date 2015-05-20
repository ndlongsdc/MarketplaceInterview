using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class ExpressShipping : PerRegionShipping
    {
        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Ex-Shipping to {0}", lineItem.DeliveryRegion);
        }

    }
}