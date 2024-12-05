using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using MostIdea.MIMGroup.Authorization;

namespace MostIdea.MIMGroup.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                    ))
                
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Hospitals,
                        L("Hospitals"),
                        url: "App/Hospitals",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Hospitals)
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.HospitalGroups,
                            L("HospitalGroups"),
                            url: "App/HospitalGroups",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_HospitalGroups)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Hospitals,
                            L("Hospitals"),
                            url: "App/Hospitals",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Hospitals)
                        )
                    )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.HospitalVsUsers,
                //        L("HospitalVsUsers"),
                //        url: "App/HospitalVsUsers",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_HospitalVsUsers)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.AddressInformations,
                //        L("AddressInformations"),
                //        url: "App/AddressInformations",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AddressInformations)
                //    )
                //)
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Orders,
                        L("Orders"),
                        url: "App/Orders",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Orders)
                    )
                )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.OrderItems,
                //        L("OrderItems"),
                //        url: "App/OrderItems",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_OrderItems)
                //    )
                //)
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.WarehouseVsCouriers,
                //        L("WarehouseVsCouriers"),
                //        url: "App/WarehouseVsCouriers",
                //        icon: "flaticon-more",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_WarehouseVsCouriers)
                //    )
                //)

                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Products,
                        L("Products"),
                        url: "App/Products",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Products)
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Products,
                            L("Products"),
                            url: "App/Products",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Products)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ProductCategories,
                            L("ProductCategories"),
                            url: "App/ProductCategories",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProductCategories)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ProductPricesForHospitals,
                            L("ProductPricesForHospitals"),
                            url: "App/ProductPricesForHospitals",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ProductPricesForHospitals)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Doctors,
                        L("Doctors"),
                        url: "App/GetUsersForType/Doctors",
                        icon: "flaticon-users",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Assistants,
                        L("Assistants"),
                        url: "App/GetUsersForType/Assistants",
                        icon: "flaticon-users",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Couriers,
                        L("Couriers"),
                        url: "App/GetUsersForType/Couriers",
                        icon: "flaticon-users",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.SalesReps,
                        L("SalesReps"),
                        url: "App/GetUsersForType/SalesReps",
                        icon: "flaticon-users",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                )

                .AddItem(new MenuItemDefinition(
                    AppPageNames.Common.Databank,
                    L("Databank"),
                    url: "#",
                    icon: "flaticon-more",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Databank)
                )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Vehicles,
                            L("Vehicles"),
                            url: "App/Vehicles",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Vehicles)
                        )
                    )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DynamicEnums,
                        L("DynamicEnums"),
                        url: "App/DynamicEnums",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DynamicEnums)
                    )
                )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Countries,
                            L("Countries"),
                            url: "App/Countries",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Countries)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Cities,
                            L("Cities"),
                            url: "App/Cities",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cities)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Districts,
                            L("Districts"),
                            url: "App/Districts",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Districts)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.TaxRates,
                            L("TaxRates"),
                            url: "App/TaxRates",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TaxRates)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Brands,
                            L("Brands"),
                            url: "App/Brands",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Brands)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Warehouses,
                            L("Warehouses"),
                            url: "App/Warehouses",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Warehouses)
                        )
                    )

                )
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.Tenants,
                    L("Tenants"),
                    url: "App/Tenants",
                    icon: "flaticon-list-3",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Editions,
                        L("Editions"),
                        url: "App/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "App/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_WebhookSubscription)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.DynamicProperties,
                            L("DynamicProperties"),
                            url: "App/DynamicProperty",
                            icon: "flaticon-interface-8",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_DynamicProperties)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_Settings)
                        )
                    )

                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MIMGroupConsts.LocalizationSourceName);
        }
    }
}