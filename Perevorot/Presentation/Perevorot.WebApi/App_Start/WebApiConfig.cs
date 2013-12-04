namespace Winner.WebApi.App_Start
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        #region Public static members

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute
                (
                    name : "DefaultApi",
                    routeTemplate : "api/{controller}/{id}",
                    defaults : new {id = RouteParameter.Optional}
                );
        }

        #endregion
    }
}