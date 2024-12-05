(function($) {
    app.modals.CreateOrEditAssistanceVsUserModal = function() {
        var _assistanceVsUsersService = abp.services.app.assistanceVsUsers;
        var _modalManager;
        var _$assistanceVsUserInformationForm = null;
        var _AssistanceVsUseruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AssistanceVsUsers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AssistanceVsUsers/_AssistanceVsUserUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$assistanceVsUserInformationForm = _modalManager.getModal().find('form[name=AssistanceVsUserInformationsForm]');
            _$assistanceVsUserInformationForm.validate();
        };
        $('#OpenUserLookupTableButton').click(function() {
            var assistanceVsUser = _$assistanceVsUserInformationForm.serializeFormToObject();
            _AssistanceVsUseruserLookupTableModal.open({
                id: assistanceVsUser.assistanceId,
                displayName: assistanceVsUser.userName
            }, function(data) {
                _$assistanceVsUserInformationForm.find('input[name=userName]').val(data.displayName);
                _$assistanceVsUserInformationForm.find('input[name=assistanceId]').val(data.id);
            });
        });
        $('#ClearUserNameButton').click(function() {
            _$assistanceVsUserInformationForm.find('input[name=userName]').val('');
            _$assistanceVsUserInformationForm.find('input[name=assistanceId]').val('');
        });
        $('#OpenUser2LookupTableButton').click(function() {
            var assistanceVsUser = _$assistanceVsUserInformationForm.serializeFormToObject();
            _AssistanceVsUseruserLookupTableModal.open({
                id: assistanceVsUser.doctorId,
                displayName: assistanceVsUser.userName2
            }, function(data) {
                _$assistanceVsUserInformationForm.find('input[name=userName2]').val(data.displayName);
                _$assistanceVsUserInformationForm.find('input[name=doctorId]').val(data.id);
            });
        });
        $('#ClearUserName2Button').click(function() {
            _$assistanceVsUserInformationForm.find('input[name=userName2]').val('');
            _$assistanceVsUserInformationForm.find('input[name=doctorId]').val('');
        });
        this.save = function() {
            if (!_$assistanceVsUserInformationForm.valid()) {
                return;
            }
            if ($('#AssistanceVsUser_AssistanceId').prop('required') && $('#AssistanceVsUser_AssistanceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#AssistanceVsUser_DoctorId').prop('required') && $('#AssistanceVsUser_DoctorId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            var assistanceVsUser = _$assistanceVsUserInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _assistanceVsUsersService.createOrEdit(
                assistanceVsUser
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditAssistanceVsUserModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);