(function() {
    $(function() {
        var _$ordersTable = $('#OrdersTable');
        var _ordersService = abp.services.app.orders;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Orders.Create'),
            edit: abp.auth.hasPermission('Pages.Orders.Edit'),
            'delete': abp.auth.hasPermission('Pages.Orders.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Orders/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Orders/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditOrderModal'
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
        var dataTable = _$ordersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _ordersService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#OrdersTableFilter').val(),
                        minTotalFilter: $('#MinTotalFilterId').val(),
                        maxTotalFilter: $('#MaxTotalFilterId').val(),
                        minTaxFilter: $('#MinTaxFilterId').val(),
                        maxTaxFilter: $('#MaxTaxFilterId').val(),
                        minGrandTotalFilter: $('#MinGrandTotalFilterId').val(),
                        maxGrandTotalFilter: $('#MaxGrandTotalFilterId').val(),
                        statusFilter: $('#StatusFilterId').val(),
                        orderNoFilter: $('#OrderNoFilterId').val(),
                        addressInformationNameFilter: $('#AddressInformationNameFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        hospitalNameFilter: $('#HospitalNameFilterId').val(),
                        userName2Filter: $('#UserName2FilterId').val(),
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
                                        id: data.record.order.id
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
                                    deleteOrder(data.record.order);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "order.total",
                    name: "total"
                },
                {
                    targets: 3,
                    data: "order.tax",
                    name: "tax"
                },
                {
                    targets: 4,
                    data: "order.grandTotal",
                    name: "grandTotal"
                },
                {
                    targets: 5,
                    data: "order.status",
                    name: "status",
                    render: function(status) {
                        return app.localize('Enum_OrderStatusEnum_' + status);
                    }
                },
                {
                    targets: 6,
                    data: "order.orderNo",
                    name: "orderNo"
                },
                {
                    targets: 7,
                    data: "addressInformationName",
                    name: "addressInformationFk.name"
                },
                {
                    targets: 8,
                    data: "userName",
                    name: "courierFk.name"
                },
                {
                    targets: 9,
                    data: "hospitalName",
                    name: "hospitalFk.name"
                },
                {
                    targets: 10,
                    data: "userName2",
                    name: "doctorFk.name"
                },
                {
                    targets: 11,
                    data: "warehouseName",
                    name: "warehouseFk.name"
                }
            ]
        });

        function getOrders() {
            dataTable.ajax.reload();
        }

        function deleteOrder(order) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _ordersService.delete({
                            id: order.id
                        }).done(function() {
                            getOrders(true);
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
        $('#CreateNewOrderButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _ordersService
                .getOrdersToExcel({
                    filter: $('#OrdersTableFilter').val(),
                    minTotalFilter: $('#MinTotalFilterId').val(),
                    maxTotalFilter: $('#MaxTotalFilterId').val(),
                    minTaxFilter: $('#MinTaxFilterId').val(),
                    maxTaxFilter: $('#MaxTaxFilterId').val(),
                    minGrandTotalFilter: $('#MinGrandTotalFilterId').val(),
                    maxGrandTotalFilter: $('#MaxGrandTotalFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    orderNoFilter: $('#OrderNoFilterId').val(),
                    addressInformationNameFilter: $('#AddressInformationNameFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    hospitalNameFilter: $('#HospitalNameFilterId').val(),
                    userName2Filter: $('#UserName2FilterId').val(),
                    warehouseNameFilter: $('#WarehouseNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditOrderModalSaved', function() {
            getOrders();
        });
        $('#GetOrdersButton').click(function(e) {
            e.preventDefault();
            getOrders();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getOrders();
            }
        });
    });
})();