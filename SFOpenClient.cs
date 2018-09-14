using SFSDK.entity;
using SFSDK.lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSDK
{
    public class SFOpenClient
    {
        public static string SFAppId = ConfigurationManager.AppSettings["SFAppId"].Trim();
        public static string SFAppKey = ConfigurationManager.AppSettings["SFAppKey"].Trim();
        public static string SFYuJieCode = ConfigurationManager.AppSettings["SFYuJieCode"].Trim();
        public static string domain = "https://open-prod.sf-express.com";
        //沙盒环境{domain} ：open-sbox.sf-express.com
        //生产环境{domain} ：open-prod.sf-express.com
        /// <summary>
        /// 获取Token 有效期为1小时
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            string url = string.Format(domain+"/public/v1.0/security/access_token/sf_appid/{0}/sf_appkey/{1}", SFAppId, SFAppKey);
            MessageReq<TokenEntity> accessTokenReq = new MessageReq<TokenEntity>();
            accessTokenReq.head.transType = 301;
            accessTokenReq.head.transMessageId = DateTime.Now.ToLongTimeString();
            MessageResp<TokenEntity> res = HttpWebHelper.doPost<MessageReq<TokenEntity>, MessageResp<TokenEntity>>(url, accessTokenReq);
            if (res.head.transType == 4301)
            {
                return res.body.accessToken;
            }
            else
            {
                throw new Exception(res.head.message);
            }
        }
        /// <summary>
        /// 查询AccessToken 可以查询到数据 但是可能过期
        /// </summary>
        /// <returns></returns>
        public static string QueryAccessToken()
        {
            string url = string.Format(domain + "/public/v1.0/security/access_token/query/sf_appid/{0}/sf_appkey/{1}", SFAppId, SFAppKey);
            MessageReq<TokenEntity> accessTokenReq = new MessageReq<TokenEntity>();
            accessTokenReq.head.transType = 300;
            accessTokenReq.head.transMessageId = DateTime.Now.ToLongTimeString();
            MessageResp<TokenEntity> res = HttpWebHelper.doPost<MessageReq<TokenEntity>, MessageResp<TokenEntity>>(url, accessTokenReq);
           
            if (res.head.code == "EX_CODE_OPENAPI_0105")//访问令牌过期
            {
                return GetAccessToken();
            }
           else if (res.head.transType == 4300)
            {
                return res.body.accessToken;
            }
            else
            {
                throw new Exception(res.head.message);
            }
        }

        public static string SubmitOrder(string province,string city,string address,string contact,string tel,string goodsName, short payMethod)
        {
            string url = string.Format(domain+"/rest/v1.0/order/access_token/{0}/sf_appid/{1}/sf_appkey/{2}", QueryAccessToken(), SFAppId, SFAppKey);
            MessageReq<OrderReqEntity> req = new MessageReq<OrderReqEntity>();
            req.head.transType = 200;
            req.head.transMessageId= DateTime.Now.ToLongTimeString();
            req.body = new OrderReqEntity();
            req.body.orderId = "SF"+DateTime.Now.ToString("yyyyMMddHHmmssfff");
            req.body.expressType = 1;//标准快递
            req.body.isDoCall = 1; //通知收派员上门取件
            req.body.payMethod = payMethod;//付款方式 1月结 2收方付 3第三方付
            if (req.body.payMethod == 1)
            {
                req.body.custId = SFYuJieCode;//顺丰月结卡号 10 位数字
            }          
            req.body.consigneeInfo = new DeliverConsigneeInfoDto();
            req.body.consigneeInfo.address = address;
            req.body.consigneeInfo.city = city;
            req.body.consigneeInfo.company = "顺丰";
            req.body.consigneeInfo.contact = contact;
            req.body.consigneeInfo.tel = tel;
            req.body.consigneeInfo.province = province;
            req.body.cargoInfo = new CargoInfoDto();
            req.body.cargoInfo.cargo = goodsName;
            MessageResp<OrderRespEntity> res = HttpWebHelper.doPost<MessageReq<OrderReqEntity>, MessageResp<OrderRespEntity>>(url, req);
            if (res.head.code == "EX_CODE_OPENAPI_0105")
            {
                GetAccessToken();
                SubmitOrder( province,  city,  address,  contact,  tel,  goodsName,payMethod);
            }
            if (res.head.transType == 4200)
            {
                return res.body.orderId;
            }            
            else
            {
                throw new Exception(res.head.message);
            }
        }
        /// <summary>
        /// 查询订单返回
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string QueryOrder(string orderId)
        {
            string url = string.Format(domain+"/rest/v1.0/order/query/access_token/{0}/sf_appid/{1}/sf_appkey/{2}", QueryAccessToken(), SFAppId, SFAppKey);
            MessageReq<OrderQueryReqDto> req = new MessageReq<OrderQueryReqDto>();
            req.head.transType = 203;
            req.head.transMessageId = DateTime.Now.ToLongTimeString();
            req.body = new OrderQueryReqDto();
            req.body.orderId =orderId;
            MessageResp<OrderQueryRespDto> res = HttpWebHelper.doPost<MessageReq<OrderQueryReqDto>, MessageResp<OrderQueryRespDto>>(url, req);
            //return HttpWebHelper.ObjectToJson(res);
            if (res.head.code == "EX_CODE_OPENAPI_0105")
            {
                GetAccessToken();
                QueryOrder(orderId);
            }
            if (res.head.transType == 4203)
            {
                return res.body.mailNo;
            }
            else
            {
                throw new Exception(res.head.message);
            }
        }
        public static string QueryRoute(string trackingNumber)
        {
            string url = string.Format(domain+"/rest/v1.0/route/query/access_token/{0}/sf_appid/{1}/sf_appkey/{2}", QueryAccessToken(), SFAppId, SFAppKey);
            MessageReq<RouteReqDto> req = new MessageReq<RouteReqDto>();
            req.head.transType = 501;
            req.head.transMessageId = DateTime.Now.ToLongTimeString();
            req.body = new RouteReqDto();
            req.body.trackingNumber = trackingNumber;
            req.body.trackingType = 2;//2 订单号查询 1 运单号查询
            req.body.methodType = 1;
            MessageResp<List<RouteRespDto>> res = HttpWebHelper.doPost<MessageReq<RouteReqDto>, MessageResp<List<RouteRespDto>>>(url, req);
            if (res.head.code == "EX_CODE_OPENAPI_0105")
            {
                GetAccessToken();
                QueryRoute(trackingNumber);
            }
            return HttpWebHelper.ObjectToJson(res);

        }
    }
}
