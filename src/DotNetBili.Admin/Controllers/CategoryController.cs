using DotNetBili.IService;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Dto.Category;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBili.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInfoService _categoryInfoService;

        public CategoryController(ICategoryInfoService categoryInfoService)
        {
            _categoryInfoService = categoryInfoService;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        public async Task<ResponseDto> LoadCatogory(long pcategoryId)
        {
            var result = await _categoryInfoService.GetCategoryInfoDtosAsync(pcategoryId);

            return new ResponseDto(result);
        }

        /// <summary>
        /// 添加/修改分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> SaveCatogory(CategoryInfoDto categoryInfoDto)
        {
            await _categoryInfoService.SaveCategoryInfoAsync(categoryInfoDto);

            return new ResponseDto();
        }

        /// <summary>
        /// 删除分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> DeleteCategory(long categoryId)
        {
            var result = await _categoryInfoService.DeleteCategoryInfoAsync(categoryId);

            var response = new ResponseDto();
            response.IsSuccess = result;

            return response;
        }

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="pcategoryId">父级id</param>
        /// <param name="categotyIds">排序后的id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> ChangeSort(
            [FromBody] long pcategoryId,
            [FromBody] string categotyIds)
        {
            await _categoryInfoService.ChangeSortAync(pcategoryId, categotyIds);

            return new ResponseDto();
        }
    }
}
