using System.ComponentModel.DataAnnotations;

namespace DotNetBili.Model.Dto.Account
{
    /// <summary>
    /// 登录输入参数
    /// </summary>
    public class LoginInput
    {
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        /// <summary>
        /// 验证码key
        /// </summary>
        public string CheckCodeKey { get; set; } = null!;
        /// <summary>
        /// 验证码
        /// </summary>
        public string CheckCode { get; set; } = null!;
    }
}
