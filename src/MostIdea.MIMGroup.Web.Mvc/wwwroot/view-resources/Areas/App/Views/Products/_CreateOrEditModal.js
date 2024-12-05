(function($) {
    app.modals.CreateOrEditProductModal = function() {
        var _productsService = abp.services.app.products;
        var _modalManager;
        var _$productInformationForm = null;
        var _ProductproductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });
        var _ProducttaxRateLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/TaxRateLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_ProductTaxRateLookupTableModal.js',
            modalClass: 'TaxRateLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
            _$productInformationForm.validate();
        };
        $('#OpenProductCategoryLookupTableButton').click(function() {
            var product = _$productInformationForm.serializeFormToObject();
            _ProductproductCategoryLookupTableModal.open({
                id: product.productCategoryId,
                displayName: product.productCategoryName
            }, function(data) {
                _$productInformationForm.find('input[name=productCategoryName]').val(data.displayName);
                _$productInformationForm.find('input[name=productCategoryId]').val(data.id);
            });
        });
        $('#ClearProductCategoryNameButton').click(function() {
            _$productInformationForm.find('input[name=productCategoryName]').val('');
            _$productInformationForm.find('input[name=productCategoryId]').val('');
        });
        $('#OpenTaxRateLookupTableButton').click(function() {
            var product = _$productInformationForm.serializeFormToObject();
            _ProducttaxRateLookupTableModal.open({
                id: product.taxRateId,
                displayName: product.taxRateName
            }, function(data) {
                _$productInformationForm.find('input[name=taxRateName]').val(data.displayName);
                _$productInformationForm.find('input[name=taxRateId]').val(data.id);
            });
        });
        $('#ClearTaxRateNameButton').click(function() {
            _$productInformationForm.find('input[name=taxRateName]').val('');
            _$productInformationForm.find('input[name=taxRateId]').val('');
        });
        this.save = function() {
            if (!_$productInformationForm.valid()) {
                return;
            }
            if ($('#Product_ProductCategoryId').prop('required') && $('#Product_ProductCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductCategory')));
                return;
            }
            if ($('#Product_TaxRateId').prop('required') && $('#Product_TaxRateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TaxRate')));
                return;
            }
            var product = _$productInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _productsService.createOrEdit(
                product
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditProductModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);