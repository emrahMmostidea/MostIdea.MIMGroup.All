(function($) {
    app.modals.CreateOrEditWarehouseModal = function() {
        var _warehousesService = abp.services.app.warehouses;
        var _modalManager;
        var _$warehouseInformationForm = null;
        var _WarehousedistrictLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Warehouses/DistrictLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Warehouses/_WarehouseDistrictLookupTableModal.js',
            modalClass: 'DistrictLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$warehouseInformationForm = _modalManager.getModal().find('form[name=WarehouseInformationsForm]');
            _$warehouseInformationForm.validate();
        };
        $('#OpenDistrictLookupTableButton').click(function() {
            var warehouse = _$warehouseInformationForm.serializeFormToObject();
            _WarehousedistrictLookupTableModal.open({
                id: warehouse.districtId,
                displayName: warehouse.districtName
            }, function(data) {
                _$warehouseInformationForm.find('input[name=districtName]').val(data.displayName);
                _$warehouseInformationForm.find('input[name=districtId]').val(data.id);
            });
        });
        $('#ClearDistrictNameButton').click(function() {
            _$warehouseInformationForm.find('input[name=districtName]').val('');
            _$warehouseInformationForm.find('input[name=districtId]').val('');
        });
        this.save = function() {
            if (!_$warehouseInformationForm.valid()) {
                return;
            }
            if ($('#Warehouse_DistrictId').prop('required') && $('#Warehouse_DistrictId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('District')));
                return;
            }
            var warehouse = _$warehouseInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _warehousesService.createOrEdit(
                warehouse
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditWarehouseModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);