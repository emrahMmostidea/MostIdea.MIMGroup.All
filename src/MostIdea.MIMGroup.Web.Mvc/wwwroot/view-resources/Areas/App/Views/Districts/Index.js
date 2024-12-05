(function() {
    $(function() {
        var _$districtsTable = $('#DistrictsTable');
        var _districtsService = abp.services.app.districts;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Districts.Create'),
            edit: abp.auth.hasPermission('Pages.Districts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Districts.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Districts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Districts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDistrictModal'
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
        var dataTable = _$districtsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _districtsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#DistrictsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        cityNameFilter: $('#CityNameFilterId').val()
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
                                        id: data.record.district.id
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
                                    deleteDistrict(data.record.district);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "district.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "cityName",
                    name: "cityFk.name"
                }
            ]
        });

        function getDistricts() {
            dataTable.ajax.reload();
        }

        function deleteDistrict(district) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _districtsService.delete({
                            id: district.id
                        }).done(function() {
                            getDistricts(true);
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
        $('#CreateNewDistrictButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _districtsService
                .getDistrictsToExcel({
                    filter: $('#DistrictsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    cityNameFilter: $('#CityNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditDistrictModalSaved', function() {
            getDistricts();
        });
        $('#GetDistrictsButton').click(function(e) {
            e.preventDefault();
            getDistricts();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getDistricts();
            }
        });
    });
})();