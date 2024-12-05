(function($) {
    app.modals.CreateOrEditHospitalVsUserModal = function() {
        var _hospitalVsUsersService = abp.services.app.hospitalVsUsers;
        var _modalManager;
        var _$hospitalVsUserInformationForm = null;
        var _HospitalVsUserhospitalLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HospitalVsUsers/HospitalLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HospitalVsUsers/_HospitalVsUserHospitalLookupTableModal.js',
            modalClass: 'HospitalLookupTableModal'
        });
        var _HospitalVsUseruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HospitalVsUsers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HospitalVsUsers/_HospitalVsUserUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$hospitalVsUserInformationForm = _modalManager.getModal().find('form[name=HospitalVsUserInformationsForm]');
            _$hospitalVsUserInformationForm.validate();
        };
        $('#OpenHospitalLookupTableButton').click(function() {
            var hospitalVsUser = _$hospitalVsUserInformationForm.serializeFormToObject();
            _HospitalVsUserhospitalLookupTableModal.open({
                id: hospitalVsUser.hospitalId,
                displayName: hospitalVsUser.hospitalName
            }, function(data) {
                _$hospitalVsUserInformationForm.find('input[name=hospitalName]').val(data.displayName);
                _$hospitalVsUserInformationForm.find('input[name=hospitalId]').val(data.id);
            });
        });
        $('#ClearHospitalNameButton').click(function() {
            _$hospitalVsUserInformationForm.find('input[name=hospitalName]').val('');
            _$hospitalVsUserInformationForm.find('input[name=hospitalId]').val('');
        });
        $('#OpenUserLookupTableButton').click(function() {
            var hospitalVsUser = _$hospitalVsUserInformationForm.serializeFormToObject();
            _HospitalVsUseruserLookupTableModal.open({
                id: hospitalVsUser.userId,
                displayName: hospitalVsUser.userName
            }, function(data) {
                _$hospitalVsUserInformationForm.find('input[name=userName]').val(data.displayName);
                _$hospitalVsUserInformationForm.find('input[name=userId]').val(data.id);
            });
        });
        $('#ClearUserNameButton').click(function() {
            _$hospitalVsUserInformationForm.find('input[name=userName]').val('');
            _$hospitalVsUserInformationForm.find('input[name=userId]').val('');
        });
        this.save = function() {
            if (!_$hospitalVsUserInformationForm.valid()) {
                return;
            }
            if ($('#HospitalVsUser_HospitalId').prop('required') && $('#HospitalVsUser_HospitalId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Hospital')));
                return;
            }
            if ($('#HospitalVsUser_UserId').prop('required') && $('#HospitalVsUser_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            var hospitalVsUser = _$hospitalVsUserInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _hospitalVsUsersService.createOrEdit(
                hospitalVsUser
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditHospitalVsUserModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);