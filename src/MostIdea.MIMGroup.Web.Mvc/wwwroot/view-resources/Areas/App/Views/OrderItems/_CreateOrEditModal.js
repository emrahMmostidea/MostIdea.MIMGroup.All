(function($) {
    app.modals.CreateOrEditOrderItemModal = function() {
        var _orderItemsService = abp.services.app.orderItems;
        var _modalManager;
        var _$orderItemInformationForm = null;
        var _OrderItemproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/OrderItems/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/OrderItems/_OrderItemProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });
        var _OrderItemorderLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/OrderItems/OrderLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/OrderItems/_OrderItemOrderLookupTableModal.js',
            modalClass: 'OrderLookupTableModal'
        });
        this.init = function(modalManager) {
            _modalManager = modalManager;
            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            _$orderItemInformationForm = _modalManager.getModal().find('form[name=OrderItemInformationsForm]');
            _$orderItemInformationForm.validate();
        };
        $('#OpenProductLookupTableButton').click(function() {
            var orderItem = _$orderItemInformationForm.serializeFormToObject();
            _OrderItemproductLookupTableModal.open({
                id: orderItem.productId,
                displayName: orderItem.productName
            }, function(data) {
                _$orderItemInformationForm.find('input[name=productName]').val(data.displayName);
                _$orderItemInformationForm.find('input[name=productId]').val(data.id);
            });
        });
        $('#ClearProductNameButton').click(function() {
            _$orderItemInformationForm.find('input[name=productName]').val('');
            _$orderItemInformationForm.find('input[name=productId]').val('');
        });
        $('#OpenOrderLookupTableButton').click(function() {
            var orderItem = _$orderItemInformationForm.serializeFormToObject();
            _OrderItemorderLookupTableModal.open({
                id: orderItem.orderId,
                displayName: orderItem.orderOrderNo
            }, function(data) {
                _$orderItemInformationForm.find('input[name=orderOrderNo]').val(data.displayName);
                _$orderItemInformationForm.find('input[name=orderId]').val(data.id);
            });
        });
        $('#ClearOrderOrderNoButton').click(function() {
            _$orderItemInformationForm.find('input[name=orderOrderNo]').val('');
            _$orderItemInformationForm.find('input[name=orderId]').val('');
        });
        this.save = function() {
            if (!_$orderItemInformationForm.valid()) {
                return;
            }
            if ($('#OrderItem_ProductId').prop('required') && $('#OrderItem_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#OrderItem_OrderId').prop('required') && $('#OrderItem_OrderId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Order')));
                return;
            }
            var orderItem = _$orderItemInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _orderItemsService.createOrEdit(
                orderItem
            ).done(function() {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditOrderItemModalSaved');
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);