using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SFSDK
{
    [Serializable]
    [XmlRoot(ElementName = "Response")]
    public class SFExpressResponse
    {
        public String Head { get; set; }
        public ERROR ERROR { get; set; }
        public Body Body { get; set; }
    }
    [Serializable]
    public class ERROR
    {
        [XmlAttribute(AttributeName = "code")]
        public String code { get; set; }
        [XmlText]
        public String text { get; set; }
    }
    [Serializable]
    public class Body
    {
        [XmlElement(ElementName = "OrderResponse")]
        public OrderResponse OrderResponse { get; set; }

        [XmlElement(ElementName = "RouteResponse")]
        public RouteResponse RouteResponse { get; set; }


    }
    [XmlRoot(ElementName = "OrderResponse")]
    public class OrderResponse
    {
        //订单号
        [XmlAttribute(AttributeName = "orderid")]
        public string OrderId { get; set; }
        //运单号
        [XmlAttribute(AttributeName = "mailno")]
        public string MailNo { get; set; }
        //原寄地区域代码(可用于顺丰电子运单标签打印)
        [XmlAttribute(AttributeName = "origincode")]
        public string OriginCode { get; set; }
        //目的地区域代码(可用于顺丰电子运单标签打印)
        [XmlAttribute(AttributeName = "destcode")]
        public string DestCode { get; set; }
        //筛单结果：1：人工确认 2：可收派 3：不可以收派
        [XmlAttribute(AttributeName = "filter_result")]
        public string FilterResult { get; set; }

    }
    [XmlRoot(ElementName = "RouteResponse")]
    public class RouteResponse
    {
        //运单号
        [XmlAttribute(AttributeName = "mailno")]
        public String MailNo { get;set;}
        //路由
        [XmlElement(ElementName = "Route")]
        public List<Route> Route { get; set; }
    }
    [XmlRoot(ElementName = "Route")]
    public class Route
    {
        //路由节点发生的时间
        [XmlAttribute(AttributeName = "accept_time")]
        public String AcceptTime { get; set; }
        //[XmlAttribute(AttributeName = "accept_address")]
        //public String AcceptAddress { get; set; }
        //路由节点具体描述
        [XmlAttribute(AttributeName = "remark")]
        public String Remark { get; set; }
        [XmlAttribute(AttributeName = "opcode")]
        public String opcode { get; set; }
    }
}
