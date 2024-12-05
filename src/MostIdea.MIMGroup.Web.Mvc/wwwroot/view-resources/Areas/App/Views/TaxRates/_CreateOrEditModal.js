(function($) {
    app.modals.CreateOrEditTaxRateModal = function() {
        var _taxRatesService = abp.services.app.taxRates;
        var _modalManager;
        var _$taxRateInformationForm = null;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$taxRateInformationForm = _modalManager.getModal().find('form[name=TaxRateInformationsForm]');
            _$taxRateInformationForm.validate();
        };
        this.save = function() {
            if (!_$taxRateInformationForm.valid()) {
                return;
            }
            var taxRate = _$taxRateInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _taxRatesService.createOrEdit(
                taxRate
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditTaxRateModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);