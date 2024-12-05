(function() {
    $(function() {
        var _$hospitalVsUsersTable = $('#HospitalVsUsersTable');
        var _hospitalVsUsersService = abp.services.app.hospitalVsUsers;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.HospitalVsUsers.Create'),
            edit: abp.auth.hasPermission('Pages.HospitalVsUsers.Edit'),
            'delete': abp.auth.hasPermission('Pages.HospitalVsUsers.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HospitalVsUsers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HospitalVsUsers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHospitalVsUserModal'
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
        var dataTable = _$hospitalVsUsersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hospitalVsUsersService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#HospitalVsUsersTableFilter').val(),
                        hospitalNameFilter: $('#HospitalNameFilterId').val(),
                        userNameFilter: $('#UserNameFilterId').val()
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
                                        id: data.record.hospitalVsUser.id
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
                                    deleteHospitalVsUser(data.record.hospitalVsUser);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "hospitalName",
                    name: "hospitalFk.name"
                },
                {
                    targets: 3,
                    data: "userName",
                    name: "userFk.name"
                }
            ]
        });

        function getHospitalVsUsers() {
            dataTable.ajax.reload();
        }

        function deleteHospitalVsUser(hospitalVsUser) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _hospitalVsUsersService.delete({
                            id: hospitalVsUser.id
                        }).done(function() {
                            getHospitalVsUsers(true);
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
        $('#CreateNewHospitalVsUserButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _hospitalVsUsersService
                .getHospitalVsUsersToExcel({
                    filter: $('#HospitalVsUsersTableFilter').val(),
                    hospitalNameFilter: $('#HospitalNameFilterId').val(),
                    userNameFilter: $('#UserNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditHospitalVsUserModalSaved', function() {
            getHospitalVsUsers();
        });
        $('#GetHospitalVsUsersButton').click(function(e) {
            e.preventDefault();
            getHospitalVsUsers();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getHospitalVsUsers();
            }
        });
    });
})();