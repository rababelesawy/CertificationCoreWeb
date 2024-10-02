using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Certification.Domain.DomainModels
{
    public class PagedResultViewModel<T>
    {
        public IPagedList<T> Items { get; set; } = new List<T>().ToPagedList();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
