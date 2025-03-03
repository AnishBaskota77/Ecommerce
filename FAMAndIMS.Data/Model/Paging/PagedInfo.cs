using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.Paging
{
    public class PagedInfo
    {
        private string _sortBy = string.Empty;
        private string _sortOrder = string.Empty;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int Showingfrom { get; set; }
        public int ShowingTo { get; set; }
        public virtual string SortingCol
        {
            get => _sortBy;
            set => _sortBy = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        public virtual string SortType
        {
            get => _sortOrder;
            set => _sortOrder = value is not null && value.Equals("DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "DESC"
                : "ASC";
        }
    }
}
