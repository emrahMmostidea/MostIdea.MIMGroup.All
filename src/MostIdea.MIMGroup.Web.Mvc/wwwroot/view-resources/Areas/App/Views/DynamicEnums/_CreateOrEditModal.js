(function ($) {
    app.modals.CreateOrEditDynamicEnumModal = function () {

        var _dynamicEnumsService = abp.services.app.dynamicEnums;

        var _modalManager;
        var _$dynamicEnumInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$dynamicEnumInformationForm = _modalManager.getModal().find('form[name=DynamicEnumInformationsForm]');
            _$dynamicEnumInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$dynamicEnumInformationForm.valid()) {
                return;
            }

            

            var dynamicEnum = _$dynamicEnumInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _dynamicEnumsService.createOrEdit(
				dynamicEnum
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDynamicEnumModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);