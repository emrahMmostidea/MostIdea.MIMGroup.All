(function($) {
    app.modals.CreateOrEditBrandModal = function() {
        var _brandsService = abp.services.app.brands;
        var _modalManager;
        var _$brandInformationForm = null;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$brandInformationForm = _modalManager.getModal().find('form[name=BrandInformationsForm]');
            _$brandInformationForm.validate();
        };
        this.save = function() {
            if (!_$brandInformationForm.valid()) {
                return;
            }
            var brand = _$brandInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _brandsService.createOrEdit(
                brand
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditBrandModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);