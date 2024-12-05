(function () {
    $(function () {

        var _$dynamicEnumsTable = $('#DynamicEnumsTable');
        var _dynamicEnumsService = abp.services.app.dynamicEnums;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DynamicEnums.Create'),
            edit: abp.auth.hasPermission('Pages.DynamicEnums.Edit'),
            'delete': abp.auth.hasPermission('Pages.DynamicEnums.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/DynamicEnums/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicEnums/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditDynamicEnumModal'
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

        var dataTable = _$dynamicEnumsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dynamicEnumsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DynamicEnumsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					enumFileFilter: $('#EnumFileFilterId').val()
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
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.dynamicEnum.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDynamicEnum(data.record.dynamicEnum);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "dynamicEnum.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "dynamicEnum.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "dynamicEnum.enumFile",
						 name: "enumFile"   
					}
            ]
        });

        function getDynamicEnums() {
            dataTable.ajax.reload();
        }

        function deleteDynamicEnum(dynamicEnum) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _dynamicEnumsService.delete({
                            id: dynamicEnum.id
                        }).done(function () {
                            getDynamicEnums(true);
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

        $('#CreateNewDynamicEnumButton').click(function () {
            _createOrEditModal.open();
        });        

		

        abp.event.on('app.createOrEditDynamicEnumModalSaved', function () {
            getDynamicEnums();
        });

		$('#GetDynamicEnumsButton').click(function (e) {
            e.preventDefault();
            getDynamicEnums();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDynamicEnums();
		  }
		});
		
		
		
    });
})();
