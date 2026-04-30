using DotNetBili.IService.Base;
using DotNetBili.Model.Dto.Category;
using DotNetBili.Model.Entities;

namespace DotNetBili.IService
{
    public interface ICategoryInfoService : IBaseServices<CategoryInfo>
    {
        Task<List<CategoryInfoDto>> GetCategoryInfoDtosAsync();

        Task SaveCategoryInfoAsync(CategoryInfoDto categoryInfoDto);

        Task DeleteCategoryInfoAsync(long categoryId);

        Task ChangeSortAync(long pcategoryId, string categotyIds);
    }
}
