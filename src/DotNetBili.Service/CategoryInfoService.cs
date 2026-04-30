using AutoMapper;
using DotNetBili.Common.Caches;
using DotNetBili.Common.Core;
using DotNetBili.IService;
using DotNetBili.Model.CustomerException;
using DotNetBili.Model.Dto.Category;
using DotNetBili.Model.Entities;
using DotNetBili.Repository.Base;
using DotNetBili.Service.Base;

namespace DotNetBili.Service
{
    public class CategoryInfoService : BaseService<CategoryInfo>, ICategoryInfoService
    {
        private readonly IMapper _mapper;
        private readonly ICaching _caching;
        public CategoryInfoService(
            IBaseRepository<CategoryInfo> baseDal,
            IMapper mapper,
            ICaching caching) : base(baseDal)
        {
            _mapper = mapper;
            _caching = caching;
        }

        /// <summary>
        /// 获取所有分类信息的列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryInfoDto>> GetCategoryInfoDtosAsync()
        {
            var categoryInfos = await Db.Queryable<CategoryInfo>().OrderBy(t => t.Sort).ToListAsync();

            var categoryInfoDos = _mapper.Map<List<CategoryInfoDto>>(categoryInfos);

            List<CategoryInfoDto> result = new List<CategoryInfoDto>();
            ConvertLineToTree(categoryInfoDos, 0, result);

            return result;
        }

        //0 0 34 35
        private void ConvertLineToTree(List<CategoryInfoDto> categoryInfos, long parentCategoryId, List<CategoryInfoDto> result)
        {
            var children = categoryInfos.Where(t => t.ParentCategoryId == parentCategoryId)
                                .OrderBy(t => t.Sort)
                                .ToList();

            foreach (var child in children)
            {
                child.Children = new List<CategoryInfoDto>();
                ConvertLineToTree(categoryInfos, child.CategoryId, child.Children);
                result.Add(child);
            }
        }

        /// <summary>
        /// 添加/修改分类信息
        /// </summary>
        /// <param name="categoryInfoDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task SaveCategoryInfoAsync(CategoryInfoDto categoryInfoDto)
        {
            //判断分类编号是否存在
            int count = await Db.Queryable<CategoryInfo>().Where(t => t.CategoryCode == categoryInfoDto.CategoryCode && t.CategoryId != categoryInfoDto.CategoryId).CountAsync();
            if (count > 0)
            {
                throw new BusinessException("分类编号已存在");
            }
            //获取最大的序号
            int maxSort = await Db.Queryable<CategoryInfo>().Where(t => t.ParentCategoryId == categoryInfoDto.ParentCategoryId).MaxAsync(t => t.Sort);

            categoryInfoDto.Sort = maxSort + 1;

            if (categoryInfoDto.CategoryId == 0)
            {
                var categoryInfo = _mapper.Map<CategoryInfo>(categoryInfoDto);

                await Add(categoryInfo);
            }
            else
            {
                var categoryInfo = await QueryById(categoryInfoDto.CategoryId);

                _mapper.Map(categoryInfoDto, categoryInfo);

                await Update(categoryInfo);
            }
        }


        /// <summary>
        /// 删除分类信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteCategoryInfoAsync(long categoryId)
        {
            //将分类下的子分类都查询出来
            var categorys = await Db.Queryable<CategoryInfo>().Where(t => t.ParentCategoryId == categoryId || t.CategoryId == categoryId).ToListAsync();

            //todo：查询分类下是否有视频，如果有则不允许删除

            var result = await DeleteByIds(categorys.Select(t => t.CategoryId as object).ToArray());
            if (!result)
            {
                throw new BusinessException("删除分类失败");
            }

            //刷新缓存
            await SaveToRedis();
        }

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="pcategoryId"></param>
        /// <param name="categotyIds"></param>
        /// <returns></returns>
        public async Task ChangeSortAync(long pcategoryId, string categotyIds)
        {
            var categorys = await Db.Queryable<CategoryInfo>().Where(t => t.ParentCategoryId == pcategoryId).ToListAsync();

            var categoryList = categotyIds.Split(",").ToList();
            for (int i = 0; i < categoryList.Count; i++)
            {
                var first = categorys.FirstOrDefault(t => t.CategoryId.ToString() == categoryList[i]);
                if (first != null)
                {
                    first.Sort = i + 1;
                }
            }

            await Update(categorys);

            //刷新缓存
            await SaveToRedis();
        }

        /// <summary>
        /// 刷新分类缓存
        /// </summary>
        /// <returns></returns>
        private async Task SaveToRedis()
        {
            var categoryRedis = await GetCategoryInfoDtosAsync();
            await _caching.SetAsync(CacheConst.category, categoryRedis);
        }
    }
}
