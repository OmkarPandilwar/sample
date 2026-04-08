using System.Text.Json.Serialization;

namespace CustomerManagement.Application.DTOs;

public record LoginRequest(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password
);

public record LoginResponse(
    string Token,
    string Username,
    string Role,
    DateTime ExpiresAt
);