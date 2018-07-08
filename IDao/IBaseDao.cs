using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace X.IDao
{
    /// <summary>
    /// 接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseDao<TEntity> where TEntity : Entity.BaseEntity, new()
    {
        #region 增
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity, long operatorID = 0, bool isSave = true);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        int InsertAll(List<TEntity> entities, long operatorID = 0);
        #endregion

        #region 软删
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        bool Delete(TEntity entity, long operatorID = 0);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> predicate, long operatorID = 0);
        /// <summary>
        /// ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteByID(long id, long operatorID = 0);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        bool DeleteByIDs(List<long> ids, long operatorID = 0);
        #endregion

        #region 永久删除
        /// <summary>
        /// 永久删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Remove(TEntity entity);
        /// <summary>
        /// 永久删除
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        int Remove(Expression<Func<TEntity, bool>> filterExpression);
        #endregion

        #region 改
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        bool Update(TEntity entity, long operatorID = 0, bool isSave = true);
        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        bool Update(TEntity entity, long operatorID = 0, params string[] properties);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="propertie"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, bool>> propertie, Expression<Func<TEntity>> updateExpression);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <param name="where"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        int Update(Dictionary<string, string> list, string where, params object[] values);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="updateset"></param>
        /// <param name="where"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        int Update(string updateset, string where, params object[] values);
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        TEntity Save(TEntity entity, long operatorID = 0);
        #endregion

        #region 查
        #region TEntity
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);
        /// <summary>
        /// 查询ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        TEntity GetById(long id, bool includeDeleted = false);
        /// <summary>
        /// 返回单个
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity Find(params object[] keys);

        #endregion

        #region IQueryable
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(bool includeDeleted = false);
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);
        #endregion

        #endregion

        #region 事务

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTransaction();
        #endregion

        #region 执行SQL

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <returns></returns>
        int Execute(string sql, params object[] parameters);
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
