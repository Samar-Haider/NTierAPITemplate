namespace NTierAPITemplate.Application.Dtos
{
    public record RegisterRequest(
           string FirstName,
           string LastName,
           string Email,
           string UserName,
           string Password,
           string ConfirmPassword,
           string PhoneNumber,
           string ZipCode,
           string? ReferralCode
       );
}
