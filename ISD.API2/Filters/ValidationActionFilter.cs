using ISD.API.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API2.Filters
{
    public class ValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var listError = GetErrorListFromModelState(modelState);
                ApiResponse res = new ApiResponse
                {
                    Code = 400,
                    IsSuccess = false,
                };
                foreach(var mess in listError)
                {
                    res.Message += mess;
                }
                context.Result = new BadRequestObjectResult(res);
            }
        }
        private static List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errorList = query.ToList();
            return errorList;
        }
    }
}
