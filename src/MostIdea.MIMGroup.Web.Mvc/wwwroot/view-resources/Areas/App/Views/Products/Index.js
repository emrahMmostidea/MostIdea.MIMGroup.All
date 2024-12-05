(function() {
    $(function() {
        var _$productsTable = $('#ProductsTable');
        var _productsService = abp.services.app.products;
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Products.Create'),
            edit: abp.auth.hasPermission('Pages.Products.Edit'),
            'delete': abp.auth.hasPermission('Pages.Products.Delete')
        };
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Products/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductModal'
        });
        var _viewProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Products/ViewproductModal',
            modalClass: 'ViewProductModal'
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
        var dataTable = _$productsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productsService.getAll,
                inputFilter: function() {
                    return {
                        filter: $('#ProductsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        minPriceFilter: $('#MinPriceFilterId').val(),
                        maxPriceFilter: $('#MaxPriceFilterId').val(),
                        minQuantityFilter: $('#MinQuantityFilterId').val(),
                        maxQuantityFilter: $('#MaxQuantityFilterId').val(),
                        productCategoryNameFilter: $('#ProductCategoryNameFilterId').val(),
                        taxRateNameFilter: $('#TaxRateNameFilterId').val()
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
                                    _viewProductModal.open({
                                        id: data.record.product.id
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
                                        id: data.record.product.id
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
                                    deleteProduct(data.record.product);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "product.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "product.description",
                    name: "description"
                },
                {
                    targets: 4,
                    data: "product.price",
                    name: "price"
                },
                {
                    targets: 5,
                    data: "product.quantity",
                    name: "quantity"
                },
                {
                    targets: 6,
                    data: "productCategoryName",
                    name: "productCategoryFk.name"
                },
                {
                    targets: 7,
                    data: "taxRateName",
                    name: "taxRateFk.name"
                }
            ]
        });

        function getProducts() {
            dataTable.ajax.reload();
        }

        function deleteProduct(product) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function(isConfirmed) {
                    if (isConfirmed) {
                        _productsService.delete({
                            id: product.id
                        }).done(function() {
                            getProducts(true);
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
        $('#CreateNewProductButton').click(function() {
            _createOrEditModal.open();
        });
        $('#ExportToExcelButton').click(function() {
            _productsService
                .getProductsToExcel({
                    filter: $('#ProductsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    minPriceFilter: $('#MinPriceFilterId').val(),
                    maxPriceFilter: $('#MaxPriceFilterId').val(),
                    minQuantityFilter: $('#MinQuantityFilterId').val(),
                    maxQuantityFilter: $('#MaxQuantityFilterId').val(),
                    productCategoryNameFilter: $('#ProductCategoryNameFilterId').val(),
                    taxRateNameFilter: $('#TaxRateNameFilterId').val()
                })
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });
        abp.event.on('app.createOrEditProductModalSaved', function() {
            getProducts();
        });
        $('#GetProductsButton').click(function(e) {
            e.preventDefault();
            getProducts();
        });
        $(document).keypress(function(e) {
            if (e.which === 13) {
                getProducts();
            }
        });
    });
})();