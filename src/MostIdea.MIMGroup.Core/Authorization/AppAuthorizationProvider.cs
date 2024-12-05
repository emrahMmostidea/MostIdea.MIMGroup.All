using System.Drawing.Printing;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace MostIdea.MIMGroup.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var salesConsultants = pages.CreateChildPermission(AppPermissions.Pages_SalesConsultants, L("SalesConsultants"));
            salesConsultants.CreateChildPermission(AppPermissions.Pages_SalesConsultants_Create, L("CreateNewSalesConsultant"));
            salesConsultants.CreateChildPermission(AppPermissions.Pages_SalesConsultants_Edit, L("EditSalesConsultant"));
            salesConsultants.CreateChildPermission(AppPermissions.Pages_SalesConsultants_Delete, L("DeleteSalesConsultant"));

            var assistanceVsUsers = pages.CreateChildPermission(AppPermissions.Pages_AssistanceVsUsers, L("AssistanceVsUsers"));
            assistanceVsUsers.CreateChildPermission(AppPermissions.Pages_AssistanceVsUsers_Create, L("CreateNewAssistanceVsUser"));
            assistanceVsUsers.CreateChildPermission(AppPermissions.Pages_AssistanceVsUsers_Edit, L("EditAssistanceVsUser"));
            assistanceVsUsers.CreateChildPermission(AppPermissions.Pages_AssistanceVsUsers_Delete, L("DeleteAssistanceVsUser"));

            var dynamicEnumItems = pages.CreateChildPermission(AppPermissions.Pages_DynamicEnumItems, L("DynamicEnumItems"));
            dynamicEnumItems.CreateChildPermission(AppPermissions.Pages_DynamicEnumItems_Create, L("CreateNewDynamicEnumItem"));
            dynamicEnumItems.CreateChildPermission(AppPermissions.Pages_DynamicEnumItems_Edit, L("EditDynamicEnumItem"));
            dynamicEnumItems.CreateChildPermission(AppPermissions.Pages_DynamicEnumItems_Delete, L("DeleteDynamicEnumItem"));

            var dynamicEnums = pages.CreateChildPermission(AppPermissions.Pages_DynamicEnums, L("DynamicEnums"));
            dynamicEnums.CreateChildPermission(AppPermissions.Pages_DynamicEnums_Create, L("CreateNewDynamicEnum"));
            dynamicEnums.CreateChildPermission(AppPermissions.Pages_DynamicEnums_Edit, L("EditDynamicEnum"));
            dynamicEnums.CreateChildPermission(AppPermissions.Pages_DynamicEnums_Delete, L("DeleteDynamicEnum"));

            var orderComments = pages.CreateChildPermission(AppPermissions.Pages_OrderComments, L("OrderComments"));
            orderComments.CreateChildPermission(AppPermissions.Pages_OrderComments_Create, L("CreateNewOrderComment"));
            orderComments.CreateChildPermission(AppPermissions.Pages_OrderComments_Edit, L("EditOrderComment"));
            orderComments.CreateChildPermission(AppPermissions.Pages_OrderComments_Delete, L("DeleteOrderComment"));

            var orderItems = pages.CreateChildPermission(AppPermissions.Pages_OrderItems, L("OrderItems"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Create, L("CreateNewOrderItem"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Edit, L("EditOrderItem"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Delete, L("DeleteOrderItem"));

            var warehouseVsCouriers = pages.CreateChildPermission(AppPermissions.Pages_WarehouseVsCouriers, L("WarehouseVsCouriers"));
            warehouseVsCouriers.CreateChildPermission(AppPermissions.Pages_WarehouseVsCouriers_Create, L("CreateNewWarehouseVsCourier"));
            warehouseVsCouriers.CreateChildPermission(AppPermissions.Pages_WarehouseVsCouriers_Edit, L("EditWarehouseVsCourier"));
            warehouseVsCouriers.CreateChildPermission(AppPermissions.Pages_WarehouseVsCouriers_Delete, L("DeleteWarehouseVsCourier"));

            var warehouses = pages.CreateChildPermission(AppPermissions.Pages_Warehouses, L("Warehouses"));
            warehouses.CreateChildPermission(AppPermissions.Pages_Warehouses_Create, L("CreateNewWarehouse"));
            warehouses.CreateChildPermission(AppPermissions.Pages_Warehouses_Edit, L("EditWarehouse"));
            warehouses.CreateChildPermission(AppPermissions.Pages_Warehouses_Delete, L("DeleteWarehouse"));

            var orders = pages.CreateChildPermission(AppPermissions.Pages_Orders, L("Orders"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Create, L("CreateNewOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Edit, L("EditOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Delete, L("DeleteOrder"));

            var productPricesForHospitals = pages.CreateChildPermission(AppPermissions.Pages_ProductPricesForHospitals, L("ProductPricesForHospitals"));
            productPricesForHospitals.CreateChildPermission(AppPermissions.Pages_ProductPricesForHospitals_Create, L("CreateNewProductPricesForHospital"));
            productPricesForHospitals.CreateChildPermission(AppPermissions.Pages_ProductPricesForHospitals_Edit, L("EditProductPricesForHospital"));
            productPricesForHospitals.CreateChildPermission(AppPermissions.Pages_ProductPricesForHospitals_Delete, L("DeleteProductPricesForHospital"));

            var products = pages.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));

            var productCategories = pages.CreateChildPermission(AppPermissions.Pages_ProductCategories, L("ProductCategories"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Create, L("CreateNewProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Edit, L("EditProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Delete, L("DeleteProductCategory"));

            var brands = pages.CreateChildPermission(AppPermissions.Pages_Brands, L("Brands"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Create, L("CreateNewBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Edit, L("EditBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Delete, L("DeleteBrand"));

            var addressInformations = pages.CreateChildPermission(AppPermissions.Pages_AddressInformations, L("AddressInformations"));
            addressInformations.CreateChildPermission(AppPermissions.Pages_AddressInformations_Create, L("CreateNewAddressInformation"));
            addressInformations.CreateChildPermission(AppPermissions.Pages_AddressInformations_Edit, L("EditAddressInformation"));
            addressInformations.CreateChildPermission(AppPermissions.Pages_AddressInformations_Delete, L("DeleteAddressInformation"));

            var hospitalVsUsers = pages.CreateChildPermission(AppPermissions.Pages_HospitalVsUsers, L("HospitalVsUsers"));
            hospitalVsUsers.CreateChildPermission(AppPermissions.Pages_HospitalVsUsers_Create, L("CreateNewHospitalVsUser"));
            hospitalVsUsers.CreateChildPermission(AppPermissions.Pages_HospitalVsUsers_Edit, L("EditHospitalVsUser"));
            hospitalVsUsers.CreateChildPermission(AppPermissions.Pages_HospitalVsUsers_Delete, L("DeleteHospitalVsUser"));

            var hospitals = pages.CreateChildPermission(AppPermissions.Pages_Hospitals, L("Hospitals"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Create, L("CreateNewHospital"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Edit, L("EditHospital"));
            hospitals.CreateChildPermission(AppPermissions.Pages_Hospitals_Delete, L("DeleteHospital"));

            var hospitalGroups = pages.CreateChildPermission(AppPermissions.Pages_HospitalGroups, L("HospitalGroups"));
            hospitalGroups.CreateChildPermission(AppPermissions.Pages_HospitalGroups_Create, L("CreateNewHospitalGroup"));
            hospitalGroups.CreateChildPermission(AppPermissions.Pages_HospitalGroups_Edit, L("EditHospitalGroup"));
            hospitalGroups.CreateChildPermission(AppPermissions.Pages_HospitalGroups_Delete, L("DeleteHospitalGroup"));

            var databank = pages.CreateChildPermission(AppPermissions.Pages_Databank, L("Databank"));

            var taxRates = pages.CreateChildPermission(AppPermissions.Pages_TaxRates, L("TaxRates"));
            taxRates.CreateChildPermission(AppPermissions.Pages_TaxRates_Create, L("CreateNewTaxRate"));
            taxRates.CreateChildPermission(AppPermissions.Pages_TaxRates_Edit, L("EditTaxRate"));
            taxRates.CreateChildPermission(AppPermissions.Pages_TaxRates_Delete, L("DeleteTaxRate"));

            var districts = pages.CreateChildPermission(AppPermissions.Pages_Districts, L("Districts"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Create, L("CreateNewDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Edit, L("EditDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Delete, L("DeleteDistrict"));

            var cities = pages.CreateChildPermission(AppPermissions.Pages_Cities, L("Cities"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Create, L("CreateNewCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Edit, L("EditCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Delete, L("DeleteCity"));

            var countries = pages.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            pages.CreateChildPermission(AppPermissions.Pages_Vehicles, L("Vehicles"), multiTenancySides: MultiTenancySides.Tenant);

            var global = context.GetPermissionOrNull(AppPermissions.Global) ?? context.CreatePermission(AppPermissions.Global, L("Global"));
            global.CreateChildPermission(AppPermissions.Global_ShowPrices, L("ShowPrices"), multiTenancySides: MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MIMGroupConsts.LocalizationSourceName);
        }
    }
}