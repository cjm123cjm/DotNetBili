using DotNetBili.IService.Base;
using DotNetBili.Model.Dto;
using DotNetBili.Repository.Base;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace DotNetBili.Service.Base
{
    public class BaseService<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        private readonly IBaseRepository<TEntity> _baseDal;

        public BaseService(IBaseRepository<TEntity> baseDal)
        {
            _baseDal = baseDal;
        }

        public ISqlSugarClient Db => _baseDal.Db;

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            return await _baseDal.QueryByIdAsync(objId, blnUseCache);
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await _baseDal.QueryByIDsAsync(lstIds);
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<TEntity> Add(TEntity entity)
        {
            return await _baseDal.AddAsync(entity);
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<List<long>> Add(List<TEntity> listEntity)
        {
            return await _baseDal.AddAsync(listEntity);
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await _baseDal.UpdateAsync(entity);
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(List<TEntity> entity)
        {
            return await _baseDal.UpdateAsync(entity);
        }

        public async Task<bool> Update(TEntity entity, string where)
        {
            return await _baseDal.UpdateAsync(entity, where);
        }

        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await _baseDal.UpdateAsync(operateAnonymousObjects);
        }

        public async Task<bool> Update(
            TEntity entity,
            List<string> lstColumns = null,
            List<string> lstIgnoreColumns = null,
            string where = ""
        )
        {
            return await _baseDal.UpdateAsync(entity, lstColumns, lstIgnoreColumns, where);
        }


        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await _baseDal.DeleteAsync(entity);
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await _baseDal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await _baseDal.DeleteByIdsAsync(ids);
        }


        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await _baseDal.QueryAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string where)
        {
            return await _baseDal.QueryAsync(where);
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _baseDal.QueryAsync(whereExpression);
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await _baseDal.QueryAsync(expression);
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表带条件排序
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return await _baseDal.QueryAsync(expression, whereExpression, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await _baseDal.QueryAsync(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return await _baseDal.QueryAsync(whereExpression, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string where, string orderByFileds)
        {
            return await _baseDal.QueryAsync(where, orderByFileds);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
        {
            return await _baseDal.QuerySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null)
        {
            return await _baseDal.QueryTableAsync(sql, parameters);
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFileds)
        {
            return await _baseDal.QueryAsync(whereExpression, top, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string where,
            int top,
            string orderByFileds)
        {
            return await _baseDal.QueryAsync(where, top, orderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex,
            int pageSize,
            string orderByFileds)
        {
            return await _baseDal.QueryAsync(
                whereExpression,
                pageIndex,
                pageSize,
                orderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string where,
            int pageIndex,
            int pageSize,
            string orderByFileds)
        {
            return await _baseDal.QueryAsync(
                where,
                pageIndex,
                pageSize,
                orderByFileds);
        }

        public async Task<PageDto<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex = 1, int pageSize = 20, string orderByFileds = null)
        {
            return await _baseDal.QueryPageAsync(whereExpression,
                pageIndex, pageSize, orderByFileds);
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await _baseDal.QueryByIdAsync(objId);
        }


        #region 分表

        public async Task<List<long>> AddSplit(TEntity entity)
        {
            return await _baseDal.AddSplitAsync(entity);
        }

        public async Task<bool> UpdateSplit(TEntity entity, DateTime dateTime)
        {
            return await _baseDal.UpdateSplitAsync(entity, dateTime);
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteSplit(TEntity entity, DateTime dateTime)
        {
            return await _baseDal.DeleteSplitAsync(entity, dateTime);
        }

        public async Task<TEntity> QueryByIdSplit(object objId)
        {
            return await _baseDal.QueryByIdSplitAsync(objId);
        }

        public async Task<PageDto<TEntity>> QueryPageSplit(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime,
            int pageIndex = 1, int pageSize = 20, string orderByFields = null)
        {
            return await _baseDal.QueryPageSplitAsync(whereExpression, beginTime, endTime,
                pageIndex, pageSize, orderByFields);
        }

        #endregion
    }
}
