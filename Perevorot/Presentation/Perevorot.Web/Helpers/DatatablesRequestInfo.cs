namespace Perevorot.Web.Helpers
{
    public class DatatablesRequestInfo
    {
        public int iDisplayStart { get; set; }    //Display start point in the current data set.
        public int iDisplayLength { get; set; }    //Number of records that the table can display in the current draw. It is expected that the number of records returned will be equal to this number, unless the server has fewer records to return.
        public int iColumns { get; set; }    //Number of columns being displayed (useful for getting individual column search info)
        public string sSearch { get; set; }    //Global search field
        public bool bRegex { get; set; }    //True if the global filter should be treated as a regular expression for advanced filtering, false if not.
        public bool bSearchable { get; set; }    //_(int) 	Indicator for if a column is flagged as searchable or not on the client-side
        //public string sSearch { get; set; }    //_(int) 	Individual column filter
        //public bool bRegex { get; set; }    //_(int) 	True if the individual column filter should be treated as a regular expression for advanced filtering, false if not
        public bool bSortable { get; set; }    //_(int) 	Indicator for if a column is flagged as sortable or not on the client-side
        public int iSortingCols { get; set; }    //Number of columns to sort on
        public int iSortCol { get; set; }    //_(int) 	Column being sorted on (you will need to decode this number for your database)
        public string sSortDir { get; set; }    //_(int) 	Direction to be sorted - "desc" or "asc".
        public string mDataProp { get; set; }    //_(int) 	The value specified by mDataProp for each column. This can be useful for ensuring that the processing of data is independent from the order of the columns.
        public string sEcho { get; set; }    //Information for DataTables to use for rendering.        
    }
}