namespace Perevorot.Web.Helpers
{
    public class DatatablesResponseInfo
    {
        public DatatablesResponseInfo(DatatablesRequestInfo datatablesRequestInfo)
        {
            sEcho = datatablesRequestInfo.sEcho;

        }

        //Total records, before filtering (i.e. the total number of records in the database)
        public int iTotalRecords { get; set; }

        //Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned in this result set)
        public int iTotalDisplayRecords { get; set; }

        //An unaltered copy of sEcho sent from the client side. This parameter will change with each draw (it is basically a draw count) - so it is important that this is implemented. Note that it strongly recommended for security reasons that you 'cast' this parameter to an integer in order to prevent Cross Site Scripting (XSS) attacks.
        public string sEcho { get; set; }

        //The data in a 2D array. Note that you can change the name of this parameter with sAjaxDataProp.
        public dynamic aaData { get; set; }
        
    }
}