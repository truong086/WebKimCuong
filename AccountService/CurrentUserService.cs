using Payment.Applications.Interface;
using System.Security.Claims;

namespace thuongmaidientus1.AccountService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string? UserId => httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
    }
}
