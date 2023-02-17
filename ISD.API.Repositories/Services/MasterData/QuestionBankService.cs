using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.MasterData
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public QuestionBankService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public QuestionViewModel Create(QuestionCreateViewModel obj)
        {
            var newQuestion = _mapper.Map<QuestionBankModel>(obj);
            if(string.IsNullOrEmpty(obj.Answer)&& string.IsNullOrEmpty(obj.AnswerC)&& string.IsNullOrEmpty(obj.AnswerB))
            {
                newQuestion.Actived = false;
            }
            else
            {
                newQuestion.Actived = true;
            }
            _unitOfWork.QuestionBankRepository.Add(newQuestion);
            _unitOfWork.Save();
            return _mapper.Map<QuestionViewModel>(newQuestion);
        }

        public QuestionViewModel GetById(Guid id)
        {
            QuestionBankModel model = _unitOfWork.QuestionBankRepository.GetBy(m => m.Id == id, m => m.CreateByNavigation, m => m.LastEditByNavigation, m => m.QuestionCategory, m => m.Department);
            if (model == null)
            {
                throw new ResourceNotFoundException("Question {0} was not found", id);
            }
            return _mapper.Map<QuestionViewModel>(model);
        }

        public List<DepartmentViewModel> GetDepartment()
        {
            var list = _unitOfWork.QuestionBankRepository.GetDepartment().ToList();
            return _mapper.Map<List<CatalogModel>, List<DepartmentViewModel>>(list);
        }

        public List<QuestionCategoryViewModel> GetQuestionCategoriesAvailable()
        {
            var listQuestionCategory = _unitOfWork.QuestionBankRepository.GetQuestionCategoriesAvailable().ToList();
            return _mapper.Map<List<CatalogModel>, List<QuestionCategoryViewModel>>(listQuestionCategory);
        }

        public List<QuestionViewModel> Search(int? questionBankCode, string question, Guid questionCategoryId, bool? actived, int pageIndex, int pageSize, out int totalRow)
        {
            var list = _unitOfWork.QuestionBankRepository.Search(questionBankCode, question, questionCategoryId, actived, out totalRow);
            if(list.Count()!=0)
            {
                var pagging=list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<QuestionBankModel>, List<QuestionViewModel>>(pagging);
            }
            return _mapper.Map<List<QuestionBankModel>, List<QuestionViewModel>>(list.ToList());
        }

        public void Update(Guid id, QuestionEditViewModel obj)
        {
            QuestionBankModel model = _unitOfWork.QuestionBankRepository.GetById(id);
            if (model == null)
            {
                throw new ResourceNotFoundException("Question {0} was not found", id);
            }
            _mapper.Map<QuestionEditViewModel, QuestionBankModel>(obj, model);
            if (string.IsNullOrEmpty(obj.Answer) && string.IsNullOrEmpty(obj.AnswerC) && string.IsNullOrEmpty(obj.AnswerB))
            {
                model.Actived = false;
            }
            else
            {
                model.Actived = true;
            }
            _unitOfWork.QuestionBankRepository.Update(model);
            _unitOfWork.Save();
        }
    }
}
