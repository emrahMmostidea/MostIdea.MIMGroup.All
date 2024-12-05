(function($) {
    app.modals.CreateOrEditWarehouseVsCourierModal = function() {
        var _warehouseVsCouriersService = abp.services.app.warehouseVsCouriers;
        var _modalManager;
        var _$warehouseVsCourierInformationForm = null;
        var _WarehouseVsCourieruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/WarehouseVsCouriers/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WarehouseVsCouriers/_WarehouseVsCourierUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        var _WarehouseVsCourierwarehouseLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/WarehouseVsCouriers/WarehouseLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WarehouseVsCouriers/_WarehouseVsCourierWarehouseLookupTableModal.js',
            modalClass: 'WarehouseLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$warehouseVsCourierInformationForm = _modalManager.getModal().find('form[name=WarehouseVsCourierInformationsForm]');
            _$warehouseVsCourierInformationForm.validate();
        };
        $('#OpenUserLookupTableButton').click(function() {
            var warehouseVsCourier = _$warehouseVsCourierInformationForm.serializeFormToObject();
            _WarehouseVsCourieruserLookupTableModal.open({
                id: warehouseVsCourier.courierId,
                displayName: warehouseVsCourier.userName
            }, function(data) {
                _$warehouseVsCourierInformationForm.find('input[name=userName]').val(data.displayName);
                _$warehouseVsCourierInformationForm.find('input[name=courierId]').val(data.id);
            });
        });
        $('#ClearUserNameButton').click(function() {
            _$warehouseVsCourierInformationForm.find('input[name=userName]').val('');
            _$warehouseVsCourierInformationForm.find('input[name=courierId]').val('');
        });
        $('#OpenWarehouseLookupTableButton').click(function() {
            var warehouseVsCourier = _$warehouseVsCourierInformationForm.serializeFormToObject();
            _WarehouseVsCourierwarehouseLookupTableModal.open({
                id: warehouseVsCourier.warehouseId,
                displayName: warehouseVsCourier.warehouseName
            }, function(data) {
                _$warehouseVsCourierInformationForm.find('input[name=warehouseName]').val(data.displayName);
                _$warehouseVsCourierInformationForm.find('input[name=warehouseId]').val(data.id);
            });
        });
        $('#ClearWarehouseNameButton').click(function() {
            _$warehouseVsCourierInformationForm.find('input[name=warehouseName]').val('');
            _$warehouseVsCourierInformationForm.find('input[name=warehouseId]').val('');
        });
        this.save = function() {
            if (!_$warehouseVsCourierInformationForm.valid()) {
                return;
            }
            if ($('#WarehouseVsCourier_CourierId').prop('required') && $('#WarehouseVsCourier_CourierId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#WarehouseVsCourier_WarehouseId').prop('required') && $('#WarehouseVsCourier_WarehouseId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Warehouse')));
                return;
            }
            var warehouseVsCourier = _$warehouseVsCourierInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _warehouseVsCouriersService.createOrEdit(
                warehouseVsCourier
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditWarehouseVsCourierModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);