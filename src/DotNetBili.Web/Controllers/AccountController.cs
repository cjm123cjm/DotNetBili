using DotNetBili.Common.Caches;
using DotNetBili.Common.Captcha;
using DotNetBili.IService;
using DotNetBili.Model.CustomerException;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Dto.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBili.Web.Controllers
{
    [Route("account/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICaching _caching;
        private readonly IUserInfoService _userInfoService;
        public AccountController(
            ICaching caching,
            IUserInfoService userInfoService)
        {
            _caching = caching;
            _userInfoService = userInfoService;
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
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        public async Task<ResponseDto> Register(RegisterInput registerInput)
        {
            try
            {
                //校验验证码
                var code = _caching.Get<string>(registerInput.CheckCodeKey);
                if (!code.Equals(registerInput.CheckCode))
                {
                    throw new BusinessException("验证码错误");
                }

                await _userInfoService.RegisterAsync(registerInput);

                return new ResponseDto();
            }
            finally
            {
                //移除redis缓存
                _caching.Remove(registerInput.CheckCodeKey);
            }
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

                var loginResponseDto = await _userInfoService.LoginAsync(loginInput);

                return new ResponseDto(loginResponseDto);
            }
            finally
            {
                //移除redis缓存
                _caching.Remove(loginInput.CheckCodeKey);
            }
        }
    }
}
