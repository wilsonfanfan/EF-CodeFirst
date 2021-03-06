﻿//--------------------------------------------------------------------------------------
// <auto-generated>
// QQ:584280962
// T4 生成时间2018/5/25 12:09:54
// </auto-generated>
//--------------------------------------------------------------------------------------
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using X.Constant;
using X.IService;
using X.IDao;
using X.Common;

namespace X.Service
{
    public partial class LogService : AppServiceBase, ILogService
    {
		public ILogDao repository { get; set; } 
		
        public LogService(ILogDao _repository)
        {
            repository = _repository;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public IResponse<Models.Log> Create(Models.Log model)
        {
            var entity = repository.Add(Mapper.Map<Entity.Log>(model), CurrentUserSession.UserId);
            if (entity == null)
            {
                return new IResponse<Models.Log> { Status = false, ErrCode = ErrorMessage.DataNotExist.Code, ErrMsg = ErrorMessage.DataNotExist.Msg };
            }
            return new IResponse<Models.Log> { Status = true, Result = Mapper.Map<Models.Log>(entity) };
        }
        public IListResult<Models.Log> GetList(string Filter = null, string Sort = null, params object[] Values)
        {
            var query = repository.Query();
            if (!Filter.IsNull())
            {
                query = query.Where(Filter, Values);
            }
            if (!Sort.IsNull())
            {
                query = query.SortBy(Sort);
            }
            var list = query.ToList().Select(entity => Mapper.Map<Models.Log>(entity)).ToList();
            var response = new IListResult<Models.Log> { Status = true, Result = list };
            return response;
        }

        public IPageResult<Models.Log> GetPageList(string Filter, string Sort, int PageNumber = 1, int PageSize = 20, params object[] Values)
        {
            var query = repository.Query();
            if (!Filter.IsNull())
            {
                query = query.Where(Filter, Values);
            }
            if (!Sort.IsNull())
            {
                query = query.SortBy(Sort);
            }
            long count = 0;
            var items = query.Page(PageNumber, PageSize, out count).ToList().Select(entity => Mapper.Map<Models.Log>(entity)).ToList();

            var response = new IPageResult<Models.Log>()
            {
                Result = new PagedResult<Models.Log>
                {
                    CurrentPage = PageNumber,
                    TotalItems = count,
                    PageSize = PageSize,
                    Items = items
                }
            };
            return response;
        }

        public IResponse<Models.Log> GetById(long ID)
        {
            var response = new IResponse<Models.Log>
            {
                Status = true,
                Result = Mapper.Map<Models.Log>(repository.GetById(ID))
            };
            return response;
        }
        public IResponse<Models.Log> Get(string Filter, params object[] values)
        {
            var query = repository.Query();
            if (!Filter.IsNull())
            {
                query = query.Where(Filter, values);
            }
            var response = new IResponse<Models.Log>
            {
                Status = true,
                Result = Mapper.Map<Models.Log>(query.First())
            };
            return response;
        }
		
        public IResponse<bool> Update(Models.Log model, params string[] values)
        {
            var response = new IResponse<bool>()
            {
                Status = true
            };
            var entity = Mapper.Map<Entity.Log>(model);
            response.Result = repository.Update(entity, CurrentUserSession.UserId, values);
            return response;
        }
        public IResponse<int> Update(Dictionary<string, string> list, string where, params object[] values)
        {
            var response = new IResponse<int>()
            {
                Status = true
            };
            response.Result = repository.Update(list, where, values);
            return response;
        }
        public IResponse<int> Update(string updateset, string where, params object[] values)
        {
            var response = new IResponse<int>()
            {
                Status = true
            };
            response.Result = repository.Update(updateset, where, values);
            return response;
        }
        public IResponse<int> SetProperty(string field, int val, params long[] ids)
        {
            var response = new IResponse<int>()
            {
                Status = true
            };
            response.Result = repository.Update(field + "=" + val, string.Format("Id in ({0})", string.Join(",", ids)));
            return response;
        }
        public IResponse<bool> Delete(long ID)
        {
            var response = new IResponse<bool>()
            {
                Status = true
            };
            response.Result = repository.DeleteByID(ID, CurrentUserSession.UserId);
            return response;
        }
        public IResponse<bool> DeleteIDs(List<long> IDs)
        {
            var response = new IResponse<bool>()
            {
                Status = true
            };
            response.Result = repository.DeleteByIDs(IDs, CurrentUserSession.UserId);
            return response;
        }
		
        public IResponse<bool> Exists(string Filter, params object[] Values)
        {
            var result = repository.Query().Where(Filter, Values).Count();
            return new IResponse<bool> { Status = true, Result = result > 0 };
        }
        public IResponse<int> Count(string Filter, params object[] Values)
        {
            var result = repository.Query().Where(Filter, Values).Count();
            return new IResponse<int> { Status = true, Result = result };
        }
    }
}
