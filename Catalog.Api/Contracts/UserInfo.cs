using System.Collections.Generic;
using System;

namespace Catalog.Api.Contracts
{
    public enum UserGender
    {
        MALE,
        FEMALE
    }

    public class UserInfo
    {
        public UserGender Gender { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool SubscribedToMailChimp { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public List<string> Roles;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string _Id { get; set; } = string.Empty;
    }
}
