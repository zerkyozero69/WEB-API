using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    /// <summary>
    /// ข้อมูลส่ง-รับเมล็ดพันธุ์ให้หน่วยงาน
    /// ==================
    /// SendStatus:
    ///     รับเมล็ด
    ///         NotSend = 0 (อยู่ระหว่างดำเนินการ)
    ///         Send = 1(จัดส่งเมล็ดพันธุ์แล้ว)
    ///         Accept = 2 (ตรวจสอบแล้ว)
    ///         Approve = 3(อนุมัติรับเมล็ดพันธุ์)
    ///         Cancel = 4 (ยกเลิก)
    ///         Eject = 8(ไม่อนุมัติ)
    ///     ==================
    ///     ส่งเมล็ด
    ///         SendAccept = 5(ตรวจสอบแล้ว)
    ///         SendApprove = 6 (อนุมัติส่งเมล็ดพันธุ์)
    ///         SendCancel = 7(ยกเลิกส่งเมล็ดพันธุ์)
    ///         SendEject = 9 (ไม่อนุมัติส่งเมล็ดพันธุ์)
    /// </summary>
    public class SendOrderSeedController : ApiController
    {
        //database connection.
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// แสดงข้อมูลส่ง-รับเมล็ดพันธุ์ให้หน่วยงาน
        /// </summary>
        /// <param name="Org_Oid">OID ของหน่วยงาน</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/List")]
        public HttpResponseMessage GetSendOrderSeed()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                string type = HttpContext.Current.Request.Form["type"].ToString(); //รับ=1/ส่ง=2

                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));

                    List<SendOrderSeedType> SendItems = new List<SendOrderSeedType>();
                    List<SendOrderSeedType> ReceiveItems = new List<SendOrderSeedType>();
                    SendOrderSeedModel lists = new SendOrderSeedModel();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    if (type == "2")
                    {  //ส่ง

                        IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus=2 and SendOrgOid.Oid='" + org_oid + "'", null));
                        if (collection.Count > 0)
                        {
                            foreach (SendOrderSeed row in collection)
                            {
                                SendOrderSeedType item = new SendOrderSeedType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.SendStatus = row.SendStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.RefNo = row.SendNo + "|" + row.SendOrgOid.Oid.ToString() + "|2";

                                item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                                SendItems.Add(item);
                            }
                        }
                        //lists.Sender = null; //SendItems;
                        return Request.CreateResponse(HttpStatusCode.OK, SendItems);
                    }
                    else if (type == "1")
                    {  //รับ

                        IList<SendOrderSeed> collection2 = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus=5 and ReceiveOrgOid.Oid='" + org_oid + "'", null));
                        if (collection2.Count > 0)
                        {
                            foreach (SendOrderSeed row in collection2)
                            {
                                SendOrderSeedType item = new SendOrderSeedType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.SendStatus = row.SendStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.RefNo = row.SendNo + "|" + row.ReceiveOrgOid.Oid.ToString() + "|1";
                                item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                                ReceiveItems.Add(item);
                            }
                            //lists.Receive = ReceiveItems;
                            return Request.CreateResponse(HttpStatusCode.OK, ReceiveItems);
                        }
                    }

                    //invalid
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid และ type (1=รับ/2=ส่ง) ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                }
                else {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid และ type (1=รับ/2=ส่ง) ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "ไม่พบข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// แสดงรายละเอียดข้อมูลส่ง-รับเมล็ดพันธุ์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/Detail")]
        public HttpResponseMessage GetSendOrderSeedDetail()
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString();

                if (RefNo != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SendOrderSeed));
                    SendOrderSeedType item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendNo='"+ _refno + "'", null));

                    foreach (SendOrderSeed row in collection)
                    {
                        item = new SendOrderSeedType();
                        item.SendNo = row.SendNo;
                        item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                        item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                        item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                        item.Remark = row.Remark;
                        item.SendStatus = row.SendStatus.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.CancelMsg = row.CancelMsg;
                        item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                        item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                        item.RefNo = RefNo;
                        item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString();

                        List<SendOrderSeedDetailType> details = new List<SendOrderSeedDetailType>();
                        SendOrderSeedDetailType _dt = null; 

                        foreach (SendOrderSeedDetail rw in row.SendOrderSeedDetails)
                        {
                            _dt = new SendOrderSeedDetailType();
                            _dt.LotNumber = rw.LotNumber.LotNumber;
                            _dt.Amount = rw.Amount;
                            _dt.Weight = rw.Weight;
                            _dt.WeightUnitOid = rw.WeightUnitOid.Oid.ToString();
                            _dt.WeightUnitName = rw.WeightUnitOid.UnitName;
                            _dt.Used = rw.Used;
                            _dt.SeedTypeOid = rw.SeedTypeOid.Oid.ToString();
                            _dt.SeedTypeName = rw.SeedTypeOid.SeedTypeName;
                            _dt.BudgetSourceOid = rw.BudgetSourceOid.Oid.ToString();
                            _dt.BudgetSourceName = rw.BudgetSourceOid.BudgetName;
                            _dt.AnimalSeeName = rw.AnimalSeeName;
                            _dt.AnimalSeedOid = rw.AnimalSeedOid.Oid.ToString();
                            _dt.AnimalSeedName = rw.AnimalSeedOid.SeedName;
                            _dt.AnimalSeedLevelOid = rw.AnimalSeedLevelOid.Oid.ToString();
                            _dt.AnimalSeedLevel = rw.AnimalSeedLevel;
                            _dt.AnimalSeedLevelName = rw.AnimalSeedLevelOid.SeedLevelName;
                            _dt.AnimalSeedCode = rw.AnimalSeedCode;
                            details.Add(_dt);
                        }

                        item.Details= details;

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
                else {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// ปรับปรุงข้อมูลส่ง-รับเมล็ดพันธุ์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <param name="Status">สถานะการตรวจสอบ (1=อนุมัติ/2=ไม่อนุมัติ)</param>
        /// <param name="CancelMsg">หมายเหตุกรณีไม่อนุมัติ</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/Update")]
        public HttpResponseMessage UpdateSendOrderSeed()
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["CancelMsg"].ToString(); //หมายเหตุ

                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SendOrderSeed));
                    List<SendOrderSeed> list = new List<SendOrderSeed>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SendOrderSeed objSupplierProduct = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));
                    if (objSupplierProduct != null)
                    {
                        if (_type == "1") //รับ
                        { 
                            if (Status == "1")
                            { //Approve
                                objSupplierProduct.SendStatus = EnumSendOrderSeedStatus.Approve; //3
                                ObjectSpace.CommitChanges();
                            }
                            else if (Status == "2")
                            { //Reject
                                objSupplierProduct.SendStatus = EnumSendOrderSeedStatus.Eject; //8
                                objSupplierProduct.CancelMsg = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                        }
                        else if (_type == "2") //ส่ง
                        { 
                            if (Status == "1")
                            { //Approve
                                objSupplierProduct.SendStatus = EnumSendOrderSeedStatus.SendApprove; //6
                                ObjectSpace.CommitChanges();
                            }
                            else if (Status == "2")
                            { //Reject
                                objSupplierProduct.SendStatus = EnumSendOrderSeedStatus.SendEject; //9
                                objSupplierProduct.CancelMsg = CancelMsg;
                                ObjectSpace.CommitChanges();
                            }
                        }

                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);

                    }
                    else {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "-1";
                        err.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }                  
                }
                else {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

    }
}
