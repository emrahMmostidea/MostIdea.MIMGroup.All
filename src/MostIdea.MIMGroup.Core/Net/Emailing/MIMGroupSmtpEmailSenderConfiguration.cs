using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace MostIdea.MIMGroup.Net.Emailing
{
    public class MIMGroupSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public MIMGroupSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}