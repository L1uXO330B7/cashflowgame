using Common.Enum;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace API.Filter
{
    public class ModelStateErrorActionFilter : IActionFilter
    {
        /// <summary>
        /// Model 格式驗證
        /// 參考：https://stackoverflow.com/questions/66255339/any-way-we-can-override-data-annotation-response-in-asp-net-core-3-1
        /// </summary>
        /// <param name="_ActionExecutingContext"></param>
        public void OnActionExecuting(ActionExecutingContext _ActionExecutingContext)
        {
            if (!_ActionExecutingContext.ModelState.IsValid) // Model 格式驗證錯誤時
            {
                var ErrorMsgs = _ActionExecutingContext.ModelState
                .Where(modelError => modelError.Value.Errors.Count > 0)
                .Select(modelError => new ModelStateError
                {
                    ErrorField = modelError.Key,
                    ErrorDescription = modelError.Value.Errors.FirstOrDefault().ErrorMessage
                }).ToList();

                var Res = new ApiResponse();
                Res.Code = (int)ResponseStatusCode.FormatValidationError;
                Res.Message = string.Join('\n', ErrorMsgs.Select((x,Index) => (Index+1)+". "+x.ErrorDescription));
                Res.Success = false;

                _ActionExecutingContext.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(Res)
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext _ActionExecutingContext)
        {

        }

        public class ModelStateError
        {
            public string ErrorField { get; set; }
            public string ErrorDescription { get; set; }
        }

        // 參考:https://code-maze.com/aspnetcore-modelstate-validation-web-api/

        //_ActionExecutingContext.Result = new UnprocessableEntityObjectResult(_ActionExecutingContext.ModelState);

        //var ObjectResult = new UnprocessableEntityObjectResult(_ActionExecutingContext.ModelState);

        //var Props = ObjectResult.Value
        //    .GetType()
        //    .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //var ErrorMsgs = new List<string>();

        //foreach (var Prop in Props)
        //{
        //    if (Prop.Name == "Values")
        //    {
        //        var Temp = Prop.GetValue(ObjectResult.Value, null);
        //        if (Temp != null)
        //        {
        //            var PropValues = (List<List<string>>)(Temp);
        //            foreach(var PropValue in PropValues)
        //            {
        //                foreach (var ErrorMsg in PropValue)
        //                {
        //                    ErrorMsgs.Add(ErrorMsg);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
