(function() {
    $(function() {
        var _$productCategoriesTable = $('#ProductCategoriesTable');
        var _productCategoriesService = abp.services.app.productCategories;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProductCategories.Create'),
            edit: abp.auth.hasPermission('Pages.ProductCategories.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProductCategories.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ProductCategories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ProductCategories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductCategoryModal'
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
        var dataTable = _$productCategoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productCategoriesService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#ProductCategoriesTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        productCategoryNameFilter: $('#ProductCategoryNameFilterId').val(),
                        brandNameFilter: $('#BrandNameFilterId').val()
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
                                        id: data.record.productCategory.id
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
                                    deleteProductCategory(data.record.productCategory);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "productCategory.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "productCategory.description",
                    name: "description"
                },
                {
                    targets: 4,
                    data: "productCategoryName",
                    name: "productCategoryFk.name"
                },
                {
                    targets: 5,
                    data: "brandName",
                    name: "brandFk.name"
                }
            ]
        });

        function getProductCategories() {
            dataTable.ajax.reload();
        }

        function deleteProductCategory(productCategory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _productCategoriesService.delete({
                            id: productCategory.id
                        }).done(function() {
                            getProductCategories(true);
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
        $('#CreateNewProductCategoryButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _productCategoriesService
                .getProductCategoriesToExcel({
                    filter: $('#ProductCategoriesTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    productCategoryNameFilter: $('#ProductCategoryNameFilterId').val(),
                    brandNameFilter: $('#BrandNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditProductCategoryModalSaved', function() {
            getProductCategories();
        });
        $('#GetProductCategoriesButton').click(function(e) {
            e.preventDefault();
            getProductCategories();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getProductCategories();
            }
        });
    });
})();