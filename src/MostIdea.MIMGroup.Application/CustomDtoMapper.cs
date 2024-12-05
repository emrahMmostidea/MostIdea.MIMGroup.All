using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.B2B;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using MostIdea.MIMGroup.Auditing.Dto;
using MostIdea.MIMGroup.Authorization.Accounts.Dto;
using MostIdea.MIMGroup.Authorization.Delegation;
using MostIdea.MIMGroup.Authorization.Permissions.Dto;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Roles.Dto;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users.Delegation.Dto;
using MostIdea.MIMGroup.Authorization.Users.Dto;
using MostIdea.MIMGroup.Authorization.Users.Importing.Dto;
using MostIdea.MIMGroup.Authorization.Users.Profile.Dto;
using MostIdea.MIMGroup.Chat;
using MostIdea.MIMGroup.Chat.Dto;
using MostIdea.MIMGroup.DynamicEntityProperties.Dto;
using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.Friendships;
using MostIdea.MIMGroup.Friendships.Cache;
using MostIdea.MIMGroup.Friendships.Dto;
using MostIdea.MIMGroup.Localization.Dto;
using MostIdea.MIMGroup.MultiTenancy;
using MostIdea.MIMGroup.MultiTenancy.Dto;
using MostIdea.MIMGroup.MultiTenancy.HostDashboard.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments;
using MostIdea.MIMGroup.MultiTenancy.Payments.Dto;
using MostIdea.MIMGroup.Notifications.Dto;
using MostIdea.MIMGroup.Organizations.Dto;
using MostIdea.MIMGroup.Sessions.Dto;
using MostIdea.MIMGroup.WebHooks.Dto;

namespace MostIdea.MIMGroup
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditSalesConsultantDto, SalesConsultant>().ReverseMap();
            configuration.CreateMap<SalesConsultantDto, SalesConsultant>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssistanceVsUserDto, AssistanceVsUser>().ReverseMap();
            configuration.CreateMap<AssistanceVsUserDto, AssistanceVsUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditDynamicEnumItemDto, DynamicEnumItem>().ReverseMap();
            configuration.CreateMap<DynamicEnumItemDto, DynamicEnumItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditDynamicEnumDto, DynamicEnum>().ReverseMap();
            configuration.CreateMap<DynamicEnumDto, DynamicEnum>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderCommentDto, OrderComment>().ReverseMap();
            configuration.CreateMap<OrderCommentDto, OrderComment>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderItemDto, OrderItem>().ReverseMap();
            configuration.CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditWarehouseVsCourierDto, WarehouseVsCourier>().ReverseMap();
            configuration.CreateMap<WarehouseVsCourierDto, WarehouseVsCourier>().ReverseMap();
            configuration.CreateMap<CreateOrEditWarehouseDto, Warehouse>().ReverseMap();
            configuration.CreateMap<WarehouseDto, Warehouse>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderDto, Order>().ReverseMap();
            configuration.CreateMap<OrderDto, Order>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductPricesForHospitalDto, ProductPricesForHospital>().ReverseMap();
            configuration.CreateMap<ProductPricesForHospitalDto, ProductPricesForHospital>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditBrandDto, Brand>().ReverseMap();
            configuration.CreateMap<BrandDto, Brand>().ReverseMap();
            configuration.CreateMap<CreateOrEditAddressInformationDto, AddressInformation>().ReverseMap();
            configuration.CreateMap<AddressInformationDto, AddressInformation>().ReverseMap();
            configuration.CreateMap<CreateOrEditHospitalVsUserDto, HospitalVsUser>().ReverseMap();
            configuration.CreateMap<HospitalVsUserDto, HospitalVsUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditHospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<HospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<CreateOrEditHospitalGroupDto, HospitalGroup>().ReverseMap();
            configuration.CreateMap<HospitalGroupDto, HospitalGroup>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaxRateDto, TaxRate>().ReverseMap();
            configuration.CreateMap<TaxRateDto, TaxRate>().ReverseMap();
            configuration.CreateMap<CreateOrEditDistrictDto, District>().ReverseMap();
            configuration.CreateMap<DistrictDto, District>().ReverseMap();
            configuration.CreateMap<CreateOrEditCityDto, City>().ReverseMap();
            configuration.CreateMap<CityDto, City>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}