(function() {
    $(function() {
        var _$salesConsultantsTable = $('#SalesConsultantsTable');
        var _salesConsultantsService = abp.services.app.salesConsultants;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.SalesConsultants.Create'),
            edit: abp.auth.hasPermission('Pages.SalesConsultants.Edit'),
            'delete': abp.auth.hasPermission('Pages.SalesConsultants.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SalesConsultants/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SalesConsultants/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSalesConsultantModal'
        });
        var _viewSalesConsultantModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SalesConsultants/ViewsalesConsultantModal',
            modalClass: 'ViewSalesConsultantModal'
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
        var dataTable = _$salesConsultantsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _salesConsultantsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#SalesConsultantsTableFilter').val(),
                        userNameFilter: $('#UserNameFilterId').val(),
                        userName2Filter: $('#UserName2FilterId').val()
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
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function(data) {
                                    _viewSalesConsultantModal.open({
                                        id: data.record.salesConsultant.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function() {
                                    return _permissions.edit;
                                },
                                action: function(data) {
                                    _createOrEditModal.open({
                                        id: data.record.salesConsultant.id
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
                                    deleteSalesConsultant(data.record.salesConsultant);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "userName",
                    name: "salesConsultantFk.name"
                },
                {
                    targets: 3,
                    data: "userName2",
                    name: "doctorFk.name"
                }
            ]
        });

        function getSalesConsultants() {
            dataTable.ajax.reload();
        }

        function deleteSalesConsultant(salesConsultant) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _salesConsultantsService.delete({
                            id: salesConsultant.id
                        }).done(function() {
                            getSalesConsultants(true);
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
        $('#CreateNewSalesConsultantButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _salesConsultantsService
                .getSalesConsultantsToExcel({
                    filter: $('#SalesConsultantsTableFilter').val(),
                    userNameFilter: $('#UserNameFilterId').val(),
                    userName2Filter: $('#UserName2FilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditSalesConsultantModalSaved', function() {
            getSalesConsultants();
        });
        $('#GetSalesConsultantsButton').click(function(e) {
            e.preventDefault();
            getSalesConsultants();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getSalesConsultants();
            }
        });
    });
})();