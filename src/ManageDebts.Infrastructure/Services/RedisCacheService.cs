using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Debts;

public sealed class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private static readonly DistributedCacheEntryOptions DefaultOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };

    public RedisCacheService(IDistributedCache cache) => _cache = cache;

    public Task SetDebtDetailAsync(string debtId, DebtDto debt, CancellationToken ct = default)
    {
        var key = Key(debtId);
        var data = JsonSerializer.Serialize(debt);
        return _cache.SetStringAsync(key, data, DefaultOptions, ct);
    }

    public async Task<DebtDto?> GetDebtDetailAsync(string debtId, CancellationToken ct = default)
    {
        var data = await _cache.GetStringAsync(Key(debtId), ct);
        return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<DebtDto>(data);
    }

    public Task RemoveDebtDetailAsync(string debtId, CancellationToken ct = default)
        => _cache.RemoveAsync(Key(debtId), ct);

    private static string Key(string debtId) => $"debt:{debtId}";
}
