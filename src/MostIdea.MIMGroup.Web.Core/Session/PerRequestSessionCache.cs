using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNetCore.Http;
using MostIdea.MIMGroup.Sessions;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Web.Session
{
    public class PerRequestSessionCache : IPerRequestSessionCache, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionAppService _sessionAppService;

        public PerRequestSessionCache(
            IHttpContextAccessor httpContextAccessor,
            ISessionAppService sessionAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionAppService = sessionAppService;
        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return await _sessionAppService.GetCurrentLoginInformations();
            }

            var cachedValue = httpContext.Items["__PerRequestSessionCache"] as GetCurrentLoginInformationsOutput;
            if (cachedValue == null)
            {
                cachedValue = await _sessionAppService.GetCurrentLoginInformations();
                httpContext.Items["__PerRequestSessionCache"] = cachedValue;
            }

            return cachedValue;
        }
    }
}