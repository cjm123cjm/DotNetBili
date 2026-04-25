using System.ComponentModel.DataAnnotations;

namespace DotNetBili.Model.Dto.Account
{
    /// <summary>
    /// 注册输入参数
    /// </summary>
    public class RegisterInput
    {
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        [MaxLength(20)]
        public string NickName { get; set; } = null!;

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
