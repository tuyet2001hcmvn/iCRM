using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using ISD.ViewModels.MasterData;

namespace ISD.API.Controllers
{
    public class MasterDataController : BaseController
    {
        // GET: MasterData
        #region Get News
        public ActionResult GetNewsBy(Guid? AccountId, string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new List<NewsMobileViewModel>();
                result = _unitOfWork.NewsRepository.GetNewsBy(AccountId, CompanyCode);
                return _APISuccess(result);
            });
        }

        public ActionResult GetNewsDetails(Guid NewsId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new NewsMobileViewModel();
                result = _unitOfWork.NewsRepository.GetNewsDetails(NewsId);
                return _APISuccess(result);
            });
        }
        #endregion Get News

        //Use for AC Library(AC Catalogue)
        #region Province, District
        public ActionResult GetProvince()
        {
            return ExecuteAPIWithoutAuthContainer(() =>
            {
                var _provinceRepository = new ProvinceRepository(_context);
                var provinceList = _provinceRepository.GetAll()
                                                    .Select(p => new ISDSelectGuidItem()
                                                    {
                                                        id = p.ProvinceId,
                                                        name = p.ProvinceName,
                                                    });
                return _APISuccess(provinceList);
            });
        }

        public ActionResult GetDistrictBy(Guid ProvinceId)
        {
            return ExecuteAPIWithoutAuthContainer(() =>
            {
                var _districtRepository = new DistrictRepository(_context);
                var districtList = _districtRepository.GetBy(ProvinceId)
                                                .Select(p => new ISDSelectGuidItem()
                                                {
                                                    id = p.DistrictId,
                                                    name = p.DistrictName,
                                                }).OrderBy(p => p.name.Length).ThenBy(p => p.name);
                return _APISuccess(districtList);
            });
        }
        #endregion Province, District

        //Question Category
        #region Danh mục câu hỏi
        public ActionResult GetQuestionCategory(string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new List<ISDSelectGuidItem>();
                result = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.QuestionCategory).Select(p => new ISDSelectGuidItem
                {
                    id = p.CatalogId,
                    name = p.CatalogText_vi,
                }).ToList();
                result.Insert(0, new ISDSelectGuidItem
                {
                    id = Guid.Empty,
                    name = "-- Tất cả --",
                });
                return _APISuccess(result);
            });
        }
        #endregion

        #region Tra cứu danh sách câu hỏi
        public ActionResult SearchQuestionBank(string Question, Guid? QuestionCategoryId, string token, string key, int PageSize = 10, int PageNumber = 1)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
              var result = (from question in _context.QuestionBankModel
                          join category in _context.CatalogModel on question.QuestionCategoryId equals category.CatalogId
                          join department in _context.CatalogModel on question.DepartmentId equals department.CatalogId
                          join account in _context.AccountModel on question.CreateBy equals account.AccountId
                          where (Question == null || Question == "" || question.Question.Contains(Question)) &&
                                  (QuestionCategoryId == null || QuestionCategoryId == Guid.Empty || question.QuestionCategoryId == QuestionCategoryId) 
                                  //&&
                                  //(Actived == null || question.Actived == Actived)
                          orderby question.QuestionBankCode
                          select new QuestionResultViewModel()
                          {
                              QuestionBankCode = question.QuestionBankCode,
                              Question = question.Question,
                              Answer = question.Answer,
                              AnswerC = question.AnswerC,
                              AnswerB = question.AnswerB,
                              QuestionCategoryName = category.CatalogText_vi,
                              CreateBy = account.FullName,
                              DepartmentName = department.CatalogText_vi,
                              CreateTime = question.CreateTime
                          }).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.Answer))
                        {
                            item.Answer = HtmlToText.ConvertHtml(item.Answer);
                        }

                        if (!string.IsNullOrEmpty(item.AnswerC))
                        {
                            item.AnswerC = HtmlToText.ConvertHtml(item.AnswerC);
                        }
                        if (!string.IsNullOrEmpty(item.AnswerB))
                        {
                            item.AnswerB = HtmlToText.ConvertHtml(item.AnswerB);
                        }
                    }
                }
                return _APISuccess(result);
            });
        }
        #endregion
    }
}