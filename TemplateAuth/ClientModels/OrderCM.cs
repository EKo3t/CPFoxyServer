using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateAuth.Models;

namespace TemplateAuth.ClientModels
{
    public class OrderCM
    {
        public OrderCM()
        { }

        public OrderCM(Order order)
        {
            if (order != null)
            {
                this.OrderTime = order.orderTime;
                this.StartAddress = order.StartAddress;
                this.EndAddress = order.EndAddress;
            }
        }

        public Guid Id { get; set; }

        public DateTime OrderTime { get; set; }

        public string StartAddress { get; set; }

        public string EndAddress { get; set; }

        public string Email { get; set; }

        public Status Status { get; set; }
    }
}