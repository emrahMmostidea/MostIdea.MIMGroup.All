(function($) {
    app.modals.CreateOrEditSalesConsultantModal = function() {
        var _salesConsultantsService = abp.services.app.salesConsultants;
        var _modalManager;
        var _$salesConsultantInformationForm = null;
        var _SalesConsultantuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SalesConsultants/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SalesConsultants/_SalesConsultantUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$salesConsultantInformationForm = _modalManager.getModal().find('form[name=SalesConsultantInformationsForm]');
            _$salesConsultantInformationForm.validate();
        };
        $('#OpenUserLookupTableButton').click(function() {
            var salesConsultant = _$salesConsultantInformationForm.serializeFormToObject();
            _SalesConsultantuserLookupTableModal.open({
                id: salesConsultant.salesConsultantId,
                displayName: salesConsultant.userName
            }, function(data) {
                _$salesConsultantInformationForm.find('input[name=userName]').val(data.displayName);
                _$salesConsultantInformationForm.find('input[name=salesConsultantId]').val(data.id);
            });
        });
        $('#ClearUserNameButton').click(function() {
            _$salesConsultantInformationForm.find('input[name=userName]').val('');
            _$salesConsultantInformationForm.find('input[name=salesConsultantId]').val('');
        });
        $('#OpenUser2LookupTableButton').click(function() {
            var salesConsultant = _$salesConsultantInformationForm.serializeFormToObject();
            _SalesConsultantuserLookupTableModal.open({
                id: salesConsultant.doctorId,
                displayName: salesConsultant.userName2
            }, function(data) {
                _$salesConsultantInformationForm.find('input[name=userName2]').val(data.displayName);
                _$salesConsultantInformationForm.find('input[name=doctorId]').val(data.id);
            });
        });
        $('#ClearUserName2Button').click(function() {
            _$salesConsultantInformationForm.find('input[name=userName2]').val('');
            _$salesConsultantInformationForm.find('input[name=doctorId]').val('');
        });
        this.save = function() {
            if (!_$salesConsultantInformationForm.valid()) {
                return;
            }
            if ($('#SalesConsultant_SalesConsultantId').prop('required') && $('#SalesConsultant_SalesConsultantId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#SalesConsultant_DoctorId').prop('required') && $('#SalesConsultant_DoctorId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            var salesConsultant = _$salesConsultantInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _salesConsultantsService.createOrEdit(
                salesConsultant
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditSalesConsultantModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);