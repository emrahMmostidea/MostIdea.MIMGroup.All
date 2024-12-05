using System;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Localization;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Castle.Windsor;
using Microsoft.AspNetCore.SignalR;
using MostIdea.MIMGroup.Chat;

namespace MostIdea.MIMGroup.Web.Chat.SignalR
{
    public class ChatHub : OnlineClientHubBase
    {
        private readonly IChatMessageManager _chatMessageManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IWindsorContainer _windsorContainer;
        private bool _isCallByRelease;
        private IAbpSession ChatAbpSession { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        public ChatHub(
            IChatMessageManager chatMessageManager,
            ILocalizationManager localizationManager,
            IWindsorContainer windsorContainer,
            IOnlineClientManager<ChatChannel> onlineClientManager,
            IOnlineClientInfoProvider clientInfoProvider) : base(onlineClientManager, clientInfoProvider)
        {
            _chatMessageManager = chatMessageManager;
            _localizationManager = localizationManager;
            _windsorContainer = windsorContainer;

            Logger = NullLogger.Instance;
            ChatAbpSession = NullAbpSession.Instance;
        }

        public async Task<string> SendMessage(SendChatMessageInput input)
        {
            var sender = Context.ToUserIdentifier();
            var receiver = new UserIdentifier(input.TenantId, input.UserId);

            try
            {
                using (ChatAbpSession.Use(Context.GetTenantId(), Context.GetUserId()))
                {
                    await _chatMessageManager.SendMessageAsync(sender, receiver, input.Message, input.TenancyName, input.UserName, input.ProfilePictureId);
                    return string.Empty;
                }
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return _localizationManager.GetSource("AbpWeb").GetString("InternalServerError");
            }
        }
        
        
         
        public async Task<string> SendQuickOrder(string message, long senderId, long userId, string userName)
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"].ToString();
            Logger.Warn("ACCESS TOKEN : " + accessToken);
            return await SendMessage(new SendChatMessageInput
            {
                Message = message,
                SenderId = senderId,
                UserId = userId,
                UserName = userName,
                ProfilePictureId = null,
                TenancyName = null,
                TenantId = null
            });
        }
        
        
         

        public void Register()
        {
            Logger.Debug("A client is registered: " + Context.ConnectionId);
        }

        protected override void Dispose(bool disposing)
        {
            if (_isCallByRelease)
            {
                return;
            }
            base.Dispose(disposing);
            if (disposing)
            {
                _isCallByRelease = true;
                _windsorContainer.Release(this);
            }
        }
    }
}