using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using X.IDao;
using EntityFramework.Extensions;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace X.Dao
{
    /// <summary>
    /// 基础
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseDao<TEntity> : IBaseDao<TEntity> where TEntity : Entity.BaseEntity, new()
    {
        #region 属性
        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime UtcDateTime
        {
            get
            {
                return DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            }
        }
        /// <summary>
        /// 数据对象
        /// </summary>
        public Database Db
        {
            get
            {
                return Instance.Database;
            }
        }
        private readonly DbSet<TEntity> _dbSet;
        private DbContext _instance;
        /// <summary>
        /// 数据对象
        /// </summary>
        public DbContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DbSession.GetCurrentDbContext();
                }
                return _instance;
            }
        }
        public DbSet<TEntity> Entities { get { return _dbSet; } }
        /// <summary>
        /// 基础
        /// </summary>
        public BaseDao(DbContext context)
        {
            _instance = context;
            _dbSet = _instance.Set<TEntity>();
        }
        /// <summary>
        /// 基础
        /// </summary>
        public BaseDao()
        {
            _instance = DbSession.GetCurrentDbContext();
            _dbSet = _instance.Set<TEntity>();
        }
        #endregion

        #region 增
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity, long operatorID = 0, bool isSave = true)
        {
            entity.CreatedTime = UtcDateTime;
            entity.CreaterID = operatorID;
            var m = this._dbSet.Add(entity);
            if (isSave && SaveChanges() == 0)
            {
                return null;
            }
            return m;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public virtual int InsertAll(List<TEntity> entities, long operatorID = 0)
        {
            entities.ForEach(entity =>
            {
                entity.CreatedTime = UtcDateTime;
                entity.CreaterID = operatorID;
            });
            var list = this._dbSet.AddRange(entities);
            return SaveChanges();
        }
        #endregion

        #region 软删
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public virtual bool Delete(TEntity entity, long operatorID = 0)
        {
            entity.IsDeleted = true;
            entity.DeletedTime = UtcDateTime;
            entity.DeleterID = operatorID;
            return Update(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate, long operatorID = 0)
        {
            return Update(predicate, () => new TEntity { IsDeleted = true, DeletedTime = UtcDateTime, DeleterID = operatorID });
        }
        /// <summary>
        /// Id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool DeleteByID(long id, long operatorID = 0)
        {
            return Update(t => t.Id == id, () => new TEntity { IsDeleted = true, DeletedTime = UtcDateTime, DeleterID = operatorID }) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public virtual bool DeleteByIDs(List<long> ids, long operatorID = 0)
        {
            return Update(t => ids.Contains(t.Id), () => new TEntity { IsDeleted = true, DeletedTime = UtcDateTime, DeleterID = operatorID }) > 0;
        }
        #endregion

        #region 永久删除
        /// <summary>
        /// 永久删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Remove(TEntity entity)
        {
            this._dbSet.Remove(entity);
            return SaveChanges() > 0;
        }
        /// <summary>
        /// 永久删除
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public virtual int Remove(Expression<Func<TEntity, bool>> filterExpression)
        {
            this._dbSet.RemoveRange(this._dbSet.Where(filterExpression));
            return SaveChanges();
        }
        #endregion

        #region 改
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity entity, long operatorID = 0, bool isSave = true)
        {
            var entry = this.Instance.Entry(entity);
            entity.ModifiedTime = UtcDateTime;
            entity.ModifierID = operatorID;
            entry.State = EntityState.Modified;
            return isSave ? SaveChanges() > 0 : true;
        }
        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity entity, long operatorID = 0, params string[] properties)
        {
            var local = this._dbSet.Local.FirstOrDefault(f => f.Id == entity.Id);
            var entry = this.Instance.Entry(entity);
            if (local != null)
            {
                this.Instance.Entry(local).State = EntityState.Detached;
            }
            if (entry.State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }
            entity.ModifiedTime = UtcDateTime;
            entity.ModifierID = operatorID;
            if (properties != null && properties.Length > 0)
            {
                foreach (var propertyName in properties)
                {
                    entry.Property(propertyName).IsModified = true;
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }
            return SaveChanges() > 0;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="propertie"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TEntity, bool>> propertie, Expression<Func<TEntity>> updateExpression)
        {
            return this.Instance.UpdateBatch<TEntity>(propertie, updateExpression);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Update(Dictionary<string, string> list, string where, params object[] values)
        {
            if (list == null || list.Count == 0)
            {
                return 0;
            }
            var T = typeof(TEntity);
            int i = 0;
            StringBuilder Sql = new StringBuilder();
            Sql.AppendFormat("UPDATE [{0}]", T.Name);
            Sql.Append(" SET ");
            List<object> parms = new List<object>();
            foreach (var k in list)
            {
                if (!string.IsNullOrEmpty(k.Value))
                {
                    Sql.AppendFormat(" [{0}]=@p{1},", k.Key, i);
                    parms.Add(k.Value);
                    i++;
                }
            }
            Sql = Sql.Remove(Sql.Length - 1, 1);
            if (where != null)
            {
                Sql.Append(" WHERE ");
                Sql.Append(where);
                parms.AddRange(values);
            }
            return Execute(Sql.ToString(), parms);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="updateset"></param>
        /// <param name="where"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual int Update(string updateset, string where, params object[] values)
        {
            if (string.IsNullOrEmpty(updateset))
            {
                return 0;
            }
            var T = typeof(TEntity);
            StringBuilder Sql = new StringBuilder();
            Sql.AppendFormat("UPDATE [{0}]", T.Name);
            Sql.Append(" SET ");
            Sql.Append(updateset);
            if (where != null)
            {
                Sql.Append(" WHERE ");
                Sql.Append(where);
            }
            return Execute(Sql.ToString(), values);
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public virtual TEntity Save(TEntity entity, long operatorID = 0)
        {
            if (entity.Id <= 0)
            {
                var t = Add(entity, operatorID);
                return t;
            }
            if (Update(entity, operatorID))
            {
                return entity;
            }
            return null;
        }
        #endregion

        #region 查
        #region TEntity
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false)
        {
            var query = this._dbSet.Where(predicate);
            if (!includeDeleted)
            {
                query = query.Where(t => t.IsDeleted == false);
            }
            return query.AsNoTracking().Any();
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="countLamdba">查询表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<TEntity, bool>> countLamdba)
        {
            return this._dbSet.AsNoTracking().Count(countLamdba);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual TEntity GetById(long id, bool includeDeleted = false)
        {
            return Single(entity => entity.Id == id, includeDeleted);
        }
        /// <summary>
        /// 返回单个
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false)
        {
            try
            {
                return this._dbSet.Where(entity => (includeDeleted || !entity.IsDeleted)).AsNoTracking().Single(predicate);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual TEntity Find(params object[] keys)
        {
            try
            {
                return this._dbSet.Find(keys);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region IQueryable
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(bool includeDeleted = false)
        {
            var query = this._dbSet.AsQueryable().AsNoTracking();
            if (!includeDeleted)
            {
                query = query.Where(entity => entity.IsDeleted == false);
            }
            return query;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false)
        {
            var query = this._dbSet.Where(predicate).AsQueryable().AsNoTracking();
            if (!includeDeleted)
            {
                query = query.Where(entity => entity.IsDeleted == false);
            }
            return query;
        }
        #endregion

        #endregion

        #region 执行SQL

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <returns></returns>
        public virtual int Execute(string sql, params object[] parameters)
        {
            return this.Instance.Database.ExecuteSqlCommand(sql, parameters);
        }
        #endregion

        #region 事务

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public virtual DbTransaction BeginTransaction()
        {
            if (this.Instance.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Instance.Database.Connection.Open();
            }
            return this.Instance.Database.Connection.BeginTransaction();
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            try
            {
                return this.Instance.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = new StringBuilder();
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validateionError in validationErrors.ValidationErrors)
                    {
                        msg.AppendFormat("属性:{0} 错误:{1}", validateionError.PropertyName, validateionError.ErrorMessage);
                    }
                }
                logger.Write("DbEntityValidationException", msg.ToString());
                throw new Exception(msg.ToString(), dbEx);
            }
            catch (DbException dbEx)
            {
                logger.Write("DefaultConnection", "", dbEx);
                throw new Exception("DefaultConnection", dbEx);
            }
            catch (DbUpdateException dbEx)
            {
                logger.Write("DbUpdateException", dbEx.Message);
                throw new Exception("DbUpdateException", dbEx);
            }
            catch (Exception ex)
            {
                logger.Write("Exception", ex.Message);
                throw new Exception("Exception", ex);
            }
        }
        #endregion
    }
}
