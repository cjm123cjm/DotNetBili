using DotNetBili.Model.Dto;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace DotNetBili.Repository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// SqlsugarClient实体
        /// </summary>
        ISqlSugarClient Db { get; }

        /// <summary>
        /// 根据Id查询实体
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryByIdAsync(object objId);
        Task<TEntity> QueryByIdAsync(object objId, bool blnUseCache = false);
        /// <summary>
        /// 根据id数组查询实体list
        /// </summary>
        /// <param name="lstIds">主键Id数组</param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIDsAsync(object[] lstIds);

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        Task<long> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        Task<List<long>> AddAsync(List<TEntity> listEntity);

        /// <summary>
        /// 更新实体数据/以主键为条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// 批量更新实体数据/以主键为条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<TEntity> entity);
        /// <summary>
        /// 更新实体数据,where条件为字符串,例如 "id=1"
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, string where);
        /// <summary>
        /// sql语句更新数据,例如 "update table set name='bili' where id=1"
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateAsync(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// 匿名对象更新数据/根据主键更新数据,例如 new { id=1,name='bili' }
        /// </summary>
        /// <param name="operateAnonymousObjects">匿名对象</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateAsync(object operateAnonymousObjects);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="lstColumns">要更新的列</param>
        /// <param name="lstIgnoreColumns">要忽略的列</param>
        /// <param name="where">更新条件</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateAsync(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string where = "");
        
        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(TEntity entity);
        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteByIdAsync(object id);
        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(object[] ids);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync();
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string where);
        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression);
        /// <summary>
        /// 按照特定列查询数据列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TResult>> QueryAsync<TResult>(Expression<Func<TEntity, TResult>> expression);
        /// <summary>
        /// 按照特定列查询数据列表带条件排序
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="orderByFields">排序条件</param>
        /// <returns></returns>
        Task<List<TResult>> QueryAsync<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        /// <summary>
        /// 查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        /// <summary>
        /// 查询一个列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string where, string orderByFields);
        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFields);
        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string where, int top, string orderByFields);
        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        Task<List<TEntity>> QuerySqlAsync(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        Task<DataTable> QueryTableAsync(string sql, SugarParameter[] parameters = null);
        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<PageDto<TEntity>> QueryPageAsync(Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize, string orderByFields);
        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string where, int pageIndex, int pageSize, string orderByFields);
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize, string orderByFileds);

        #region 分表
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryByIdSplitAsync(object objId);
        /// <summary>
        /// 自动分表插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<List<long>> AddSplitAsync(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<bool> DeleteSplitAsync(TEntity entity, DateTime dateTime);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<bool> UpdateSplitAsync(TEntity entity, DateTime dateTime);
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
        Task<PageDto<TEntity>> QueryPageSplitAsync(Expression<Func<TEntity, bool>> whereExpression, DateTime beginTime, DateTime endTime, int pageIndex = 1, int pageSize = 20, string orderByFields = null);
        #endregion
    }
}
