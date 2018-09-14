using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFSDK
{
    public class SFDataEntity
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 顺丰订单ID
        /// </summary>
        public string SFOrderId { get; set; }
        /// <summary>
        /// 顺丰运单号
        /// </summary>
        public string SFMailNo { get; set; }
        /// <summary>
        /// 顺丰月结号
        /// </summary>
        public string SFYueJieCode { get; set; }
        /// <summary>
        /// 目的地 （大笔头)
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// 目的地代码 必要 审核
        /// </summary>
        public string DestCode { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string ResvRealName { get; set; }
        /// <summary>
        /// 收件人电话
        /// </summary>
        public string ResvTel { get; set; }
        /// <summary>
        /// 收件人详细地址
        /// </summary>
        public string ResvAddress { get; set; }
        /// <summary>
        /// 收件人省
        /// </summary>
        public string ResvProvince { get; set; }
        /// <summary>
        /// 收件人城市
        /// </summary>
        public string ResvCity { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string SendRealName { get; set; }    
        /// <summary>
        /// 发件人电话
        /// </summary>
        public string SendTel { get; set; }
        /// <summary>
        /// 发送人地址
        /// </summary>
        public string SendAddress { get; set; }

        /// <summary>
        /// 发件人省
        /// </summary>
        public string SendProvince { get; set; }
        /// <summary>
        /// 发件人城市
        /// </summary>
        public string SendCity { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 数量(包裹数量)
        /// </summary>
        public int GoodsNum { get; set; }
        /// <summary>
        /// 重量量
        /// </summary>
        public string GoodsWeight { get; set; }
        /// <summary>
        /// 专柜名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 其他备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 打印日期
        /// </summary>
        public string PrintDate { get; set; }
        /// <summary>
        /// 寄付方式
        /// </summary>
        public int PayMethod { get; set; } = 1;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string GoodsDescription { get; set; }
    }
}