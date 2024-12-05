(function() {
    $(function() {
        var _$taxRatesTable = $('#TaxRatesTable');
        var _taxRatesService = abp.services.app.taxRates;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.TaxRates.Create'),
            edit: abp.auth.hasPermission('Pages.TaxRates.Edit'),
            'delete': abp.auth.hasPermission('Pages.TaxRates.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TaxRates/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TaxRates/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTaxRateModal'
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
        var dataTable = _$taxRatesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _taxRatesService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#TaxRatesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        minRateFilter: $('#MinRateFilterId').val(),
                        maxRateFilter: $('#MaxRateFilterId').val()
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
                                        id: data.record.taxRate.id
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
                                    deleteTaxRate(data.record.taxRate);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "taxRate.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "taxRate.rate",
                    name: "rate"
                }
            ]
        });

        function getTaxRates() {
            dataTable.ajax.reload();
        }

        function deleteTaxRate(taxRate) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _taxRatesService.delete({
                            id: taxRate.id
                        }).done(function() {
                            getTaxRates(true);
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
        $('#CreateNewTaxRateButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _taxRatesService
                .getTaxRatesToExcel({
                    filter: $('#TaxRatesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    minRateFilter: $('#MinRateFilterId').val(),
                    maxRateFilter: $('#MaxRateFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditTaxRateModalSaved', function() {
            getTaxRates();
        });
        $('#GetTaxRatesButton').click(function(e) {
            e.preventDefault();
            getTaxRates();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getTaxRates();
            }
        });
    });
})();