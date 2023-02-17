using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class SAPGetDanhMucViewModel
    {
        public string Type { get; set; }
        public string CompanyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public decimal? FromValue { get; set; }
        public decimal? ToValue { get; set; }
    }


    public class SAPGetCongNoProfileViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string Year { get; set; }
        public string ProfileCode { get; set; }
        public decimal? DoanhSo { get; set; }
        public decimal? ThanhToan { get; set; }
        public decimal? ConLai { get; set; }
        public decimal? DuDauKy { get; set; }
        public decimal? NoTrongKy { get; set; }
        public decimal? CoTrongKy { get; set; }
        public decimal? DuCuoiKy { get; set; }
    }

    public class SAPGetMuaHangProfileViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string GroupProductCode { get; set; }
        public string GroupProductName { get; set; }
        public decimal? Year { get; set; }
        public decimal? Year1 { get; set; }
        public decimal? Year2 { get; set; }
        public decimal? Total { get; set; }
    }

    public class SAPGetPhanCapDoanhSoViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string SalesEmployeeName { get; set; }
        public int? ColNo { get; set; }
        public string ColName { get; set; }
        public int? ColValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? FromValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? ToValue { get; set; }
    }
    public class SAPGetTangTruongDoanhSoViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string SalesEmployeeName { get; set; }
        public int? ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        public string Value { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? DoanhSo1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? DoanhSo2 { get; set; }
    }
    public class SAPGetChiTietDoanhSoViewModel
    {
        public int? STT { get; set; }
        public string CompanyCode { get; set; }
        public string RolesCode { get; set; }
        public string RolesName { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string SalesEmployeeName { get; set; }
        public int? ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Value { get; set; }
        public string Address { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerCareerCode { get; set; }
        public string CustomerCareerName { get; set; }
        public string SaleOfficeCode { get; set; }
        public string SaleOfficeName { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }

    
    public class SAPValueSOViewModel
    {
        public int? STT { get; set; }
        public string SO { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Value { get; set; }
    }
}
