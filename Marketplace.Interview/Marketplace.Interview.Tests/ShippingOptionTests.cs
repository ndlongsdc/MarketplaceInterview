using System.Collections.Generic;
using NUnit.Framework;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class ShippingOptionTests
    {
        [Test]
        public void FlatRateShippingOptionTest()
        {
            var flatRateShippingOption = new FlatRateShipping { FlatRate = 1.5m };
            var shippingAmount = flatRateShippingOption.GetAmount(new LineItem(), new Basket());

            Assert.That(shippingAmount, Is.EqualTo(1.5m), "Flat rate shipping not correct.");
        }

        [Test]
        public void PerRegionShippingOptionTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
                                              {
                                                  PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
                                              };

            var shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.Europe }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void BasketShippingTotalTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping { FlatRate = 1.1m };

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(3.35m));
        }
        
        [Test]
        public void ExpressShippingOptionTest()
        {
            var expressShippingOption = new ExpressShipping()
            {
                PerRegionCosts = new[]{
                    new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = 1m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 2m
                                                                               },
                                                                               new RegionShippingCost{
                                                                                   DestinationRegion = RegionShippingCost.Regions.RestOfTheWorld,
                                                                                    Amount = 3m
                                                                               }        

                }
            };

            var shippingAmount = expressShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK, Id = 1, SupplierId =1, ProductId = "1" }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1m));
            shippingAmount = expressShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.Europe, Id = 1, SupplierId = 1, ProductId = "1" }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(2m));
            shippingAmount = expressShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.RestOfTheWorld, Id = 1, SupplierId = 1, ProductId = "1" }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(3m));

        }
        [Test]
        public void BasketShippingTotalWithExpressTest()
        {
            #region Create Shipping Option
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                        {
                                                                            new RegionShippingCost()
                                                                                {
                                                                                    DestinationRegion =
                                                                                        RegionShippingCost.Regions.UK,
                                                                                    Amount = .75m
                                                                                },
                                                                            new RegionShippingCost()
                                                                                {
                                                                                    DestinationRegion =
                                                                                        RegionShippingCost.Regions.Europe,
                                                                                    Amount = 1.5m
                                                                                },
                                                                                new RegionShippingCost{
                                                                                    DestinationRegion = RegionShippingCost.Regions.RestOfTheWorld,
                                                                                    Amount = 2m
                                                                                }                                                                               
                                                                        },
            };
            var flatRateShippingOption = new FlatRateShipping()
            {
                FlatRate = 3
            };
            var expressShippingOption = new ExpressShipping()
            {
                PerRegionCosts = new[]{
                    new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = 1m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 2m
                                                                               },
                                                                               new RegionShippingCost{
                                                                                   DestinationRegion = RegionShippingCost.Regions.RestOfTheWorld,
                                                                                    Amount = 3m
                                                                               }        

                }
            };
            #endregion
            #region Create Basket
            var basket = new Basket()
            {
                LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption,
                                                              Id = 1,
                                                              SupplierId = 1
                                                         },                 //0.75                                   
                                                    new LineItem()
                                                        {
                                                            DeliveryRegion = RegionShippingCost.Regions.UK,
                                                            Shipping = flatRateShippingOption,
                                                            Id = 2,
                                                            SupplierId = 1
                                                        }, //3
                                                     new LineItem() {Shipping = expressShippingOption, DeliveryRegion = RegionShippingCost.Regions.UK, SupplierId = 1, Id = 3}, //1
                                                     new LineItem() {Shipping = expressShippingOption, DeliveryRegion = RegionShippingCost.Regions.Europe, SupplierId = 1, Id = 4}, //2
                                                     new LineItem() {Shipping = expressShippingOption, DeliveryRegion = RegionShippingCost.Regions.UK, SupplierId = 1, Id = 5} // 1
                                                 }
            };
            #endregion
            var calculator = new ShippingCalculator();

            decimal discount = calculator.GetDiscount(basket);
            Assert.That(discount, Is.EqualTo(0.5m));
            decimal basketShipping = calculator.CalculateShipping(basket);           
            Assert.That(basketShipping, Is.EqualTo(7.25m));
           
        }
    }
}