using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.Paging
{
    public class PagedRequest
    {
        protected int MaxPageSize = 500;
        protected int DefaultPageSize = 20;
        private int _pageSize;
        private int _pageNumber;
        private string _searchVal = string.Empty;
        private string _SortType = "ASC";
        private string _SortingCol = string.Empty;
        public virtual int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(Math.Max(value, 1), MaxPageSize);
        }
        public virtual int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = Math.Max(value, 1);
        }
        public virtual string SearchVal
        {
            get => _searchVal;
            set => _searchVal = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        public virtual string SortingCol
        {
            get => _SortingCol;
            set => _SortingCol = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        public virtual string SortType
        {
            get => _SortType;
            set => _SortType = value is not null && value.Equals("DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "DESC"
                : "ASC";
        }
    }
}
