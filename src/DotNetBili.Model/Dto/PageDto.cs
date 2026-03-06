namespace DotNetBili.Model.Dto
{
    public class PageDto<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage => (int)Math.Ceiling((decimal)TotalCount / PageSize);
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 20;
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Data { get; set; }
    }
}
