namespace DotNetBili.Common.Option
{
    public class FolderPathOptions
    {
        /// <summary>
        /// 物理路径
        /// </summary>
        public string PhysicalPath { get; set; } = null!;

        /// <summary>
        /// 虚拟目录
        /// </summary>
        public string VirtualPath { get; set; } = null!;
    }
}
