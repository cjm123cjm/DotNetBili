using AutoMapper;
using DotNetBili.Common.Caches;
using DotNetBili.Common.HttpContextUser;
using DotNetBili.IService;
using DotNetBili.IService.Base;
using DotNetBili.Model.CustomerException;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Dto.Account;
using DotNetBili.Model.Entities;
using DotNetBili.Model.Enums;
using DotNetBili.Repository.Base;
using DotNetBili.Service.Base;

namespace DotNetBili.Service
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        private readonly IUser _appUser;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ICaching _caching;

        public UserInfoService(
            IBaseRepository<UserInfo> baseDal,
            IUser appUser,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            ICaching caching) : base(baseDal)
        {
            _appUser = appUser;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _caching = caching;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task RegisterAsync(RegisterInput registerInput)
        {
            var emailUser = await Db.Queryable<UserInfo>().FirstAsync(t => t.Email == registerInput.Email);
            if (emailUser != null)
            {
                throw new BusinessException("邮箱已存在");
            }
            var nickNameUser = await Db.Queryable<UserInfo>().FirstAsync(t => t.NickName == registerInput.NickName);
            if (nickNameUser != null)
            {
                throw new BusinessException("昵称已存在");
            }
            UserInfo userInfo = new UserInfo
            {
                Email = registerInput.Email,
                NickName = registerInput.NickName,
                Password = registerInput.Password,
                Sex = (int)UserStatusEnum.ENABLE,
                JoinTime = DateTime.Now,
                Status = (int)UserSexEnum.SECRECY,
                Theme = 1
            };

            //todo：系统设置 => 初始化用户硬币 

            await Add(userInfo);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<LoginResponseDto> LoginAsync(LoginInput loginInput)
        {
            var userInfo = await Db.Queryable<UserInfo>().FirstAsync(t => t.Email == loginInput.Email && t.Password == loginInput.Password);
            if (userInfo == null)
            {
                throw new BusinessException("账户密码错误");
            }
            if (userInfo.Status == (int)UserStatusEnum.DISENABLE)
            {
                throw new BusinessException("账户已被禁用");
            }

            userInfo.LastLoginTime = DateTime.Now;
            userInfo.LastLoginIp = _appUser.GetClientIpv4();

            await Update(userInfo);

            //生成jwt
            var token = _jwtTokenGenerator.GenerateToken(userInfo);

            var userInfoDto = _mapper.Map<UserInfoDto>(userInfo);

            //todo:设置 粉丝数、关注数、硬币数

            //保存到缓存，7天过期
            await _caching.SetAsync(CacheConst.tokenWeb+token, userInfoDto, TimeSpan.FromDays(7));

            return new LoginResponseDto
            {
                Token = token,
                UserDto = userInfoDto
            };
        }
    }
}
