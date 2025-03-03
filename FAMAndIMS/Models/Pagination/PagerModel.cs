using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Models.Pagination
{
    public class PagerModel : ActionSpecs
    {
        private const int _noOfPagerButtons = 5;
        private readonly int _noOfButtonsToTheLeftFromCurrentPagerBtn = (int)Math.Ceiling((decimal)_noOfPagerButtons / 2);
        private readonly int _noOfButtonsToTheRightFromCurrentPagerBtn = (int)Math.Ceiling((decimal)_noOfPagerButtons / 2) - 1;

        public int StartPage { get; set; }
        public int CurrentPage { get; set; }
        public int EndPage { get; set; }
        public int TotalPages { get; set; }

        public PagerModel(int totalPages, int pageIndex, string controller, string action)
        {
            TotalPages = totalPages;
            CurrentPage = pageIndex;

            int startPage = CurrentPage - _noOfButtonsToTheLeftFromCurrentPagerBtn;
            int endPage = CurrentPage + _noOfButtonsToTheRightFromCurrentPagerBtn;

            if (startPage <= 0)
            {
                endPage -= startPage - 1;
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > _noOfPagerButtons)
                {
                    startPage = endPage - _noOfPagerButtons - 1;
                }
            }

            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            Controller = controller;
            Action = action;
        }

        public bool IsFirstPage => CurrentPage == 1;

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public bool IsLastPage => CurrentPage == TotalPages;
    }
}
