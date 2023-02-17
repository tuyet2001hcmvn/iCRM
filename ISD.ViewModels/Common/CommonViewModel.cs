using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConstantClass = ISD.Constant.ConstantClass;

namespace ISD.ViewModels
{
    public class BaseEntity
    {
        public long ROWNUMBER { get; set; }
        public int TOTALROW { get; set; }
        public string Action { get; set; }
        public int index { get; set; }
        public bool isAdmin { get; set; }
    }

    public class JqGridTree
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ParentList { get; set; }
        public int? ParentID { get; set; }
        public int Level { get; set; }
        public bool isLeaf { get; set; }
    }

    public class tree_jeasy
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool? Checked { get; set; }
        public string iconCls { get; set; }
        public string UserName { get; set; }
        public List<tree_jeasy> children { get; set; }
    }

    public class ColExcelOptionModel
    {
        public ColExcelOptionModel(string _ColModel = "", string _ColName = "", ConstantClass.Excel_ObjectType _ObjectType = 0, ConstantClass.Excel_TextAlign _TextAlign = 0, bool _Bold = false, int _FontSize = 0, int _FontStyle = 0)
        {
            this.ColModel = _ColModel;
            this.ColName = _ColName;
            this.ObjectType = _ObjectType;
            this.TextAlign = _TextAlign;
            this.Bold = _Bold;
            this.FontSize = _FontSize;
            this.FontStyle = _FontStyle;
        }

        public string ColModel { get; set; }
        public string ColName { get; set; }

        public ConstantClass.Excel_ObjectType ObjectType { get; set; }
        public ConstantClass.Excel_TextAlign TextAlign { get; set; }
        public bool? Bold {get;set;}
        public int? FontSize { get; set; }
        public int? FontStyle { get; set; }
    }

    public class DocumentCommon : BaseEntity
    {
        //public int rownumber { get; set; }
        public Int64 DocumentID { get; set; }

        public string DocumentNoFull { get; set; }

        [Display(Name = "Ngày chứng từ")]
        public DateTime DocumentDate { get; set; }

        [Required]
        [Display(Name = "Loại CT")]
        public string DocumentClassCode { get; set; }

        [Required]
        [Display(Name = "Số CT")]
        public string DocumentTypeCode { get; set; }

        public string PolicyNo { get; set; }

        public Nullable<decimal> amountinvnd { get; set; }

        [Display(Name = "Số hợp đồng")]
        public string Description { get; set; }

        [Display(Name = "CN")]
        public string BranchCode { get; set; }

        public string Branch_ShortName { get; set; }

        public string CreateUser { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }

        public Nullable<int> crossCompanyDocumentID { get; set; }

        public Nullable<int> ApprovalID { get; set; }

        public string ApprovalName { get; set; }

        public string ApprovalByUser { get; set; }

        [Display(Name = "Năm")]
        public Nullable<int> FiscalYear { get; set; }

        [Display(Name = "Kỳ")]
        public Nullable<int> Period { get; set; }

        public Nullable<DateTime> LastUpdate { get; set; }

        public string LastUser { get; set; }

        // Using for Contract
        [Display(Name = "Lần thanh toán")]
        public Nullable<int> SeqNo { get; set; }
    }

    public class DocumentInfo : BaseEntity
    {
        public Int64 DocumentID { get; set; }
        [Required]
        [Display(Name = "Chi nhánh")]
        public string BranchCode { get; set; }
        public Nullable<long> Branch_ID { get; set; }
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Số CT")]
        public string DocumentNo { get; set;}
        [Required]
        [Display(Name = "Loại CT")]
        public string DocumentClassCode { get; set; }
        [Required]
        public string DocumentTypeCode { get; set; }
        [Required]
        [Display(Name = "Ngày CT")]
        public Nullable<DateTime> DocumentDate { get; set; }
        [Required]
        [Display(Name = "Ngày tỷ giá")]
        public Nullable<DateTime> ExchangeRateDate { get; set; }

        [Required]
        [Display(Name = "Đại Diện")]
        public string Representative { get; set; }

        [Display(Name = "Về khoản")]
        public string Description { get; set; }
        public Nullable<decimal> amountinvnd { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public string LastUser { get; set; }
        public string CreateUser { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        [Required]
        [Display(Name = "Năm KT")]
        public Nullable<int> FiscalYear { get; set; }
        [Required]
        [Display(Name = "Kỳ KT")]
        public Nullable<int> Period { get; set; }
        [Display(Name = "Hạn TT")]
        public Nullable<DateTime> SettlementDate { get; set; }
        public Nullable<int> CrossCompanyDocumentID { get; set; }

        public string KindCode { get; set; }
        public string TransactionKindCode { get; set; }
        public string ReInsuranceNo { get; set; }
        public string OpenDepartmentCode { get; set; }
        public string CustomerNo { get; set; }
        public string BrokerCode { get; set; }
    }

    public class DocumentDetailInfo : BaseEntity
    {
        public Int64 DocumentDetailID { get; set; }
        public Int64 DocumentID { get; set; }
        public bool isExistAccount = false;
        public string commonMsg { get; set; }
        public string controlName { get; set; }
        public string PolicyNoStatus1 { get; set; }
        public int PolicyNoStatus2 { get; set; }


        public int DebitAccountStatus1 { get; set; }
        public int DebitAccountStatus2 { get; set; }
        public string DebitBranchCode { get; set; }
        public string DebitDepartmentCode { get; set; }
        public Nullable<DateTime> DebitBillDate { get; set; }
        public string DebitBillCode { get; set; }

        [StringLength(7)]
        public string DebitBillNo { get; set; }
        public string DebitTaxCode { get; set; }
        public string DebitVatTypeCode { get; set; }
        public string DebitInsuranceObject { get; set; }
        public string DebitPolicyNo { get; set; }
        public string DebitClaimNo { get; set; }
        public string DebitReInsuranceNo { get; set; }
        public string DebitReInsuranceClaimNo { get; set; }

        [Required]
        [Display(Name = "Tài khoản")]
        public string DebitGLAccountID { get; set; }
        public string DebitInsuranceTypeCode { get; set; }
        public Nullable<int> DebitCustomerTypeID { get; set; }
        public string DebitCustomerNo { get; set; }
        public Nullable<decimal> DebitQuantity { get; set; }
        public string DebitUnitCode { get; set; }
        public string DebitThirdPartyLiabilityNo { get; set; }


        public int CreditAccountStatus1 { get; set; }
        public int CreditAccountStatus2 { get; set; }
        public string CreditBranchCode { get; set; }
        public string CreditDepartmentCode { get; set; }
        public Nullable<DateTime> CreditBillDate { get; set; }
        public string CreditBillCode { get; set; }

        [StringLength(7)]
        public string CreditBillNo { get; set; }
        public string CreditTaxCode { get; set; }
        public string CreditVatTypeCode { get; set; }
        public string CreditInsuranceObject { get; set; }
        public string CreditPolicyNo { get; set; }
        public string CreditClaimNo { get; set; }
        public string CreditReInsuranceNo { get; set; }
        public string CreditReInsuranceClaimNo { get; set; }
        [Required]
        [Display(Name = "Tài khoản")]
        public string CreditGlAccountID { get; set; }
        public string CreditInsuranceTypeCode { get; set; }
        public Nullable<int> CreditCustomerTypeID { get; set; }
        public string CreditCustomerNo { get; set; }
        public Nullable<decimal> CreditQuantity { get; set; }
        public string CreditUnitCode { get; set; }
        public string CreditThirdPartyLiabilityNo { get; set; }

        public string UnderWriter { get; set; }
        [Required]
        public string CurencyCode { get; set; }

       
        public Nullable<decimal> AmountInCurrency { get; set; }
        
        public Nullable<decimal> ExchangeRate { get; set; }
        
        public Nullable<decimal> AmountInVND { get; set; }
       
        public Nullable<decimal> VatInCurency { get; set; }
      
        public Nullable<decimal> VatInVND { get; set; }
        public int FiscalYear { get; set; }
        public int Period { get; set; }
        public string BrokerCode { get; set; }
        public string AgentCode { get; set; }
        public string Note { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string CrossCompany { get; set; }
        public string CrossGLAccountID { get; set; }

        public string DebitOldBranch { get; set; }
        public string CreditOldBranch { get; set; }
        public Nullable<int> SeqNo { get; set; }


        //Cờ kiểm tra các ràng buộc Tài khoản
        public bool? DebitBillCodeFlag { get; set; }
        public bool? DebitBillFlag { get; set; }
        public bool? DebitBillDateFlag { get; set; }
        public bool? DebitTaxCodeFlag { get; set; }
        public bool? DebitBranchFlag { get; set; }
        public bool? DebitClaimFlag { get; set; }
        public bool? DebitDepartmentFlag { get; set; }
        public bool? DebitInsuranceTypeFlag { get; set; }
        public bool? DebitOpenAccountFlag { get; set; }
        public bool? DebitPolicyFlag { get; set; }
        public bool? DebitQuantityFlag { get; set; }
        public bool? DebitReinsuranceClaimFlag { get; set; }
        public bool? DebitReinsuranceFlag { get; set; }
        public bool? DebitThirdPartyLiablityFlag { get; set; }
        public bool? DebitVarianceFlag { get; set; }
        public bool? DebitCustomerFlag { get; set; }
        public bool? DebitVatTypeFlag { get; set; }
        public bool? DebitUnitFlag { get; set; }

        public bool? CreditBillCodeFlag { get; set; }
        public bool? CreditBillFlag { get; set; }
        public bool? CreditBillDateFlag { get; set; }
        public bool? CreditTaxCodeFlag { get; set; }
        public bool? CreditBranchFlag { get; set; }
        public bool? CreditClaimFlag { get; set; }
        public bool? CreditDepartmentFlag { get; set; }
        public bool? CreditInsuranceTypeFlag { get; set; }
        public bool? CreditOpenAccountFlag { get; set; }
        public bool? CreditPolicyFlag { get; set; }
        public bool? CreditQuantityFlag { get; set; }
        public bool? CreditReinsuranceClaimFlag { get; set; }
        public bool? CreditReinsuranceFlag { get; set; }
        public bool? CreditThirdPartyLiablityFlag { get; set; }
        public bool? CreditVarianceFlag { get; set; }
        public bool? CreditCustomerFlag { get; set; }
        public bool? CreditVatTypeFlag { get; set; }
        public bool? CreditUnitFlag { get; set; }

        public bool? UnderWriterFlag { get; set; }

        public DocumentInfo documentInfo { get; set; }
        public ContractInfo contractInfo { get; set; }
        public IndemnifyInfo indemnifyInfo { get; set; }
        //public List<GLAccountEntity> listAcc { get; set; }
        //public List<InsuranceTypeEntity> listInsType { get; set; }
        //public List<HierarchyEntity> listBranch { get; set; }
        //public List<HierarchyEntity> listDepartment { get; set; }
        //public List<VATCodeEntity> listVATCode { get; set; }
        //public List<CustomerEntity> listCustomer { get; set; }
        //public List<UnitEntity> listUnit { get; set; }
        //public List<CurrencyEntity> listCurrency { get; set; }
        //public List<BrokerEntity> listBroker { get; set; }
        //public List<HierarchyEntity> listUser { get; set; }
        //public List<HierarchyEntity> listAgent { get; set; }

        //public List<CommissionViewModel> listCommission { get; set; }
    }

    public class ContractInfo
    {
        public long STT { get; set; }
        public Guid Contract_ID { get; set; }
        public string Contract_No { get; set; }
        public Nullable<DateTime> Contract_Date { get; set; }
        public string Contract_SaleUser { get; set; }
        public string Contract_SaleName { get; set; }
        public Nullable<Int64> Contract_AgentID { get; set; }
        public Nullable<Int64> Contract_CustomerID { get; set; }
        public Nullable<DateTime> Contract_IssueDate { get; set; }
        public Nullable<DateTime> Contract_ExpireDate { get; set; }
        public string Contract_BillNo { get; set; }
        public Nullable<DateTime> Contract_BillDate { get; set; }
        public string StatusID { get; set; }
        public string Contract_Currency { get; set; }
        public string Contract_RiskType { get; set; }
        public Nullable<Guid> Contract_BlanketAgreementID { get; set; }
        public string Contract_Branch { get; set; }
        public decimal Contract_TaxExAmount { get; set; }
        public decimal Contract_InsuranceFeeExAmount { get; set; }
        public decimal Contract_InsuranceExAmount { get; set; }
        public decimal TotalFee { get; set; }
        public decimal PhiDaXuat { get; set; }
        public string BlanketAgreement_No { get; set; }
        public string Agent_Code { get; set; }
        public string Agent_Name { get; set; }
        public string Customer_No { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_TaxCode { get; set; }
        public string Contract_Status { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public Nullable<decimal> EndOpenAmount { get; set; }
        public string DepartmentCode { get; set; }
        public string Contract_BillCode { get; set; }
    }

    public class IndemnifyInfo:ContractInfo
    { 
    }

    public class DropdownlistFilter
    {
        public string FilterCode { get; set; }
        public string FilterName { get; set; }
    }
}
