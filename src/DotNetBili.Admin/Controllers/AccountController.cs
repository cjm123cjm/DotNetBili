using DotNetBili.Common;
using DotNetBili.Common.Caches;
using DotNetBili.Common.Captcha;
using DotNetBili.IService.Base;
using DotNetBili.Model.CustomerException;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Dto.Account;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBili.Admin.Controllers
{
    [Route("Account/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICaching _caching;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AccountController(
            ICaching caching,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _caching = caching;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseDto CheckCode()
        {
            var checkcode = CreateCaptcha.CreateVerifyCode(4, VerifyCodeType.ARITH);

            //设置缓存
            _caching.Set(checkcode.CodeKey.ToString(), checkcode.Code, TimeSpan.FromMinutes(5));

            return new ResponseDto
            {
                Code = 200,
                IsSuccess = true,
                Message = "获取验证码成功",
                Data = new
                {
                    CodeKey = checkcode.CodeKey,
                    Image = Convert.ToBase64String(checkcode.Image)
                }
            };
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        public async Task<ResponseDto> Login(LoginInput loginInput)
        {
            try
            {
                //校验验证码
                var code = _caching.Get<string>(loginInput.CheckCodeKey);
                if (!code.Equals(loginInput.CheckCode))
                {
                    throw new BusinessException("验证码错误");
                }

                string userName = AppSettings.app("Admin", "UserName");
                string password = AppSettings.app("Admin", "Password");
                if (loginInput.Email == userName && loginInput.Password == password)
                {
                    string token = _jwtTokenGenerator.GenerateToken(userName);
                    await _caching.SetAsync(CacheConst.tokenAdmin + token, userName, TimeSpan.FromDays(7));

                    return new ResponseDto
                    {
                        Code = 200,
                        IsSuccess = true,
                        Message = "登录成功",
                        Data = new
                        {
                            Token = token,
                            UserName = userName
                        }
                    };
                }
                else
                {
                    throw new BusinessException("用户名或密码错误");
                }
            }
            finally
            {
                //移除redis缓存
                _caching.Remove(loginInput.CheckCodeKey);
            }
        }
    }
}
