(function($) {
    app.modals.CreateOrEditAddressInformationModal = function() {
        var _addressInformationsService = abp.services.app.addressInformations;
        var _modalManager;
        var _$addressInformationInformationForm = null;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$addressInformationInformationForm = _modalManager.getModal().find('form[name=AddressInformationInformationsForm]');
            _$addressInformationInformationForm.validate();
        };
        this.save = function() {
            if (!_$addressInformationInformationForm.valid()) {
                return;
            }
            var addressInformation = _$addressInformationInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _addressInformationsService.createOrEdit(
                addressInformation
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAddressInformationModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);