using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perevorot.Domain.Models.DomainEntities
{
    [Table("Users")]
    public class User : AbstractUser
    {
        protected User()
        {
        }
        public User(string userName)
            : base(userName)
        {
            IsActive = true;
            LastLogin = DateTime.Now;
            LastActivityDate = DateTime.Now;
            UserRoles = new Collection<UserRole>();
        }

        public bool IsActive { get; set; }

        //navigation
        public virtual ICollection<UserRole> UserRoles { get; private set; }

        public DateTime LastLogin { get; set; }

        public UserRole CurrentRole { get; set; }

        public string LoweredUserName { get; set; }

        public string MobileAlias { get; set; }

        public bool IsAnonymous { get; set; }

        public DateTime LastActivityDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Application Application { get; set; }
    }

    [Table("Applications")]
    public class Application : PerevorotEntity
    {
        public string ApplicationName { get; set; }
        public string LoweredApplicationName { get; set; }
        public string Description { get; set; }
    }


    [Table("Membership")]
    public class Member : PerevorotEntity
    {
        public virtual Application Application { get; set; }
        public virtual User User { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public string MobilePIN { get; set; }
        public string Email { get; set; }
        public string LoweredEmail { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }
    }

    [Table("Profile")]
    public class Profile : PerevorotEntity
    {
        public User User { get; set; }
        public string PropertyNames { get; set; }
        public string PropertyValuesString { get; set; }
        public byte[] PropertyValuesBinary { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}