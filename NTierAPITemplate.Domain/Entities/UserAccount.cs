using Microsoft.AspNetCore.Identity;

namespace NTierAPITemplate.Domain.Entities
{
    public class UserAccount : IdentityUser<Guid>
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string ZipCode { get; private set; } = default!;
        public string? ReferralCode { get; private set; }
        public string? ProfileImageUrl { get; private set; }

        private UserAccount() { } // for EF

        public static UserAccount Create(
            string firstName, 
            string lastName, 
            string email,
            string userName, 
            string zipCode,
            string? referralCode = null,
            string? profileImageUrl = null)
        {
            return new UserAccount
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                UserName = userName.Trim().ToLowerInvariant(),
                ZipCode = zipCode.Trim(),
                ReferralCode = referralCode?.Trim(),
                ProfileImageUrl = profileImageUrl
            };
        }
    }
}
