namespace Winner.WebApi.App_Start
{
    using System.Web.Mvc;

    public class FilterConfig
    {
        #region Public static members

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}