using System;

namespace WebLib
{
    public class cNavigation
    {
        private System.Web.UI.Page PG=new System.Web.UI.Page();
        public String State;

        public int ActivePage
        {
            get{
                return Convert.ToInt32(PG.Session["activepage"]);                
            }
            set{
                PG.Session["activepage"] = value;
            }
        }

        #region Property Collection
        /// <summary>
        /// Property total row
        /// </summary>
        public int TotalRow
        {
            get{
                return Convert.ToInt32(PG.Session["totalrow"]);
            }
            set {
                PG.Session["totalrow"] = value;
            }
        }
        /// <summary>
        /// property paging size
        /// </summary>
        public int PagingSize
        {
            get{
                return Convert.ToInt32(PG.Session["pagingsize"]);
            }
            set{
                PG.Session["pagingsize"] = value;
            }
        }
        /// <summary>
        /// property total page
        /// </summary>
        public int TotalPage
        {
            get
            {
                int _Result=0;
                try
                {
                    _Result = Convert.ToInt32(Math.Round(Decimal.Truncate(Convert.ToInt32(PG.Session["totalrow"]) / Convert.ToInt32(PG.Session["pagingsize"])), 0));
                    if (Convert.ToInt32(PG.Session["totalrow"]) - (_Result * Convert.ToInt32(PG.Session["pagingsize"])) > 0)
                        _Result = _Result + 1;
                }catch(Exception ex){
                    Console.WriteLine(ex.Message);
                }
                return _Result;
                
            }
        }
        /// <summary>
        /// property to get filter
        /// </summary>
        /// <param name="_SearchFilter">key filter that you want to search</param>
        /// <returns></returns>
        public string GetFilter(string _SearchFilter)
        {
            string _Result=PG.Session["filterclause"].ToString();
            if(_SearchFilter.Length==0)
                _Result=PG.Session["filterclause"].ToString();
            else
            {
                if(PG.Session["filterclause"].ToString().Length==0)
                    _Result=_SearchFilter;
                else
                    _Result="(" + PG.Session["filterclause"] + ") AND (" + PG.Session["searchfilter"] + ")";
            }
            if(_Result==null)
                _Result="";
            return _Result;          
        }
        /// <summary>
        /// property to set filter
        /// </summary>
        public string SetFilter
        {
            set
            {
                PG.Session["filterclause"] = value;
            }
        }
        /// <summary>
        /// property to get order
        /// </summary>
        /// <param name="_OtherOrder"></param>
        /// <returns></returns>
        public string GetOrder(string _OtherOrder)
        {
            string _Result = _OtherOrder;
            if (_OtherOrder.Length > 0)
            {
                if (PG.Session["orderclause"].ToString().Length > 0)
                    _Result = _OtherOrder + "," + PG.Session["orderclause"];
                else
                    _Result = _OtherOrder;
            }
            else
            {
                _Result = PG.Session["orderclause"].ToString();
            }
            if (_Result == null)
                _Result = "";
            return PG.Session["orderclasuse"].ToString();
        }
        /// <summary>
        /// property to set order
        /// </summary>
        public string SetOrder
        {
            set
            {
                PG.Session["orderclause"] = value;
            }
        }
        #endregion

        /// <summary>
        /// move to first page
        /// </summary>
        public void MoveFirst()
        {
            PG.Session["activepage"] = 1;
            State = "browse";
        }
        /// <summary>
        /// move to last page
        /// </summary>
        public void MoveLast()
        {
            PG.Session["activepage"] = TotalPage;
            State = "browse";
        }
        /// <summary>
        /// move to next page
        /// </summary>
        public void NextPage()
        {
            if(Convert.ToInt32(PG.Session["activepage"]) < TotalPage && TotalPage>1)
            {
                PG.Session["activepage"] = Convert.ToInt32(PG.Session["activepage"]) + 1;
            }
            else if((TotalPage == 1 && (Convert.ToInt32(PG.Session["activepage"])) > 1))
            {
                PG.Session["activepage"] = 1;
            }
            State = "browse";
        }
        /// <summary>
        /// move to previous page
        /// </summary>
        public void PreviousPage()
        {
            if (Convert.ToInt32(PG.Session["activepage"]) > 1)
            {
                PG.Session["activepage"] = Convert.ToInt32(PG.Session["activepage"]) - 1;
                State = "browse";
            }
        }
        /// <summary>
        /// jump to the page
        /// </summary>
        /// <param name="_Value">page number that you want to jump</param>
        public void JumpPage(int _Value)
        {
            //if (_Value == 1 && _Value <= TotalPage)
            //{
                PG.Session["activepage"] = _Value;
                State = "jump";
            //}
        }
        public void Fill(int CurrentPage, int RowCount)
        {
            PG.Session["activepage"] = CurrentPage;
            PG.Session["totalrow"] = RowCount;
        }
    }
}
