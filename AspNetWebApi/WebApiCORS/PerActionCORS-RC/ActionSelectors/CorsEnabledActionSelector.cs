using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PerActionCORS_RC.Filters;

namespace PerActionCORS_RC.ActionSelectors
{
    public class CorsEnabledActionSelector : ApiControllerActionSelector
    {
        private const string Origin = "Origin";
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var originalRequest = controllerContext.Request;
            var isCorsRequest = originalRequest.Headers.Contains(Origin);

            if (originalRequest.Method == HttpMethod.Options && isCorsRequest)
            {
                var currentAccessControlRequestMethod = originalRequest.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                if (!string.IsNullOrEmpty(currentAccessControlRequestMethod))
                {
                    var modifiedRequest = new HttpRequestMessage(
                        new HttpMethod(currentAccessControlRequestMethod),
                        originalRequest.RequestUri);
                    controllerContext.Request = modifiedRequest;
                    var actualDescriptor = base.SelectAction(controllerContext);
                    controllerContext.Request = originalRequest;

                    if (actualDescriptor != null)
                    {
                        if (actualDescriptor.GetFilters().OfType<EnableCorsAttribute>().Any())
                        {
                            var descriptor = new PreflightActionDescriptor(actualDescriptor, currentAccessControlRequestMethod);
                            return descriptor;
                        }
                    }
                }
            }

            return base.SelectAction(controllerContext);
        }

        class PreflightActionDescriptor : HttpActionDescriptor
        {
            private readonly HttpActionDescriptor originalAction;
            private readonly string prefilghtAccessControlRequestMethod;
            private HttpActionBinding actionBinding;

            public PreflightActionDescriptor(HttpActionDescriptor originalAction, string accessControlRequestMethod)
            {
                this.originalAction = originalAction;
                this.prefilghtAccessControlRequestMethod = accessControlRequestMethod;
                this.actionBinding = new HttpActionBinding(this, new HttpParameterBinding[0]);
            }

            public override string ActionName
            {
                get { return originalAction.ActionName; }
            }

            public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Headers.Add(AccessControlAllowMethods, prefilghtAccessControlRequestMethod);

                var requestedHeaders = string.Join(", ", controllerContext.Request.Headers.GetValues(AccessControlRequestHeaders));

                if (!string.IsNullOrEmpty(requestedHeaders))
                {
                    response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                }

                var tcs = new TaskCompletionSource<object>();
                tcs.SetResult(response);
                return tcs.Task;
            }

            public override Collection<HttpParameterDescriptor> GetParameters()
            {
                return originalAction.GetParameters();
            }

            public override Type ReturnType
            {
                get { return typeof(HttpResponseMessage); }
            }

            public override Collection<FilterInfo> GetFilterPipeline()
            {
                return originalAction.GetFilterPipeline();
            }

            public override Collection<IFilter> GetFilters()
            {
                return originalAction.GetFilters();
            }

            public override Collection<T> GetCustomAttributes<T>()
            {
                return originalAction.GetCustomAttributes<T>();
            }

            public override HttpActionBinding ActionBinding
            {
                get { return this.actionBinding; }
                set { this.actionBinding = value; }
            }
        }
    }
}