using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBili.Common.FFmpeg
{
    public static class FFmpegUtils
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filePath">1.png=>1_thumbnail.jpg</param>
        public static void CreateImageThumbnail(string filePath)
        {
            string CMD = "ffmpeg -i \"%d\" -vf scale=200:-1 \"%d\"";
            CMD = string.Format(CMD, filePath, filePath.Substring(0, filePath.LastIndexOf('.')) + Constants.image_thumbnail);

            ProcessUtils.ExecuteCommand(CMD, true);
        }
    }
}
