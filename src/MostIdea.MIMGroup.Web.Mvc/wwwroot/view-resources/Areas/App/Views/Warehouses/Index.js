(function() {
    $(function() {
        var _$warehousesTable = $('#WarehousesTable');
        var _warehousesService = abp.services.app.warehouses;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Warehouses.Create'),
            edit: abp.auth.hasPermission('Pages.Warehouses.Edit'),
            'delete': abp.auth.hasPermission('Pages.Warehouses.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Warehouses/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Warehouses/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWarehouseModal'
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
        var dataTable = _$warehousesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _warehousesService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#WarehousesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        coordinateFilter: $('#CoordinateFilterId').val(),
                        districtNameFilter: $('#DistrictNameFilterId').val()
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
                                        id: data.record.warehouse.id
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
                                    deleteWarehouse(data.record.warehouse);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "warehouse.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "warehouse.coordinate",
                    name: "coordinate"
                },
                {
                    targets: 4,
                    data: "districtName",
                    name: "districtFk.name"
                }
            ]
        });

        function getWarehouses() {
            dataTable.ajax.reload();
        }

        function deleteWarehouse(warehouse) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _warehousesService.delete({
                            id: warehouse.id
                        }).done(function() {
                            getWarehouses(true);
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
        $('#CreateNewWarehouseButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _warehousesService
                .getWarehousesToExcel({
                    filter: $('#WarehousesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    coordinateFilter: $('#CoordinateFilterId').val(),
                    districtNameFilter: $('#DistrictNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditWarehouseModalSaved', function() {
            getWarehouses();
        });
        $('#GetWarehousesButton').click(function(e) {
            e.preventDefault();
            getWarehouses();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getWarehouses();
            }
        });
    });
})();