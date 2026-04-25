using SqlSugar;

namespace DotNetBili.Model.Entities
{
    [SugarTable("user_info")]
    [SugarIndex("unique_userinfo_nickname", nameof(UserInfo.NickName), OrderByType.Desc, true)]
    [SugarIndex("unique_userinfo_email", nameof(UserInfo.Email), OrderByType.Desc, true)]
    public class UserInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,ColumnName ="user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnName ="nick_name")]
        public string NickName { get; set; } = null!;

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 性别,0-女,1-男,2-未知
        /// </summary>
        [SugarColumn(ColumnName = "sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 出生日期,格式:yyyy-MM-dd
        /// </summary>
        [SugarColumn(ColumnName = "birthday")]
        public string? Birthday { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        [SugarColumn(ColumnName = "school")]
        public string? School { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [SugarColumn(ColumnName = "person_introduction")]
        public string? PersonIntroduction { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        [SugarColumn(ColumnName = "join_time")]
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [SugarColumn(ColumnName = "last_login_time")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录ip
        /// </summary>
        [SugarColumn(ColumnName = "last_login_ip")]
        public string? LastLoginIp { get; set; }

        /// <summary>
        /// 状态,1-正常,0-禁用
        /// </summary>
        [SugarColumn(ColumnName = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 空间公告
        /// </summary>
        [SugarColumn(ColumnName = "notice_info")]
        public string? NoticeInfo { get; set; }

        /// <summary>
        /// 硬币总数量
        /// </summary>
        [SugarColumn(ColumnName = "total_coin_count")]
        public int TotalCoinCount { get; set; }

        /// <summary>
        /// 当前硬币数量
        /// </summary>
        [SugarColumn(ColumnName = "current_coin_count")]
        public int CurrentCoinCount { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [SugarColumn(ColumnName = "theme")]
        public int Theme { get; set; }
    }
}
