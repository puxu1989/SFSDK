using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSDK.entity
{
   public class OrderReqEntity
    {
        public List<AddedServiceDto> addedServices;
        public CargoInfoDto cargoInfo;
        public DeliverConsigneeInfoDto consigneeInfo;
        public string custId;
        public DeliverConsigneeInfoDto deliverInfo;
        public short expressType;
        public short isDoCall;
        public short isGenBillNo;
        public short isGenEletricPic;
        public short needReturnTrackingNo;
        public string orderId;
        public string payArea;
        public short payMethod;
        public string remark;
        public string sendStartTime;
    }
}
