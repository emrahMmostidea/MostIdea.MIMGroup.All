(function() {
    $(function() {
        var _$brandsTable = $('#BrandsTable');
        var _brandsService = abp.services.app.brands;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Brands.Create'),
            edit: abp.auth.hasPermission('Pages.Brands.Edit'),
            'delete': abp.auth.hasPermission('Pages.Brands.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Brands/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Brands/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBrandModal'
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
        var dataTable = _$brandsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _brandsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#BrandsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val()
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
                                        id: data.record.brand.id
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
                                    deleteBrand(data.record.brand);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "brand.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "brand.description",
                    name: "description"
                }
            ]
        });

        function getBrands() {
            dataTable.ajax.reload();
        }

        function deleteBrand(brand) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _brandsService.delete({
                            id: brand.id
                        }).done(function() {
                            getBrands(true);
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
        $('#CreateNewBrandButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _brandsService
                .getBrandsToExcel({
                    filter: $('#BrandsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditBrandModalSaved', function() {
            getBrands();
        });
        $('#GetBrandsButton').click(function(e) {
            e.preventDefault();
            getBrands();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getBrands();
            }
        });
    });
})();