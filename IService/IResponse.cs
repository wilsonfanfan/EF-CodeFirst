using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.IService
{
    public class IResponse<T>
    {
        public bool Status { get; set; }
        public T Result { get; set; }
        public string ErrMsg { get; set; }
        public int ErrCode { get; set; }
    }
    public class IListResult<T>
    {
        public bool Status { get; set; }
        public List<T> Result { get; set; }
        public string ErrMsg { get; set; }
        public int ErrCode { get; set; }
    }
    public class IPageResult<T> where T : class
    {
        public bool Status { get; set; }
        public PagedResult<T> Result { get; set; }
        public string ErrMsg { get; set; }
        public int ErrCode { get; set; }
    }
    public class PagedResult<T> where T : class
    {
        public long TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
