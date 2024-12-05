(function($) {
    app.modals.CreateOrEditHospitalGroupModal = function() {
        var _hospitalGroupsService = abp.services.app.hospitalGroups;
        var _modalManager;
        var _$hospitalGroupInformationForm = null;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$hospitalGroupInformationForm = _modalManager.getModal().find('form[name=HospitalGroupInformationsForm]');
            _$hospitalGroupInformationForm.validate();
        };
        this.save = function() {
            if (!_$hospitalGroupInformationForm.valid()) {
                return;
            }
            var hospitalGroup = _$hospitalGroupInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _hospitalGroupsService.createOrEdit(
                hospitalGroup
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditHospitalGroupModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);