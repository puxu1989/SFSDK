using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSDK.entity
{
   public class OrderRespEntity
    {
        public string orderId;

        public OrderRespEntity()
        {
        }

        public OrderRespEntity(string orderId)
        {
            this.orderId = orderId;
        }
    }
}
