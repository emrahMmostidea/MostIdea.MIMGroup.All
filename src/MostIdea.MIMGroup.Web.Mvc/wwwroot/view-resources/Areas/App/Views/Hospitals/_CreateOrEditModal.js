(function($) {
    app.modals.CreateOrEditHospitalModal = function() {
        var _hospitalsService = abp.services.app.hospitals;
        var _modalManager;
        var _$hospitalInformationForm = null;
        var _HospitalhospitalGroupLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Hospitals/HospitalGroupLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Hospitals/_HospitalHospitalGroupLookupTableModal.js',
            modalClass: 'HospitalGroupLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$hospitalInformationForm = _modalManager.getModal().find('form[name=HospitalInformationsForm]');
            _$hospitalInformationForm.validate();
        };
        $('#OpenHospitalGroupLookupTableButton').click(function() {
            var hospital = _$hospitalInformationForm.serializeFormToObject();
            _HospitalhospitalGroupLookupTableModal.open({
                id: hospital.hospitalGroupId,
                displayName: hospital.hospitalGroupName
            }, function(data) {
                _$hospitalInformationForm.find('input[name=hospitalGroupName]').val(data.displayName);
                _$hospitalInformationForm.find('input[name=hospitalGroupId]').val(data.id);
            });
        });
        $('#ClearHospitalGroupNameButton').click(function() {
            _$hospitalInformationForm.find('input[name=hospitalGroupName]').val('');
            _$hospitalInformationForm.find('input[name=hospitalGroupId]').val('');
        });
        this.save = function() {
            if (!_$hospitalInformationForm.valid()) {
                return;
            }
            if ($('#Hospital_HospitalGroupId').prop('required') && $('#Hospital_HospitalGroupId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('HospitalGroup')));
                return;
            }
            var hospital = _$hospitalInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _hospitalsService.createOrEdit(
                hospital
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditHospitalModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);