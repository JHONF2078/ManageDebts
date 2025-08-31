using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using ManageDebts.Application.Common.Interfaces;
using System.Collections.Generic;
using ManageDebts.Application.Debts;

namespace ManageDebts.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
                

        public async Task SetDebtDetailAsync(string debtId, DebtDto debt)
        {
            var key = $"debt:{debtId}";
            var data = JsonSerializer.Serialize(debt);
            var options = new DistributedCacheEntryOptions
            {
                //la cache de la deuda se guarda por 1 dia
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), 
                //si nadie lo consulta durante un  5 minutos,  se borrra
                //si alguien consulta antes de los 5 minutos,
                //renueva por otros 5 minutos  a partir del ultimo acceso
                SlidingExpiration = TimeSpan.FromMinutes(5) 
            };
            await _cache.SetStringAsync(key, data, options);
        }

        public async Task<DebtDto?> GetDebtDetailAsync(string debtId)
        {
            var key = $"debt:{debtId}";
            var data = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(data)) return null;
            return JsonSerializer.Deserialize<DebtDto>(data);
        }
    }
}
