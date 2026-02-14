using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Common.Authentication.Models;

public record RefreshToken(string Value, string UserId, DateTime ExpiresAt);