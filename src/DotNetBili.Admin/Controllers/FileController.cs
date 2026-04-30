using DotNetBili.Common;
using DotNetBili.Common.Core;
using DotNetBili.Common.FFmpeg;
using DotNetBili.Common.Option;
using DotNetBili.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBili.Admin.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">要上传的图片文件</param>
        /// <param name="createThumbnail">是否创建缩略图</param>
        [HttpPost]
        public async Task<ResponseDto> UploadImage(
            [FromForm] IFormFile file,
            [FromForm] bool createThumbnail)
        {
            string date = DateTime.Now.ToString("yyyyMM");

            var folder = App.GetOptions<FolderPathOptions>();

            //先上传到临时文件夹
            string path = Path.Combine(folder.PhysicalPath, Constants.file_folder, Constants.file_cover, date);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = file.FileName;
            string fileSunffix = Path.GetExtension(fileName);
            string realFileName = $"{Guid.NewGuid()}{fileSunffix}";
            string realPath = Path.Combine(path, realFileName);
            // 保存原图
            using (var stream = new FileStream(realPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //生成缩略图
            if (createThumbnail)
            {
                FFmpegUtils.CreateImageThumbnail(realPath);
            }
            return new ResponseDto(Constants.file_folder + Constants.file_cover + date + "/" + realFileName);
        }
    }
}
