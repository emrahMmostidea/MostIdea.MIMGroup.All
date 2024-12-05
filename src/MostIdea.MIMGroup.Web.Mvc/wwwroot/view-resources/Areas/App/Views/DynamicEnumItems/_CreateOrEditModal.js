(function ($) {
    app.modals.CreateOrEditDynamicEnumItemModal = function () {

        var _dynamicEnumItemsService = abp.services.app.dynamicEnumItems;

        var _modalManager;
        var _$dynamicEnumItemInformationForm = null;

		        var _DynamicEnumItemdynamicEnumLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicEnumItems/DynamicEnumLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicEnumItems/_DynamicEnumItemDynamicEnumLookupTableModal.js',
            modalClass: 'DynamicEnumLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$dynamicEnumItemInformationForm = _modalManager.getModal().find('form[name=DynamicEnumItemInformationsForm]');
            _$dynamicEnumItemInformationForm.validate();
        };

		          $('#OpenDynamicEnumLookupTableButton').click(function () {

            var dynamicEnumItem = _$dynamicEnumItemInformationForm.serializeFormToObject();

            _DynamicEnumItemdynamicEnumLookupTableModal.open({ id: dynamicEnumItem.dynamicEnumId, displayName: dynamicEnumItem.dynamicEnumName }, function (data) {
                _$dynamicEnumItemInformationForm.find('input[name=dynamicEnumName]').val(data.displayName); 
                _$dynamicEnumItemInformationForm.find('input[name=dynamicEnumId]').val(data.id); 
            });
        });
		
		$('#ClearDynamicEnumNameButton').click(function () {
                _$dynamicEnumItemInformationForm.find('input[name=dynamicEnumName]').val(''); 
                _$dynamicEnumItemInformationForm.find('input[name=dynamicEnumId]').val(''); 
        });
		


        this.save = function () {
            if (!_$dynamicEnumItemInformationForm.valid()) {
                return;
            }
            if ($('#DynamicEnumItem_DynamicEnumId').prop('required') && $('#DynamicEnumItem_DynamicEnumId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DynamicEnum')));
                return;
            }

            

            var dynamicEnumItem = _$dynamicEnumItemInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _dynamicEnumItemsService.createOrEdit(
				dynamicEnumItem
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDynamicEnumItemModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);