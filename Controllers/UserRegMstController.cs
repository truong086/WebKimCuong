using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegMstController : ControllerBase
    {
        private readonly IAccountService _account;
        public UserRegMstController(IAccountService account)
        {
            _account = account;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(AddAccount))]
        public async Task<IActionResult> AddAccount(AccountDTO accountDTO)
        {
            var check = await _account.AddAccount(accountDTO);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(MaHoa))]
        public async Task<IActionResult> MaHoa(string value)
        {
            var check = await _account.ConvertToHS256(value);
            return Ok(check);
        }

        [HttpGet]
        [Route(nameof(GiaiMaHoa))]
        public async Task<IActionResult> GiaiMaHoa(string value)
        {
            var check = await _account.DecryptHS256(value);
            return Ok(check);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(Login login)
        {
            var check = await _account.Login(login);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(Action))]
        public async Task<IActionResult> Action(string code)
        {
            var check = await _account.Action(code);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(Logout))]
        public async Task<IActionResult> Logout(string name)
        {
            var check = await _account.Logout(name);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route(nameof(EditAccount))]
        public async Task<IActionResult> EditAccount(int id, AccountDTO accountDTO)
        {
            var check = await _account.EditAccount(id, accountDTO);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(CheckToken))]
        public async Task<IActionResult> CheckToken([FromQuery] string token)
        {
            var check = await _account.checkToken(token);
            if (check != null)
            {
                return Ok(check);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 1)
        {
            return await _account.FindAll(name, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(FindOneListIdOrName))]
        public async Task<PayLoad<List<AccountDTO>>> FindOneListIdOrName([FromQuery]IList<string> id)
        {
            return await _account.FindOne(id);
        }

        [HttpGet]
        [Route(nameof(FindAllRole))]
        public async Task<PayLoad<object>> FindAllRole()
        {
            return await _account.findAllRole();
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route(nameof(DeleteAccount))]
        public async Task<string> DeleteAccount([FromQuery]IList<string> id)
        {
            return await _account.DeleteAccount(id);
        }
    }
}
