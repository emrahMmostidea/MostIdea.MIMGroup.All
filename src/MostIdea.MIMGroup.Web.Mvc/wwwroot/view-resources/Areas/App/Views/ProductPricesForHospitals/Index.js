(function() {
    $(function() {
        var _$productPricesForHospitalsTable = $('#ProductPricesForHospitalsTable');
        var _productPricesForHospitalsService = abp.services.app.productPricesForHospitals;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProductPricesForHospitals.Create'),
            edit: abp.auth.hasPermission('Pages.ProductPricesForHospitals.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProductPricesForHospitals.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductPricesForHospitals/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductPricesForHospitals/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductPricesForHospitalModal'
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
        var dataTable = _$productPricesForHospitalsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productPricesForHospitalsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#ProductPricesForHospitalsTableFilter').val(),
                        minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
                        maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
                        minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
                        maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
                        minPriceFilter: $('#MinPriceFilterId').val(),
                        maxPriceFilter: $('#MaxPriceFilterId').val(),
                        productNameFilter: $('#ProductNameFilterId').val(),
                        productCategoryNameFilter: $('#ProductCategoryNameFilterId').val()
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
                                        id: data.record.productPricesForHospital.id
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
                                    deleteProductPricesForHospital(data.record.productPricesForHospital);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "productPricesForHospital.startDate",
                    name: "startDate",
                    render: function(startDate) {
                        if (startDate) {
                            return moment(startDate).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 3,
                    data: "productPricesForHospital.endDate",
                    name: "endDate",
                    render: function(endDate) {
                        if (endDate) {
                            return moment(endDate).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "productPricesForHospital.price",
                    name: "price"
                },
                {
                    targets: 5,
                    data: "productName",
                    name: "productFk.name"
                },
                {
                    targets: 6,
                    data: "productCategoryName",
                    name: "productCategoryFk.name"
                }
            ]
        });

        function getProductPricesForHospitals() {
            dataTable.ajax.reload();
        }

        function deleteProductPricesForHospital(productPricesForHospital) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _productPricesForHospitalsService.delete({
                            id: productPricesForHospital.id
                        }).done(function() {
                            getProductPricesForHospitals(true);
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
        $('#CreateNewProductPricesForHospitalButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _productPricesForHospitalsService
                .getProductPricesForHospitalsToExcel({
                    filter: $('#ProductPricesForHospitalsTableFilter').val(),
                    minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
                    maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
                    minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
                    maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
                    minPriceFilter: $('#MinPriceFilterId').val(),
                    maxPriceFilter: $('#MaxPriceFilterId').val(),
                    productNameFilter: $('#ProductNameFilterId').val(),
                    productCategoryNameFilter: $('#ProductCategoryNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditProductPricesForHospitalModalSaved', function() {
            getProductPricesForHospitals();
        });
        $('#GetProductPricesForHospitalsButton').click(function(e) {
            e.preventDefault();
            getProductPricesForHospitals();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getProductPricesForHospitals();
            }
        });
    });
})();