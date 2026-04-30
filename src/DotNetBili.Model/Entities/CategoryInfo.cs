using SqlSugar;

namespace DotNetBili.Model.Entities
{
    [SugarTable("category_info")]
    public class CategoryInfo
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "category_id")]
        public long CategoryId { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>

        [SugarColumn(IsPrimaryKey = true, ColumnName = "category_code", ColumnDataType = "varchar(30)")]
        public string CategoryCode { get; set; } = null!;

        /// <summary>
        /// 分类名称
        /// </summary>

        [SugarColumn(IsPrimaryKey = true, ColumnName = "category_name", ColumnDataType = "varchar(30)")]
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// 父级id,0表示一级分类
        /// </summary>
        [SugarColumn(ColumnName = "parent_category_id")]
        public long ParentCategoryId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "icon", ColumnDataType = "varchar(50)")]
        public string? Icon { get; set; }

        /// <summary>
        /// 背景图
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "background", ColumnDataType = "varchar(50)")]
        public string? Background { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnName = "sort")]
        public int Sort { get; set; }
    }
}
