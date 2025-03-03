using FAMAndIMS.Data.Model.UserActivityLogModel;
using System.Security.Claims;
using System.Text;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace FAMAndIMS.Common
{
    public class LogHelper
    {
        public static async Task<UserActivityLogModel> GetUserActivityLogAsync(HttpContext context, IHostingEnvironment hostEnv = null, bool logRequestBody = true, string userAction = "")
        {
            var remoteIpAddress = GetIpAddress(context);
            var httpMethod = context.Request.Method;
            var requestUrl = GetRequestUrl(context);
            var queryString = GetQueryString(context);
            var userAgent = GetUserAgent(context);
            var headers = GetRequestHeaders(context);
            var controllerName = GetController(context);
            var actionName = GetAction(context);
            var environment = hostEnv?.EnvironmentName ?? null;

            var activityLogParam = new UserActivityLogModel
            {
                RequestUrl = requestUrl,
                QueryString = string.IsNullOrWhiteSpace(queryString) ? null : queryString,
                Environment = environment,
                RemoteIpAddress = remoteIpAddress,
                HttpMethod = httpMethod,
                ControllerName = controllerName,
                ActionName = actionName,
                UserAgent = userAgent,
                Headers = headers,
                MachineName = Environment.MachineName,
                UserAction = userAction
            };

            if (logRequestBody)
            {
                var (isForm, requestBody) = await GetRequestBodyAsString(context);
                activityLogParam.IsFormData = isForm;
                activityLogParam.RequestBody = requestBody;
            }

            if (context.User.Identity.IsAuthenticated)
            {
                var userName = GetUsername(context);
                var email = GetUserEmail(context);


                activityLogParam.UserName = userName;
                activityLogParam.Email = email;
            }

            return activityLogParam;
        }
        private static async Task<(bool isForm, string body)> GetRequestBodyAsString(HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (!context.Request.Body.CanRead)
                return (false, null);

            context.Request.EnableBuffering();

            string requestBody;
            if (context.Request.HasFormContentType)
            {
                requestBody = GetRequestBodyFormData(context);
                return (true, requestBody);
            }

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            _ = await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Position = 0;

            return (false, requestBody);
        }

        private static string GetUsername(HttpContext context)
        {
            return context.User?.FindFirst(
                c => c.Type == ClaimTypes.Name)?.Value ?? context.User?.Identity?.Name;
        }

        private static string GetUserEmail(HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        }
        private static string RoleName(HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
        }

        private static string GetRequestBodyFormData(HttpContext context)
        {
            var form = context.Request.Form;
            var formData = new List<string>();
            foreach (var key in form.Keys)
            {
                if (form[key].Count > 1)
                {
                    // If there are multiple values for the same key, add them all to the list.
                    foreach (var value in form[key])
                    {
                        if (!string.IsNullOrEmpty(value) && !IsFile(value))
                            formData.Add($"{key}={value}");
                    }
                }
                else
                {
                    // If there is only one value for the key, add it to the list.
                    var value = form[key];
                    if (!string.IsNullOrEmpty(value) && !IsFile(value))
                        formData.Add($"{key}={value}");
                }
            }

            return string.Join("&", formData);
        }
        private static string GetRequestHeaders(HttpContext context)
        {
            var headers = new List<string>();

            foreach (var header in context.Request.Headers)
                headers.Add($"{header.Key}: {header.Value}");

            return string.Join("; ", headers);
        }

        private static string GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        private static string GetController(HttpContext context)
        {
            //return context.Request.RouteValues["controller"]?.ToString();

            var routeData = context.GetRouteData();
            var controllerName = routeData.Values["controller"]?.ToString();
            return controllerName;

        }


        private static string GetAction(HttpContext context)
        {
            // return context.Request.RouteValues["action"]?.ToString();
            var routeData = context.GetRouteData();
            var ActionName = routeData.Values["action"]?.ToString();
            return ActionName;
        }

        private static string GetQueryString(HttpContext context)
        {
            return context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
        }

        private static string GetUserAgent(HttpContext context)
        {
            context.Request.Headers.TryGetValue("User-Agent", out var userAgent);
            return userAgent;
        }
        private static string GetRequestUrl(HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        }

        private static bool IsFile(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.StartsWith("filename=", StringComparison.OrdinalIgnoreCase) || value.StartsWith("content-type=", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
