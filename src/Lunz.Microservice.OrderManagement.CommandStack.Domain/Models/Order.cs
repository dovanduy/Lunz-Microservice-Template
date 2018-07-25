using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lunz.Domain.Kernel.Models;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models
{
    public class Order : AggregateRootMappedWithExpressions<Guid>
    {
        #region Fields

        private readonly List<OrderItem> _items;

        #endregion

        #region Properties

        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public Guid? HearFromId { get; set; }
        public string HearFromName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get
            {
                return Amount * Price;
            }
            private set { }
        }
        public IReadOnlyCollection<OrderItem> Items
        {
            get
            {
                return new ReadOnlyCollection<OrderItem>(_items);
            }
            set
            {
                _items.Clear();
                _items.AddRange(value);
            }
        }

        #endregion

        public Order()
        {
            _items = new List<OrderItem>();
        }

        protected internal Order(Guid id)
            : base(id)
        {
            _items = new List<OrderItem>();
        }

        public override void InitializeEventHandlers()
        {
            Map<OrderEntered>().ToHandler(Apply).MatchExact();
            Map<OrderItemAdded>().ToHandler(Apply).MatchExact();
        }

        #region Static

        public static Order EnterOrder(Guid aggregatedId, string subject, DateTime date, Guid? hearFromId,
            string hearFromName, int amount, decimal price)
        {
            var order = new Order(aggregatedId)
            {
                Subject = subject,
                Date = date,
                HearFromId = hearFromId,
                HearFromName = hearFromName,
                Amount = amount,
                Price = price
            };

            // TODO: 填充 userDetails 参数
            order.RaiseEvent(new OrderEntered(order.Id, order.Subject, order.Date, order.HearFromId,
                order.Amount, order.Price, order.Total, null));

            return order;
        }

        #endregion

        #region Public Methods

        public void AddItem(Guid itemId, string productName, int amount, decimal price)
        {
            var item = new OrderItem(this, itemId)
            {
                ProductName = productName,
                Amount = amount,
                Price = price
            };

            RaiseEvent(new OrderItemAdded(this.Id, item.EntityId, item.ProductName,
                item.Amount, item.Price, item.Total));

            _items.Add(item);
        }

        #endregion

        #region Apply Functions

        private void Apply(OrderEntered @event)
        {
            // Do anything
        }

        private void Apply(OrderItemAdded @event)
        {
            // Do anything
        }

        #endregion
    }
}
