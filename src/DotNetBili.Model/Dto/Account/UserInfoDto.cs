namespace DotNetBili.Model.Dto.Account
{
    /// <summary>
    /// userDto
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// 性别,0-女,1-男,2-未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 出生日期,格式:yyyy-MM-dd
        /// </summary>
        public string? Birthday { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string? School { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string? PersonIntroduction { get; set; }

        /// <summary>
        /// 空间公告
        /// </summary>
        public string? NoticeInfo { get; set; }

        /// <summary>
        /// 硬币总数量
        /// </summary>
        public int TotalCoinCount { get; set; }

        /// <summary>
        /// 当前硬币数量
        /// </summary>
        public int CurrentCoinCount { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public int Theme { get; set; }
    }
}
