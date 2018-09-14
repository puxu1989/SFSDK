using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSDK.entity
{
    public class MessageResp<T>
    {
        public T body;
        public HeadMessageResp head;
    }
    public class HeadMessageResp: HeadMessageReq
    {
        public string code;
        public string message;

        public HeadMessageResp()
        {
        }

        public HeadMessageResp(string code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
