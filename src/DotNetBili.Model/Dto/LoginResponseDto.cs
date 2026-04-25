namespace DotNetBili.Model.Dto
{
    /// <summary>
    /// 登录返回
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = null!;
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto UserDto { get; set; } = null!;
    }
}
