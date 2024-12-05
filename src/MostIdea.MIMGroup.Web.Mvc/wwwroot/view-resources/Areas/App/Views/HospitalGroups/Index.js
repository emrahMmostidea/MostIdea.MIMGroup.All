(function() {
    $(function() {
        var _$hospitalGroupsTable = $('#HospitalGroupsTable');
        var _hospitalGroupsService = abp.services.app.hospitalGroups;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.HospitalGroups.Create'),
            edit: abp.auth.hasPermission('Pages.HospitalGroups.Edit'),
            'delete': abp.auth.hasPermission('Pages.HospitalGroups.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HospitalGroups/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HospitalGroups/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHospitalGroupModal'
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
        var dataTable = _$hospitalGroupsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hospitalGroupsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#HospitalGroupsTableFilter').val(),
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
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [{
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function() {
                                    return _permissions.edit;
                                },
                                action: function(data) {
                                    _createOrEditModal.open({
                                        id: data.record.hospitalGroup.id
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
                                    deleteHospitalGroup(data.record.hospitalGroup);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "hospitalGroup.name",
                    name: "name"
                }
            ]
        });

        function getHospitalGroups() {
            dataTable.ajax.reload();
        }

        function deleteHospitalGroup(hospitalGroup) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _hospitalGroupsService.delete({
                            id: hospitalGroup.id
                        }).done(function() {
                            getHospitalGroups(true);
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
        $('#CreateNewHospitalGroupButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _hospitalGroupsService
                .getHospitalGroupsToExcel({
                    filter: $('#HospitalGroupsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditHospitalGroupModalSaved', function() {
            getHospitalGroups();
        });
        $('#GetHospitalGroupsButton').click(function(e) {
            e.preventDefault();
            getHospitalGroups();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getHospitalGroups();
            }
        });
    });
})();