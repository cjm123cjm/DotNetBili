using DotNetBili.Common;
using DotNetBili.Common.Option;
using DotNetBili.IService;
using DotNetBili.Model.Dto;
using DotNetBili.Model.Entities;
using DotNetBili.Repository.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DotNetBili.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public ValuesController(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using var uow = _unitOfWorkManage.CreateUnitOfWork();

                //....业务代码

                uow.Commit();
            }
            catch (Exception)
            {
            }
            return Ok();
        }
    }
}
