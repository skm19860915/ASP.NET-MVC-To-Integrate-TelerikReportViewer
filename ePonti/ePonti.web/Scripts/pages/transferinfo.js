$(function () {
    $('#Date').val($('#Date').attr('data-value'));
    $('.date-picker').removeAttr("data-val-date");

    $('#from-tab-btn').on('click', function () {
        var geturl = $(this).attr('data-fromprojectsurl');
        $.get(geturl, {
            ProjectID: $('#toProjectID').val(),
            ToMasterItemIDs: $('.to-items-grid .check:checked').map(function (i, e) { return $(e).data('masteritemid') }).get().join(',')
        },
            function (Projects) {
                var $ddl = $('#fromProjectID');
                $ddl.find('option').remove();
                $ddl.append($('<option>').text("Select").val("0"));
                for (var i = 0; i < Projects.Projects.length; i++) {

                    var p = Projects.Projects[i];

                    $ddl.append($('<option>').text(p.Project).val(p.ProjectID));
                }
                var $grid = $('.from-items-grid');
                $grid.find('.from-item-row').not('.data-template').remove();
            });

    });
    
    $('#fromProjectID').on('change', function () {
     
        $.get($(this).attr('data-itemsurl'),
            {
                ProjectID: $('#fromProjectID').val(),
                ToMasterItemIDs: $('.to-items-grid .check:checked').map(function (i, e) { return $(e).data('masteritemid') }).get().join(',')
            },
            function (Items) {               
                if (Items)
                {
                    var $grid = $('.from-items-grid');
                    var $templ = $grid.find('.data-template');
                    $grid.find('.from-item-row').not('.data-template').remove();                                     
                    for (var i = 0; i < Items.Items.length; i++)
                    {
                        var item = Items.Items[i];
                        console.log(item);
                       
                        var $row = $templ.clone()
                        .removeClass('data-template');
                        console.log($row);
                        $row
                        .find('.check').attr('data-itemid', item.FromItemID).attr('data-masteritemid', item.FromItemID).end()
                        .find('.item').text(item.Item).end()
                        .find('.qty').text(item.Qty).end()
                        .find('.por').text(item.FromPorID).end()
                        .find('.custody').text(item.FromCustodyLocationID);
                       
                        $templ.after($row);
                    }
                  
                }

            });
    });

    $('.save-main-form').on('click', function () {
        var data = {
            PorID: queryString('porid'),
            Date: $('#Date').val(),
            TransferNumber: $('#TransferNumber').val(),
            Reason: $('#Reason').val(),
            ToProjectItemIDs: $('.to-items-grid .check:checked').map(function (i, e) { return $(e).data('itemid') }).get(),
            FromProjectID: $('#fromProjectID').val(),
            FromProjectItemIDs: $('.from-items-grid .check:checked').map(function (i, e) { return $(e).data('itemid') }).get(),
            PorItemIDs: queryString('PorItemIDs').split(',')
        };

        var $alert = $('.main-form-alert');

        $.ajax({
            url: $(this).data('submit'),
            type: 'POST',
            data: data,
            success: function (res) {
                if (res.status === 'error') {
                    var errors = 'Some error occurred. Please retry.'
                    if (res.errors && res.errors.length > 0) {
                        errors = res.errors.join('<br>');
                    }
                    $alert.html(errors);
                    showAlert($alert, 'danger', -1);
                }
                else if (res.status === 'success') {
                    $alert.html('Transfer Request saved.');
                    showAlert($alert, 'success');
                    var url = $('#previous-url').val();

                    document.location = url;
                }
            }
        });

    });

});
