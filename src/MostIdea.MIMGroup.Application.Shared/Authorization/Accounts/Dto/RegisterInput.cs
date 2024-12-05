using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Validation;

namespace MostIdea.MIMGroup.Authorization.Accounts.Dto
{
    public class RegisterInput : IValidatableObject
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [DisableAuditing]
        public string CaptchaResponse { get; set; }


        [Required]
        public UserTypeEnum UserType { get; set; }

        public Guid? HospitalId { get; set; }

        public string Website { get; set; }

        public string TaxAdministration { get; set; }

        public string TaxNumber { get; set; }

        public string DiplomaNo { get; set; }

        /// <summary>
        /// Bayi kaydında firma adı olarak kullanılacak
         /// </summary>
        public string CompanyName { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (!UserName.Equals(EmailAddress, StringComparison.OrdinalIgnoreCase) && ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
                }
            }
        }
    }
}