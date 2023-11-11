using System;

namespace WebLib.Web
{
    public interface IControlPaging
    {
        Int32 CurrentPagingSize { get; set; }
        String CurrentSort { get; set; }
        String CurrentSortDirection { get; set; }
        Int32 CurrentActivePage { get; set; }
        Int32 CurrentTotalRow { get;set; }
        void SetCurrentPageTo(Int32 pageNumber);
        void SetCurrentPageToLast();
        void SetCurrentPageToFirst();
        void SetCurrentPageToPrevious();
        void SetCurrentPageToNext();
    }
}