(function() {
    $(function() {
        var _$addressInformationsTable = $('#AddressInformationsTable');
        var _addressInformationsService = abp.services.app.addressInformations;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.AddressInformations.Create'),
            edit: abp.auth.hasPermission('Pages.AddressInformations.Edit'),
            'delete': abp.auth.hasPermission('Pages.AddressInformations.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AddressInformations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AddressInformations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAddressInformationModal'
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
        var dataTable = _$addressInformationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _addressInformationsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#AddressInformationsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val()
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
                        text: '<i class="fa fa-cog"></i><span class="caret"></span>',
                        items: [{
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function() {
                                    return _permissions.edit;
                                },
                                action: function(data) {
                                    _createOrEditModal.open({
                                        id: data.record.addressInformation.id
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
                                    deleteAddressInformation(data.record.addressInformation);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "addressInformation.name",
                    name: "name"
                }
            ]
        });

        function getAddressInformations() {
            dataTable.ajax.reload();
        }

        function deleteAddressInformation(addressInformation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _addressInformationsService.delete({
                            id: addressInformation.id
                        }).done(function() {
                            getAddressInformations(true);
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
        $('#CreateNewAddressInformationButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _addressInformationsService
                .getAddressInformationsToExcel({
                    filter: $('#AddressInformationsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditAddressInformationModalSaved', function() {
            getAddressInformations();
        });
        $('#GetAddressInformationsButton').click(function(e) {
            e.preventDefault();
            getAddressInformations();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getAddressInformations();
            }
        });
    });
})();