namespace ManageDebts.Application.Contracts.Auth
{
    public sealed record AuthenticationResponse(
      string Token,
      string RefreshToken,
      DateTime ExpirationUtc,
      string Email     
  );
}
