(function($) {
    app.modals.CreateOrEditProductPricesForHospitalModal = function() {
        var _productPricesForHospitalsService = abp.services.app.productPricesForHospitals;
        var _modalManager;
        var _$productPricesForHospitalInformationForm = null;
        var _ProductPricesForHospitalproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductPricesForHospitals/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductPricesForHospitals/_ProductPricesForHospitalProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });
        var _ProductPricesForHospitalproductCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductPricesForHospitals/ProductCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductPricesForHospitals/_ProductPricesForHospitalProductCategoryLookupTableModal.js',
            modalClass: 'ProductCategoryLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$productPricesForHospitalInformationForm = _modalManager.getModal().find('form[name=ProductPricesForHospitalInformationsForm]');
            _$productPricesForHospitalInformationForm.validate();
        };
        $('#OpenProductLookupTableButton').click(function() {
            var productPricesForHospital = _$productPricesForHospitalInformationForm.serializeFormToObject();
            _ProductPricesForHospitalproductLookupTableModal.open({
                id: productPricesForHospital.productId,
                displayName: productPricesForHospital.productName
            }, function(data) {
                _$productPricesForHospitalInformationForm.find('input[name=productName]').val(data.displayName);
                _$productPricesForHospitalInformationForm.find('input[name=productId]').val(data.id);
            });
        });
        $('#ClearProductNameButton').click(function() {
            _$productPricesForHospitalInformationForm.find('input[name=productName]').val('');
            _$productPricesForHospitalInformationForm.find('input[name=productId]').val('');
        });
        $('#OpenProductCategoryLookupTableButton').click(function() {
            var productPricesForHospital = _$productPricesForHospitalInformationForm.serializeFormToObject();
            _ProductPricesForHospitalproductCategoryLookupTableModal.open({
                id: productPricesForHospital.productCategoryId,
                displayName: productPricesForHospital.productCategoryName
            }, function(data) {
                _$productPricesForHospitalInformationForm.find('input[name=productCategoryName]').val(data.displayName);
                _$productPricesForHospitalInformationForm.find('input[name=productCategoryId]').val(data.id);
            });
        });
        $('#ClearProductCategoryNameButton').click(function() {
            _$productPricesForHospitalInformationForm.find('input[name=productCategoryName]').val('');
            _$productPricesForHospitalInformationForm.find('input[name=productCategoryId]').val('');
        });
        this.save = function() {
            if (!_$productPricesForHospitalInformationForm.valid()) {
                return;
            }
            if ($('#ProductPricesForHospital_ProductId').prop('required') && $('#ProductPricesForHospital_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#ProductPricesForHospital_ProductCategoryId').prop('required') && $('#ProductPricesForHospital_ProductCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductCategory')));
                return;
            }
            var productPricesForHospital = _$productPricesForHospitalInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _productPricesForHospitalsService.createOrEdit(
                productPricesForHospital
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditProductPricesForHospitalModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);