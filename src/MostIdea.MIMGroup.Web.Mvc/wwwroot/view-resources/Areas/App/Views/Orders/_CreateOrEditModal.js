(function ($) {
    app.orderDetail = {};
    var _ordersService = abp.services.app.orders;
    var _orderItemsService = abp.services.app.orderItems;
    var _productCategoryService = abp.services.app.productCategories;
    var _productService = abp.services.app.products;
    var _addressInformationService = abp.services.app.addressInformations;
    var order;
    var addresses = [];

    $('.date-picker').datetimepicker({
        locale: abp.localization.currentLanguage.name,
        format: 'L'
    });


    orderItemStatus = [
        { id: 1, name: "Eklendi" },
        { id: 2, name: "Kullanıldı" },
        { id: 3, name: "İade Edildi" },
    ]

    $(".order-type").change(function () {
        if (_orderId !== '00000000-0000-0000-0000-000000000000') { return; }
        $(".order-type").removeClass("active");
        $(this).addClass("active");

        var type = $("input[type='radio']:checked").val();
        if (type == 1) {
            $("#patientInfo").show();
            getData();
        } else if (type == 2) {
            $("#patientInfo").show();
            getData();

        } else if (type == 3) {
            getData();
            $("#patientInfo").hide();
            $("#patientName").val(' ');
            $("#patientSurname").val(' ');
            $("#deliveryAddress").dxSelectBox("instance").option('value', addresses[0].addressInformation.id);
            $("#invoiceAddress").dxSelectBox("instance").option('value', addresses[0].addressInformation.id);
        }
    })

    getData();

    var _orderId = $("#orderId").val();

    var products = [];

    if (_orderId !== '00000000-0000-0000-0000-000000000000') {
        _ordersService.getOrderForEdit({ id: _orderId }).then(function (res) {
            order = res;
            $("#patientName").val(res.order.patientName);
            $("#patientSurname").val(res.order.patientSurname);
            $("#operationTime").val(moment(res.order.operationTime).format('DD.MM.YYYY'));
            $("#deliveryAddress").dxSelectBox("instance").option('value', res.order.deliveryAddressId);
            $("#invoiceAddress").dxSelectBox("instance").option('value', res.order.invoiceAddressId);

        })
    }



    function getData() {
        abp.ui.setBusy($("#order_detail"));

        var firstCategory;
        _productCategoryService.getAll({}).then(function (res) {
            $("#categories").html('');
            for (var i = 0; i < res.items.length; i++) {
                var cat = res.items[i];
                $("#categories").append('<li class="nav-item"><a class="nav-link text-active-primary py-5 me-6 category-tab-link ' + (i == 0 ? ' active' : ' ') + ' " data-toggle="tab" href="#cat_tab_pane_' + i + '" data-id="' + cat.productCategory.id + '" >' + cat.productCategory.name + '</a></li>');
            }
            firstCategory = res.items[0].productCategory.id;
            getProducts(firstCategory);


            abp.ui.clearBusy($("#order_detail"));
        })

        _productService.getAll({}).then(function (res) {
            products = res.items;
            initGrid();
        });


        _addressInformationService.getAll({}).then(function (res) {
            addresses = res.items;

            $('#deliveryAddress').dxSelectBox({
                dataSource: addresses,
                displayExpr: 'addressInformation.name',
                valueExpr: 'addressInformation.id',
                required: true,
                itemTemplate(data) {
                    return `<div class='custom-item'><div class='product-name'>${data.addressInformation.name}</div></div>`;
                },
            }).dxValidator({
                validationRules: [{
                    type: 'required',
                    message: 'Hasta adı boş bırakılamaz',
                }],
            });


            $('#invoiceAddress').dxSelectBox({
                dataSource: addresses,
                displayExpr: 'addressInformation.name',
                valueExpr: 'addressInformation.id',
                itemTemplate(data) {
                    return `<div class='custom-item'><div class='product-name'>${data.addressInformation.name}</div></div>`;
                },
            }).dxValidator({
                validationRules: [{
                    type: 'required',
                    message: 'Hasta soyadı boş bırakılamaz',
                }],
            });
        });

    }


    function initGrid() {
        _orderId = $("#orderId").val();
        _orderItemsService.getAll({ orderId: _orderId }).then(function (result) {

            $("#productsGrid").dxDataGrid({
                dataSource: result.items,
                keyExpr: "orderItem.id",
                border: true,
                editing: {
                    allowUpdating: true,
                    mode: "batch",

                },
                columns: [
                    {
                        dataField: "productImage",
                        caption: app.localize('Image'),
                        cellTemplate: function (element, info) {
                            element.append("<div class='symbol'><img class='symbol-label' src='" + info.data.productImage + "' /></div>");
                        },
                        allowEditing: false,
                        allowAddings: false
                    },
                    {
                        dataField: "productName",
                        valueField: "orderItem.productId",
                        caption: "Ürün",
                        allowEditing: false,
                        allowAddings: false,
                        lookup: {
                            dataSource: products,
                            displayExpr: "product.name"
                        }

                    },
                    {
                        dataField: "orderItem.amount",
                        dataType: "number",
                        caption: "Miktar"
                    },
                    {
                        dataField: "orderItem.price",
                        dataType: "number",
                        caption: "Birim Fiyat",
                        cellTemplate: function (element, info) {
                            element.append("<div>" + info.data.orderItem.price + " ₺</div>")
                        },
                        allowEditing: false,
                        allowAddings: false

                    },
                    {
                        caption: "Kdv",
                        dataField: "taxRate",
                        dataType: "number",
                        cellTemplate: function (element, info) {
                            element.append("<div>" + info.data.taxName + '<br>' + (info.data.orderItem.amount * info.data.orderItem.price) * (info.data.taxRate / 100) + ' ₺' + "</div>");
                        },
                        allowEditing: false,
                        allowAddings: false
                    },
                    {
                        dataField: "rowTotal",
                        caption: "Toplam",
                        calculateCellValue: function (rowData) {
                            return (rowData.orderItem.amount * rowData.orderItem.price);
                        },
                        allowEditing: false,
                        allowAddings: false
                    },
                    {
                        dataField: "orderItem.status",
                        caption: "Durum",
                        lookup: {
                            dataSource: orderItemStatus,
                            valueExpr: "id",
                            displayExpr: "name"
                        },
                        cellTemplate: function (element, info) {
                            let text = info.data.orderItem.status == 1 ? "Eklendi" : (info.data.orderItem.status == 2 ? "Kullanıldı" : "İade Edildi");
                            let color = info.data.orderItem.status == 1 ? "btn-primary" : (info.data.orderItem.status == 2 ? "btn-success" : "btn-warning");
                            element.append('<button class="btn btn-sm ' + color + '">' + text + '</button>');
                        }
                    }
                ],
                summary: {
                    totalItems: [{
                        column: 'orderItem.amount',
                        summaryType: 'count',
                    },
                    {
                        column: 'orderItem.rowTotal',
                        summaryType: 'sum',
                        valueFormat: 'currency',
                    }]
                },
                columnsAutoWidth: true,
                filterRow: { visible: true },
                headerFilter: { visible: true },
                allowColumnReordering: true,
                scrolling: {
                    rowRenderingMode: 'virtual',
                },
                paging: {
                    pageSize: 10,
                },
                pager: {
                    visible: true,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
            });
        })
    }


    $("#order_detail").on("click", ".category-tab-link", function () {
        abp.ui.setBusy($("#products"));

        getProducts($(this).data('id'));
    })

    $("#order_detail").on("click", ".add-cart", function () {


        if ($("#order_detail_form").valid()) {
            abp.ui.setBusy($("#products"));
            var productId = $(this).data("id");
            var orderNo = $("#orderNo").val();
            var orderId = $("#orderId").val();
            var invoiceId = $("#invoiceAddress").dxSelectBox('instance').option('value');
            var deliveryId = $("#deliveryAddress").dxSelectBox('instance').option('value');
            if (orderId === '00000000-0000-0000-0000-000000000000') {
                orderId = null;
            }
            var price = $(this).data("price");
            _orderItemsService.createOrEdit({ productId: productId, orderId: orderId, amount: 1, price: price, invoiceId: invoiceId, deliveryId: deliveryId }).then(function (res) {

                $("#orderId").val(res.orderId);
                abp.notify.success("Ürün başarıyla eklendi !")
                abp.ui.clearBusy($("#products"));
                initGrid();

                var refresh = window.location.protocol + "//" + window.location.host + window.location.pathname + '/' + res.orderId;
                window.history.pushState({ path: refresh }, '', refresh);
            });
        } else {
            abp.notify.warn("Gerekli alanları doldurun");
        }


    });

    $("#saveBtn").click(function () {
        if ($("#order_detail_form").valid()) {
            abp.ui.setBusy("#order_detail");


            _ordersService.getOrderForEdit({ id: _orderId }).then(function (res) {
                order = res;
                console.log(order);


                var orderId = $("#orderId").val();
                var orderNo = $("#orderNo").val();
                var orderType = $("input[type='radio']:checked").val();
                var deliveryAddressId = $("#deliveryAddress").dxSelectBox('instance').option('value');
                var invoiceAddressId = $("#invoiceAddress").dxSelectBox('instance').option('value');
                var orderItems = $("#productsGrid").dxDataGrid('instance').getDataSource().items();

                var payloadOrders = []
                for (var i = 0; i < orderItems.length; i++) {
                    payloadOrders.push(
                        {
                            orderId: orderItems[i].orderItem.orderId,
                            productId: orderItems[i].orderItem.productId,
                            amount: orderItems[i].orderItem.amount,
                            price: orderItems[i].orderItem.price,
                            status: orderItems[i].orderItem.status,
                            id: orderItems[i].orderItem.id
                        });
                }


                var payload = {
                    deliveryAddressId: deliveryAddressId,
                    invoiceAddressId: invoiceAddressId,
                    status: 1,
                    paymentType: 2,
                    orderItems: payloadOrders,
                    orderNo: orderNo,
                    orderType: orderType,
                    id: orderId,
                    doctorId: order.doctorId,
                    patientName: $("input[name=billing_order_address_1]").val(),
                    patientSurname: $("input[name=billing_order_address_2]").val(),
                    operationTime: $("#operationTime").val()
                };


                _ordersService.createOrEdit(payload).then(function (res) {
                    abp.notify.success("Sipariş başarıyla kaydedildi !")
                    abp.ui.clearBusy("#order_detail");


                });

            })
        } else {
            abp.notify.warn("Gerekli alanları doldurun");
        }
    });


    function getProducts(categoryId) {
        _productService.getAll({ categoryId: categoryId }).then(function (res) {
            products = res.items;
            $("#products").html('');
            for (var i = 0; i < res.items.length; i++) {
                var product = res.items[i].product;
                $("#products").append(`
                    <div class="col my-2">
                        <div class="d-flex align-items-center border border-dashed rounded p-3 bg-white">
                            <a href="#" class=" ">
                                <span class="symbol mr-3 symbol-50px">
                                    <img src="`+ product.image + `" class="img-responsive" />
                                </span>
                            </a>
                            <div class="ms-5" style="width: 230px;">
                                <a href="#" class="text-gray-800 text-hover-primary fs-5 fw-bolder">`+ product.name + `</a>
                                <div class="fw-bold fs-7">
                                    Fiyat: <span>`+ product.price + `</span>
                                </div>
                            </div>
                            <div class="ms-5">
                                <div class="add-cart btn btn-icon btn-icon-muted btn-active-light btn-active-color-primary w-30px h-30px w-md-40px h-md-40px" style="border: 2px solid #f5f8fa" data-price="`+ product.price + `" data-id="` + product.id + `" >
                                    <i class="flaticon2-shopping-cart-1"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
            }


            abp.ui.clearBusy($("#products"));
        })
    }


})(jQuery);