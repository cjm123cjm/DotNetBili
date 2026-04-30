namespace DotNetBili.Model.Dto.Category
{
    public class CategoryInfoDto
    {
        public long CategoryId { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode { get; set; } = null!;

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// 父级id,0表示一级分类
        /// </summary>
        public long ParentCategoryId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 背景图
        /// </summary>
        public string? Background { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public List<CategoryInfoDto> Children { get; set; } = new();
    }
}
