(function($) {
    app.modals.CreateOrEditProductCategoryModal = function() {
        var _productCategoriesService = abp.services.app.productCategories;
        var _modalManager;
        var _$productCategoryInformationForm = null;
        var _ProductCategoryproductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategories/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategories/_ProductCategoryProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });
        var _ProductCategorybrandLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategories/BrandLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategories/_ProductCategoryBrandLookupTableModal.js',
            modalClass: 'BrandLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$productCategoryInformationForm = _modalManager.getModal().find('form[name=ProductCategoryInformationsForm]');
            _$productCategoryInformationForm.validate();
        };
        $('#OpenProductCategoryLookupTableButton').click(function() {
            var productCategory = _$productCategoryInformationForm.serializeFormToObject();
            _ProductCategoryproductCategoryLookupTableModal.open({
                id: productCategory.productCategoryId,
                displayName: productCategory.productCategoryName
            }, function(data) {
                _$productCategoryInformationForm.find('input[name=productCategoryName]').val(data.displayName);
                _$productCategoryInformationForm.find('input[name=productCategoryId]').val(data.id);
            });
        });
        $('#ClearProductCategoryNameButton').click(function() {
            _$productCategoryInformationForm.find('input[name=productCategoryName]').val('');
            _$productCategoryInformationForm.find('input[name=productCategoryId]').val('');
        });
        $('#OpenBrandLookupTableButton').click(function() {
            var productCategory = _$productCategoryInformationForm.serializeFormToObject();
            _ProductCategorybrandLookupTableModal.open({
                id: productCategory.brandId,
                displayName: productCategory.brandName
            }, function(data) {
                _$productCategoryInformationForm.find('input[name=brandName]').val(data.displayName);
                _$productCategoryInformationForm.find('input[name=brandId]').val(data.id);
            });
        });
        $('#ClearBrandNameButton').click(function() {
            _$productCategoryInformationForm.find('input[name=brandName]').val('');
            _$productCategoryInformationForm.find('input[name=brandId]').val('');
        });
        this.save = function() {
            if (!_$productCategoryInformationForm.valid()) {
                return;
            }
            if ($('#ProductCategory_ProductCategoryId').prop('required') && $('#ProductCategory_ProductCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductCategory')));
                return;
            }
            if ($('#ProductCategory_BrandId').prop('required') && $('#ProductCategory_BrandId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Brand')));
                return;
            }
            var productCategory = _$productCategoryInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _productCategoriesService.createOrEdit(
                productCategory
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditProductCategoryModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);