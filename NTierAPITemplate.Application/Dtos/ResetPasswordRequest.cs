namespace NTierAPITemplate.Application.Dtos
{
    public record ResetPasswordRequest(
        string Email,
        string Token,
        string NewPassword,
        string ConfirmPassword
    );
}
