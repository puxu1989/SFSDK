using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSDK.entity
{
    public class MessageReq<T>
    {
        public T body { get; set; }
        public HeadMessageReq head = new HeadMessageReq();

        
    }
}
