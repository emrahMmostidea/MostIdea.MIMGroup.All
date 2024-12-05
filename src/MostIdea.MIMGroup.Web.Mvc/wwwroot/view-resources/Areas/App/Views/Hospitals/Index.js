(function() {
    $(function() {
        var _$hospitalsTable = $('#HospitalsTable');
        var _hospitalsService = abp.services.app.hospitals;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Hospitals.Create'),
            edit: abp.auth.hasPermission('Pages.Hospitals.Edit'),
            'delete': abp.auth.hasPermission('Pages.Hospitals.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Hospitals/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Hospitals/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHospitalModal'
        });
        var _viewHospitalModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Hospitals/ViewhospitalModal',
            modalClass: 'ViewHospitalModal'
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
        var dataTable = _$hospitalsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hospitalsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#HospitalsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        hospitalGroupNameFilter: $('#HospitalGroupNameFilterId').val()
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
                        text: '<i class="fa fa-cog"></i> <span class="caret"></span>',
                        items: [{
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function(data) {
                                    _viewHospitalModal.open({
                                        id: data.record.hospital.id
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
                                        id: data.record.hospital.id
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
                                    deleteHospital(data.record.hospital);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "hospital.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "hospitalGroupName",
                    name: "hospitalGroupFk.name"
                }
            ]
        });

        function getHospitals() {
            dataTable.ajax.reload();
        }

        function deleteHospital(hospital) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _hospitalsService.delete({
                            id: hospital.id
                        }).done(function() {
                            getHospitals(true);
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
        $('#CreateNewHospitalButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _hospitalsService
                .getHospitalsToExcel({
                    filter: $('#HospitalsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    hospitalGroupNameFilter: $('#HospitalGroupNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditHospitalModalSaved', function() {
            getHospitals();
        });
        $('#GetHospitalsButton').click(function(e) {
            e.preventDefault();
            getHospitals();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getHospitals();
            }
        });
    });
})();