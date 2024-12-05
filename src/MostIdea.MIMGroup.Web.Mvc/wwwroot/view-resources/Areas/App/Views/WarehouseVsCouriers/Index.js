(function() {
    $(function() {
        var _$warehouseVsCouriersTable = $('#WarehouseVsCouriersTable');
        var _warehouseVsCouriersService = abp.services.app.warehouseVsCouriers;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.WarehouseVsCouriers.Create'),
            edit: abp.auth.hasPermission('Pages.WarehouseVsCouriers.Edit'),
            'delete': abp.auth.hasPermission('Pages.WarehouseVsCouriers.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/WarehouseVsCouriers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WarehouseVsCouriers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWarehouseVsCourierModal'
        });
        var getDateFilter = function(element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }
        var getMaxDateFilter = function(element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z");
        }
        var dataTable = _$warehouseVsCouriersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _warehouseVsCouriersService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#WarehouseVsCouriersTableFilter').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        warehouseNameFilter: $('#WarehouseNameFilterId').val()
                    };
                }
            },
            columnDefs: [{
                    className: 'control responsive',
                    orderable: false,
                    render: function() {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [{
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function() {
                                    return _permissions.edit;
                                },
                                action: function(data) {
                                    _createOrEditModal.open({
                                        id: data.record.warehouseVsCourier.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function() {
                                    return _permissions.delete;
                                },
                                action: function(data) {
                                    deleteWarehouseVsCourier(data.record.warehouseVsCourier);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "userName",
                    name: "courierFk.name"
                },
                {
                    targets: 3,
                    data: "warehouseName",
                    name: "warehouseFk.name"
                }
            ]
        });

        function getWarehouseVsCouriers() {
            dataTable.ajax.reload();
        }

        function deleteWarehouseVsCourier(warehouseVsCourier) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _warehouseVsCouriersService.delete({
                            id: warehouseVsCourier.id
                        }).done(function() {
                            getWarehouseVsCouriers(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }
        $('#ShowAdvancedFiltersSpan').click(function() {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });
        $('#HideAdvancedFiltersSpan').click(function() {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });
        $('#CreateNewWarehouseVsCourierButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _warehouseVsCouriersService
                .getWarehouseVsCouriersToExcel({
                    filter: $('#WarehouseVsCouriersTableFilter').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    warehouseNameFilter: $('#WarehouseNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditWarehouseVsCourierModalSaved', function() {
            getWarehouseVsCouriers();
        });
        $('#GetWarehouseVsCouriersButton').click(function(e) {
            e.preventDefault();
            getWarehouseVsCouriers();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getWarehouseVsCouriers();
            }
        });
    });
})();