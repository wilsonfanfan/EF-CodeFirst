using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.IDao;

namespace X.IService
{
    /// <summary>
    /// 接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseService<TEntity> where TEntity : Models.BaseModel
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        IResponse<TEntity> Create(TEntity model);
        IListResult<TEntity> GetList(string Filter = null, string Sort = null, params object[] Values);

        IPageResult<TEntity> GetPageList(string Filter, string Sort, int PageNumber = 1, int PageSize = 20, params object[] Values);

        IResponse<TEntity> GetById(long ID);
        IResponse<TEntity> Get(string Filter, params object[] values);

        IResponse<bool> Update(TEntity model, params string[] values);
        IResponse<int> Update(Dictionary<string, string> list, string where, params object[] values);
        IResponse<int> Update(string updateset, string where, params object[] values);
        IResponse<int> SetProperty(string field, int val, params long[] ids);
        IResponse<bool> Delete(long ID);
        IResponse<bool> DeleteIDs(List<long> IDs);

        IResponse<bool> Exists(string Filter, params object[] Values);

        IResponse<int> Count(string Filter, params object[] Values);
    }
}
