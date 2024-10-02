
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;



namespace CertificationWeb.CustomAuthentication
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }

        private class CustomAuthorizeFilter : IAuthorizationFilter
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CustomAuthorizeFilter(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }
            protected virtual CustomPrincipal CurrentUser
            {
                get
                {
                    var user = _httpContextAccessor.HttpContext.User;
                    if (user == null || !user.Identity.IsAuthenticated)
                    {
                        return null;
                    }

                    // Create a CustomPrincipal from the ClaimsPrincipal
                    return new CustomPrincipal(user);
                }
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (CurrentUser == null || !AuthorizeCore())
                {
                    HandleUnauthorizedRequest(context);
                }
            }

            private bool AuthorizeCore()
            {
              
                //return CurrentUser != null && CurrentUser.IsInRole("Roles");
                return CurrentUser == null ? false : true;
            }

            private void HandleUnauthorizedRequest(AuthorizationFilterContext context)
            {
                if (CurrentUser == null)
                {
                    context.Result = new RedirectToRouteResult(
                        new Microsoft.AspNetCore.Routing.RouteValueDictionary(new
                        {
                            controller = "Account",
                            action = "Login"
                        }));
                }
                else
                {
                    context.Result = new RedirectToRouteResult(
                        new Microsoft.AspNetCore.Routing.RouteValueDictionary(new
                        {
                            controller = "Error",
                            action = "AccessDenied"
                        }));
                }
            }
        }
    }
}