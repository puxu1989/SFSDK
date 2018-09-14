using SFSDK.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SFSDK
{
    public class FQClient
    {
        public static string requestUrl = "http://bsp-oisp.sf-express.com/bsp-oisp/sfexpressService"; //测试/生产环境地址 

        public static string Checkword = ConfigurationManager.AppSettings["SFCheckword"].Trim();
        public static string YuJieCode = ConfigurationManager.AppSettings["SFYuJieCode"].Trim();
        public static string ClientCode = ConfigurationManager.AppSettings["ClientCode"].Trim();
        /// <summary>
        /// 下单 返回订单号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static OrderResponse OrderSubmit(SFDataEntity entity)
        {
            string xml = GetOrderServiceRequestXml(entity);
            string verifyCode = MD5ToBase64String(xml + Checkword);
            string result = DoPost(requestUrl, xml, verifyCode);//这就得到了返回结果
            SFExpressResponse response = XMLSerializer.DeserializeXML<SFExpressResponse>(result);
            if (response.Head == "OK")
            {
                return response.Body.OrderResponse;
            }
            else
            {
                throw new Exception(response.ERROR.text);
            }
        }
        /// <summary>
        /// 订单确认或者取消 isConfirm=true 确认 需要传入orderNo 和mailNo 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="mailNo"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>
        public static bool OrderConfirm(string orderId,string mailNo, bool isConfirm = true)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Request service='OrderConfirmService' lang='zh-CN'>");
            strBuilder.Append("<Head>" + ClientCode + "</Head>");
            strBuilder.Append("<Body>");
            strBuilder.Append("<OrderConfirm").Append(" ");
            strBuilder.Append("orderid='" + orderId + "" + "'").Append(" ");
          
            if (isConfirm)//默认是确认 否则取消订单
            {
                strBuilder.Append("mailno='" + mailNo + "" + "'").Append(" ");
                strBuilder.Append("dealtype='1'").Append(" > ");
            }             
            else
                strBuilder.Append("dealtype='2'").Append(" > ");
            strBuilder.Append("</OrderConfirm>");

            strBuilder.Append("</Body>");
            strBuilder.Append("</Request>");
            string xml = strBuilder.ToString();
            string verifyCode = MD5ToBase64String(xml + Checkword);
            string result = DoPost(requestUrl, xml, verifyCode);
            SFExpressResponse response = XMLSerializer.DeserializeXML<SFExpressResponse>(result);
            if (response.Head == "OK")
            {
                return true;
            }
            else
            {
                throw new Exception(response.ERROR.text);
            }
        }
        public static OrderResponse OrderSearch(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
                throw new Exception("顺丰订单号不能为空");
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Request service='OrderSearchService' lang='zh-CN'>");
            strBuilder.Append("<Head>" + ClientCode + "</Head>");
            strBuilder.Append("<Body>");
            strBuilder.Append("<OrderSearch").Append(" ");
            strBuilder.Append("orderid='" + orderNo + "" + "'").Append(" > ");
            strBuilder.Append("</OrderSearch>");
            strBuilder.Append("</Body>");
            strBuilder.Append("</Request>");
            string xml = strBuilder.ToString();
            string verifyCode = MD5ToBase64String(xml + Checkword);
            string result = DoPost(requestUrl, xml, verifyCode);
            SFExpressResponse response = XMLSerializer.DeserializeXML<SFExpressResponse>(result);
            if (response.Head == "OK")
            {
                return response.Body.OrderResponse;
            }
            else
            {
                throw new Exception(response.ERROR.text);
            }
        }
        /// <summary>
        /// 路由查询接口（RouteService）：支持两类查询方式：①根据顺丰运单号查询 ②根据客户订单号查询   
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public static RouteResponse RouteSearch(string orderIdOrMailNo, bool isOrderId = true)
        {
            if(string.IsNullOrEmpty(orderIdOrMailNo))
                throw new Exception("查询单号不能为空");
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Request service='RouteService' lang='zh-CN'>");
            strBuilder.Append("<Head>" + ClientCode + "</Head>");
            strBuilder.Append("<Body>");
            strBuilder.Append("<RouteRequest").Append(" ");
            if (isOrderId)
                strBuilder.Append("tracking_type='2'").Append(" ");//1为运单号 2为订单号              
            else
                strBuilder.Append("tracking_type='1'").Append(" ");
            strBuilder.Append("method_type='1'").Append(" ");//标准路由
            strBuilder.Append("tracking_number='" + orderIdOrMailNo + "'").Append(" >");
            strBuilder.Append("</RouteRequest>");
            strBuilder.Append("</Body>");
            strBuilder.Append("</Request>");
            string xml = strBuilder.ToString();
            string verifyCode = MD5ToBase64String(xml + Checkword);
            string result = DoPost(requestUrl, xml, verifyCode);
            SFExpressResponse response = XMLSerializer.DeserializeXML<SFExpressResponse>(result);
            if(response==null)
                throw new Exception("序列化出错："+ result);
            if (response.Head == "OK")
            {
                if (response.Body == null || response.Body.RouteResponse == null)
                    throw new Exception("未查询到路由信息");
                return response.Body.RouteResponse;
            }
            else
            {
                throw new Exception(response.ERROR.text);
            }
        }
        /// <summary>
        /// 下单XML
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static String GetOrderServiceRequestXml(SFDataEntity entity)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<Request service='OrderService' lang='zh-CN'>");
            strBuilder.Append("<Head>" + ClientCode + "</Head>");
            strBuilder.Append("<Body>");
            strBuilder.Append("<Order").Append(" ");
            strBuilder.Append("orderid='" + entity.SFOrderId + "" + "'").Append(" ");
            //返回顺丰运单号
            strBuilder.Append("is_gen_bill_no='1'").Append(" ");
            strBuilder.Append("express_type='6'").Append(" ");//express_type=6即日快
            //寄件方信息
            strBuilder.Append("j_company='" + entity.ShopName + "'").Append(" ");
            strBuilder.Append("j_contact='" + entity.SendRealName + "'").Append(" ");
            strBuilder.Append("j_tel='" + entity.SendTel + "'").Append(" ");
            strBuilder.Append("j_address='" + entity.SendAddress + "'").Append(" ");
            //收件方信息
            strBuilder.Append("d_company='" + entity.ResvRealName + "'").Append(" ");
            strBuilder.Append("d_contact='" + entity.ResvRealName + "'").Append(" ");
            strBuilder.Append("d_tel='" + entity.ResvTel + "'").Append(" ");
            strBuilder.Append("d_address='" + entity.ResvAddress + "'").Append(" ");
            strBuilder.Append("pay_method='" + entity.PayMethod + "'").Append(" ");
            if (entity.PayMethod == 1)
            {           
                strBuilder.Append("custid='" + entity.SFYueJieCode + "'").Append(" ");
            }
            strBuilder.Append(" > ");
            //货物信息
            strBuilder.Append("<Cargo").Append(" ");
            strBuilder.Append("name='" + entity.GoodsName + "'").Append(" ");
            strBuilder.Append("count='" + entity.GoodsNum + "'").Append(" ");
            strBuilder.Append("unit='个'").Append(">");
            strBuilder.Append("</Cargo>");

            strBuilder.Append("</Order>");
            strBuilder.Append("</Body>");
            strBuilder.Append("</Request>");
            return strBuilder.ToString();
        }
        public static string MD5ToBase64String(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] byteMD5 = md5.ComputeHash(Encoding.UTF8.GetBytes(str));//MD5(注意UTF8编码) 
            string result = Convert.ToBase64String(byteMD5);//Base64 
            return result;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { // 总是接受
            return true;
        }
        private static string DoPost(string Url, string xml, string verifyCode)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            string postData = string.Format("xml={0}&verifyCode={1}", xml, verifyCode); //请求 
            WebRequest request = (HttpWebRequest)WebRequest.Create(Url); request.Method = "POST"; request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);
            byte[] postByte = Encoding.UTF8.GetBytes(postData);
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(postByte, 0, postByte.Length);
            reqStream.Close(); //读取
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
