(function () {
    $(function () {

        var _$dynamicEnumItemsTable = $('#DynamicEnumItemsTable');
        var _dynamicEnumItemsService = abp.services.app.dynamicEnumItems;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DynamicEnumItems.Create'),
            edit: abp.auth.hasPermission('Pages.DynamicEnumItems.Edit'),
            'delete': abp.auth.hasPermission('Pages.DynamicEnumItems.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/DynamicEnumItems/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicEnumItems/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditDynamicEnumItemModal'
                });
                   

		 var _viewDynamicEnumItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicEnumItems/ViewdynamicEnumItemModal',
            modalClass: 'ViewDynamicEnumItemModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$dynamicEnumItemsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dynamicEnumItemsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DynamicEnumItemsTableFilter').val(),
					enumValueFilter: $('#EnumValueFilterId').val(),
					parentIdFilter: $('#ParentIdFilterId').val(),
					isAuthRestrictionFilter: $('#IsAuthRestrictionFilterId').val(),
					authorizedUsersFilter: $('#AuthorizedUsersFilterId').val(),
					dynamicEnumNameFilter: $('#DynamicEnumNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
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
                        items: [
						{
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewDynamicEnumItemModal.open({ id: data.record.dynamicEnumItem.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.dynamicEnumItem.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDynamicEnumItem(data.record.dynamicEnumItem);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "dynamicEnumItem.enumValue",
						 name: "enumValue"   
					},
					{
						targets: 3,
						 data: "dynamicEnumItem.parentId",
						 name: "parentId"   
					},
					{
						targets: 4,
						 data: "dynamicEnumItem.isAuthRestriction",
						 name: "isAuthRestriction"  ,
						render: function (isAuthRestriction) {
							if (isAuthRestriction) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 5,
						 data: "dynamicEnumItem.authorizedUsers",
						 name: "authorizedUsers"   
					},
					{
						targets: 6,
						 data: "dynamicEnumName" ,
						 name: "dynamicEnumFk.name" 
					}
            ]
        });

        function getDynamicEnumItems() {
            dataTable.ajax.reload();
        }

        function deleteDynamicEnumItem(dynamicEnumItem) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _dynamicEnumItemsService.delete({
                            id: dynamicEnumItem.id
                        }).done(function () {
                            getDynamicEnumItems(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewDynamicEnumItemButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _dynamicEnumItemsService
                .getDynamicEnumItemsToExcel({
				filter : $('#DynamicEnumItemsTableFilter').val(),
					enumValueFilter: $('#EnumValueFilterId').val(),
					parentIdFilter: $('#ParentIdFilterId').val(),
					isAuthRestrictionFilter: $('#IsAuthRestrictionFilterId').val(),
					authorizedUsersFilter: $('#AuthorizedUsersFilterId').val(),
					dynamicEnumNameFilter: $('#DynamicEnumNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDynamicEnumItemModalSaved', function () {
            getDynamicEnumItems();
        });

		$('#GetDynamicEnumItemsButton').click(function (e) {
            e.preventDefault();
            getDynamicEnumItems();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDynamicEnumItems();
		  }
		});
		
		
		
    });
})();
