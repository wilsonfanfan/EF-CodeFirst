using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Text;
using EntityFramework;
using EntityFramework.Extensions;

namespace System.Linq
{
    public static class LinqExt
    {
        #region Linq 扩展
        //分页
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageIndex, int pageSize, out long recordCount)
        {
            Expression<Func<T, string>> order = null;
            return Page(query, pageIndex, pageSize, order, false, out recordCount);
        }

        //分页
        public static IQueryable<T> Page<T, TResult>(this IQueryable<T> query, int pageIndex, int pageSize, Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out long recordCount)
        {
            if (pageSize <= 0)
            {
                pageSize = 20;
            }

            recordCount = query.LongCount();

            if (recordCount <= pageSize || pageIndex <= 0)
            {
                pageIndex = 1;
            }

            int excludedRows = (pageIndex - 1) * pageSize;

            if (orderByProperty != null)
            {
                if (isAscendingOrder)
                {
                    query = query.OrderBy(orderByProperty);
                }
                else
                {
                    query = query.OrderByDescending(orderByProperty);
                }
            }
            if (pageIndex == 1)
            {
                return query.Take(pageSize);
            }
            else
            {
                return query.Skip(excludedRows).Take(pageSize);
            }
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="db"></param>
        /// <param name="propertie"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public static int UpdateBatch<TEntity>(this DbContext db, Expression<Func<TEntity, bool>> propertie, Expression<Func<TEntity>> updateExpression) where TEntity : class
        {
            var query = db.Set<TEntity>().Where(propertie);
            ObjectQuery objQuery = query.ToObjectQuery();
            List<object> objParams = new List<object>();
            string sql = objQuery.ToTraceString().Replace("[dbo].", "").Replace("[Extent1].", "").Replace("AS [Extent1]", "");
            int whereindex = sql.IndexOf("where", StringComparison.OrdinalIgnoreCase);
            int fromindex = sql.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            string where = sql.Substring(whereindex).Replace("__linq__", "");
            string tableName = sql.Substring(fromindex + 4, whereindex - fromindex - 4);
            int paramindex = objQuery.Parameters.Count;
            foreach (var para in objQuery.Parameters)
            {
                objParams.Add(para.Value);
            }

            var valueObj = updateExpression.Compile().Invoke();
            MemberInitExpression memberInitExpression = (MemberInitExpression)updateExpression.Body;
            if (memberInitExpression == null)
            {
                return 0;
            }
            Type valueType = typeof(TEntity);
            StringBuilder updateBuilder = new StringBuilder();
            foreach (var bind in memberInitExpression.Bindings.Cast<MemberAssignment>())
            {
                string name = bind.Member.Name;
                updateBuilder.AppendFormat(",{0}=@p{1}", name, paramindex++);
                var value = valueType.GetProperty(name).GetValue(valueObj);
                objParams.Add(value);
            }
            if (updateBuilder.Length == 0)
            {
                return 0;
            }
            sql = string.Format("UPDATE {0} SET {1} {2}", tableName, updateBuilder.Remove(0, 1).ToString(), where);
            return db.Database.ExecuteSqlCommand(sql, objParams.ToArray());
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="db"></param>
        /// <param name="propertie"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public static int UpdateBatch<TEntity>(this DbContext db, Expression<Func<TEntity, bool>> propertie, string name, string value) where TEntity : class
        {
            var query = db.Set<TEntity>().Where(propertie);
            ObjectQuery objQuery = query.ToObjectQuery();
            List<object> objParams = new List<object>();
            string sql = objQuery.ToTraceString();
            int whereindex = sql.IndexOf("where", StringComparison.OrdinalIgnoreCase);
            int fromindex = sql.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            string where = sql.Substring(whereindex).Replace("__linq__", "");
            string tableName = sql.Substring(fromindex - 4, whereindex - fromindex - 4);
            int paramindex = objQuery.Parameters.Count;
            foreach (var para in objQuery.Parameters)
            {
                objParams.Add(para.Value);
            }
            Type valueType = typeof(TEntity);
            StringBuilder updateBuilder = new StringBuilder();
            updateBuilder.AppendFormat("{0}=@p{1}", name, paramindex++);
            objParams.Add(value);
            sql = string.Format("UPDATE {0} SET {1} {2}", tableName, updateBuilder.ToString(), where);
            return db.Database.ExecuteSqlCommand(sql, objParams.ToArray());
        }
        #endregion
    }
}