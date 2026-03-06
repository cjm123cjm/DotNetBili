using DotNetBili.Common.Core;
using DotNetBili.Common.DB;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Entities;
using DotNetBili.Model.Tenant;
using DotNetBili.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetBili.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarScope _dbBase;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public ISqlSugarClient Db => _db;

        private ISqlSugarClient _db
        {
            get
            {
                ISqlSugarClient db = _dbBase;

                //修改使用 model备注字段作为切换数据库条件，使用sqlsugar TenantAttribute存放数据库ConnId
                //参考 https://www.donet5.com/Home/Doc?typeId=2246
                var tenantAttr = typeof(TEntity).GetCustomAttribute<TenantAttribute>();
                if (tenantAttr != null)
                {
                    //统一处理 configId 小写
                    db = _dbBase.GetConnectionScope(tenantAttr.configId.ToString().ToLower());
                    return db;
                }

                //多租户
                var mta = typeof(TEntity).GetCustomAttribute<MultiTenantAttribute>();
                if (mta is { TypeEnum: TenantTypeEnum.Db })
                {
                    //获取租户信息 租户信息可以提前缓存下来 
                    if (App.User is { TenantId: > 0 })
                    {
                        //.WithCache()
                        var tenant = db.Queryable<SysTenant>().WithCache().Where(s => s.Id == App.User.TenantId && s.TenantType == TenantTypeEnum.Db).First();
                        if (tenant != null)
                        {
                            var iTenant = db.AsTenant();
                            if (!iTenant.IsAnyConnection(tenant.ConfigId))
                            {
                                iTenant.AddConnection(tenant.GetConnectionConfig());
                            }

                            return iTenant.GetConnectionScope(tenant.ConfigId);
                        }
                    }
                }

                return db;
            }
        }
        public BaseRepository(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _dbBase = _unitOfWorkManage.GetDbClient();
        }

        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="objId">主键Id</param>
        /// <returns></returns>
        public async Task<TEntity> QueryByIdAsync(object objId)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(objId);
        }
        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="objId">主键Id</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns></returns>
        public async Task<TEntity> QueryByIdAsync(object objId, bool blnUseCache = false)
        {
            return await _db.Queryable<TEntity>().WithCacheIF(blnUseCache, 10).In(objId).SingleAsync();
        }
        /// <summary>
        /// 根据id数组查询实体list
        /// </summary>
        /// <param name="lstIds">主键Id数组</param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryByIDsAsync(object[] lstIds)
        {
            return await _db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var insert = _db.Insertable(entity);

            return await insert.ExecuteReturnEntityAsync();
        }
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<long> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnSnowflakeIdAsync();
            }
        }
        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<List<long>> AddAsync(List<TEntity> listEntity)
        {
            return await _db.Insertable(listEntity.ToArray()).ExecuteReturnSnowflakeIdListAsync();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            //这种方式会以主键为条件
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<TEntity> entity)
        {
            return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 更新实体数据,where条件为字符串,例如 "id=1"
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity, string where)
        {
            return await _db.Updateable(entity).Where(where).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// sql语句更新数据,例如 "update table set name='bili' where id=1"
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.ExecuteCommandAsync(sql, parameters) > 0;
        }

        /// <summary>
        /// 匿名对象更新数据/根据主键更新数据,例如 new { id=1,name='bili' }
        /// </summary>
        /// <param name="operateAnonymousObjects">匿名对象</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateAsync(object operateAnonymousObjects)
        {
            return await _db.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="lstColumns">要更新的列</param>
        /// <param name="lstIgnoreColumns">要忽略的列</param>
        /// <param name="where">更新条件</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateAsync(
            TEntity entity,
            List<string> lstColumns = null,
            List<string> lstIgnoreColumns = null,
            string where = ""
        )
        {
            IUpdateable<TEntity> up = _db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }

            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }

            if (!string.IsNullOrEmpty(where))
            {
                up = up.Where(where);
            }

            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdsAsync(object[] ids)
        {
            return await _db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string where)
        {
            return await _db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
        }
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }
        /// <summary>
        /// 按照特定列查询数据列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await _db.Queryable<TEntity>().Select(expression).ToListAsync();
        }
        /// <summary>
        /// 按照特定列查询数据列表带条件排序
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="orderByFields">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> QueryAsync<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(whereExpression != null, whereExpression).Select(expression).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields)
        {
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByFields != null, orderByFields).ToListAsync();
        }
        /// <summary>
        /// 查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
            return await _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 查询一个列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string where, string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int top,
            string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(whereExpression != null, whereExpression).Take(top).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            string where,
            int top,
            string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(!string.IsNullOrEmpty(where), where).Take(top).ToListAsync();
        }
        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySqlAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTableAsync(string sql, SugarParameter[] parameters = null)
        {
            return await _db.Ado.GetDataTableAsync(sql, parameters);
        }
        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<PageDto<TEntity>> QueryPageAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex,
            int pageSize,
            string orderByFields)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<TEntity>()
                .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            return new PageDto<TEntity>()
            {
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageSize = pageSize,
                Data = list
            };
        }
        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            string where,
            int pageIndex,
            int pageSize,
            string orderByFields)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(!string.IsNullOrEmpty(where), where).ToPageListAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex,
            int pageSize,
            string orderByFileds)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFileds), orderByFileds)
                .WhereIF(whereExpression != null, whereExpression).ToPageListAsync(pageIndex, pageSize);
        }


        #region 分表
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<TEntity> QueryByIdSplitAsync(object objId)
        {
            return await _db.Queryable<TEntity>().SplitTable().InSingleAsync(objId);
        }
        /// <summary>
        /// 自动分表插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<long>> AddSplitAsync(TEntity entity)
        {
            var insert = _db.Insertable(entity).SplitTable();
            //插入并返回雪花ID并且自动赋值ID　
            return await insert.ExecuteReturnSnowflakeIdListAsync();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSplitAsync(TEntity entity, DateTime dateTime)
        {
            //精准找单个表
            var tableName = _db.SplitHelper<TEntity>().GetTableName(dateTime); //根据时间获取表名
            return await _db.Deleteable<TEntity>().AS(tableName).Where(entity).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSplitAsync(TEntity entity, DateTime dateTime)
        {
            //精准找单个表
            var tableName = _db.SplitHelper<TEntity>().GetTableName(dateTime); //根据时间获取表名
            return await _db.Updateable(entity).AS(tableName).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        public async Task<PageDto<TEntity>> QueryPageSplitAsync(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime, int pageIndex = 1, int pageSize = 20, string orderByFields = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<TEntity>().SplitTable(beginTime, endTime)
                .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var data = new PageDto<TEntity>()
            {
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageSize = pageSize,
                Data = list
            };
            return data;

        }
        #endregion
    }
}
