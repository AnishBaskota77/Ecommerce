using FAMAndIMS.Common;
using FAMAndIMS.Data.Services.UserActivityLogServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace FAMAndIMS.Filter
{
    public class LogUserActivityAttribute : TypeFilterAttribute
    {
        public LogUserActivityAttribute(string userAction = "", bool logRequestBody = true) : base(typeof(LogUserActivityFilter))
        {
            Arguments = new object[] { userAction, logRequestBody };
        }
        public class LogUserActivityFilter : IAsyncResourceFilter
        {
            private readonly string _userAction;
            private readonly bool _logRequestBody;
            private readonly IUserActivityLogs _activityLogService;

            private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _hostEnv;


            public LogUserActivityFilter(string userAction, bool logRequestBody, IUserActivityLogs activityLogService, Microsoft.Extensions.Hosting.IHostingEnvironment hostEnv)
            {
                _userAction = userAction;
                _logRequestBody = logRequestBody;
                _activityLogService = activityLogService;
                _hostEnv = hostEnv;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                await LogUserActivityAsync(context.HttpContext);
                await next();
            }

            private async Task LogUserActivityAsync(HttpContext context)
            {
                try
                { 
                    var userActivityLogParams = await LogHelper.GetUserActivityLogAsync(context, _hostEnv, logRequestBody: _logRequestBody, userAction: _userAction);

                    await _activityLogService.AddAsync(userActivityLogParams);
                }
                catch (Exception) { }
            }
        }
    }
}
