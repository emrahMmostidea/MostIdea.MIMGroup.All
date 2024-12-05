(function() {
    $(function() {
        var _$assistanceVsUsersTable = $('#AssistanceVsUsersTable');
        var _assistanceVsUsersService = abp.services.app.assistanceVsUsers;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.AssistanceVsUsers.Create'),
            edit: abp.auth.hasPermission('Pages.AssistanceVsUsers.Edit'),
            'delete': abp.auth.hasPermission('Pages.AssistanceVsUsers.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AssistanceVsUsers/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AssistanceVsUsers/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAssistanceVsUserModal'
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
        var dataTable = _$assistanceVsUsersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _assistanceVsUsersService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#AssistanceVsUsersTableFilter').val(),
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
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function() {
                                    return _permissions.edit;
                                },
                                action: function(data) {
                                    _createOrEditModal.open({
                                        id: data.record.assistanceVsUser.id
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
                                    deleteAssistanceVsUser(data.record.assistanceVsUser);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "userName",
                    name: "assistanceFk.name"
                },
                {
                    targets: 3,
                    data: "userName2",
                    name: "doctorFk.name"
                }
            ]
        });

        function getAssistanceVsUsers() {
            dataTable.ajax.reload();
        }

        function deleteAssistanceVsUser(assistanceVsUser) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _assistanceVsUsersService.delete({
                            id: assistanceVsUser.id
                        }).done(function() {
                            getAssistanceVsUsers(true);
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
        $('#CreateNewAssistanceVsUserButton').click(function() {
            _createOrEditModal.open();
        });
        abp.event.on('app.createOrEditAssistanceVsUserModalSaved', function() {
            getAssistanceVsUsers();
        });
        $('#GetAssistanceVsUsersButton').click(function(e) {
            e.preventDefault();
            getAssistanceVsUsers();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getAssistanceVsUsers();
            }
        });
    });
})();