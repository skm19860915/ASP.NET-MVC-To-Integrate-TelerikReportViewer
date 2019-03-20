$(function () {

    function updateRequestItems(items) {
        var $grid = $('.requests');
        var $templ = $grid.find('.data-template');

        if (items) {
            $grid.find('.request').not($templ).remove();

            for (var i = 0; i < items.length; i++) {

                var $row = $templ.clone().removeClass('data-template');

                var item = items[i];

                //var itemInfoUrl = $row.find('.open-info').attr('href').replace('ITEMID', item.ItemID);
                $row.find('.request-item-id').val(item.ViewID).end()
                    //.find('.open-info').attr('href', itemInfoUrl).end()
                    .find('.item').text(item.Item).end()
                    .find('.por').text(item.Por_).end()
                    .find('.custody').text(item.Custody).end()
                    .find('.status').text(item.Status).end();

                $grid.append($row);
            }
        }

    }

    $('.create-delivery-page .save-main-form').on('click', function () {
        var resources = [];

        var data = {
            Items: queryString('items').split(','),
            ProjectID: queryString('projectid'),
            DeliveryDate: $('#DeliveryDate').val(),
            DeliveryTypeID: $('#DeliveryType').val()
        };

        var $alert = $('.main-form-alert');
        var prevUrl = $(this).data('previous');
        $.post($(this).data('save'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Enter correct data and retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Delivery Request saved.');
                showAlert($alert, 'success');

                document.location = prevUrl;
            }
        });
    });

    //$('.add-items-page .save-main-form').on('click', function () {
    //    var items = $('.items .item .check:checked').map(function (i, e) { return $(e).siblings('.itemid').val() }).get();

    //    var data = {
    //        Items: items,
    //        WoID: $('#woid').val()
    //    };

    //    var $alert = $('.main-form-alert');
    //    var prevUrl = $(this).data('previous');

    //    $.post($(this).data('additem'), data, function (res) {
    //        if (res.status === 'error') {
    //            var errors = 'Some error occurred. Please retry.'
    //            if (res.errors && res.errors.length > 0) {
    //                errors = res.errors.join('<br>');
    //            }
    //            $alert.html(errors);
    //            showAlert($alert, 'danger', -1);
    //        }
    //        else if (res.status === 'success') {
    //            $alert.html('Items added to work order.');
    //            showAlert($alert, 'success');

    //            document.location = prevUrl;
    //        }
    //    });

    //});

    $('.delivery-info-page .update-request').on('click', function () {

        var $alert = $('.delivery-update-alert');

        var requestItemIDs = $('.requests .request .check-request:checked').map(function (i, e) { return $(e).siblings('.request-item-id').val() }).get();

        if (requestItemIDs.length == 0)
        {
            $alert.html("Select the item(s) to update.");
            showAlert($alert, 'danger', -1);
            return;
        }

        var requestID = $('#RequestID').val()
        var data = {
            RequestID: $('#RequestID').val(),
            RequestItemIDs: requestItemIDs,
            CustodyID: $('#UpdateCustody').val(),
            Delivered: $('#UpdateDelivered').val(),
            StatusID: $('#UpdateStatus').val()
        };

        var $alert = $('.delivery-update-alert');

        $.post($(this).data('update'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Delivery request item updated.');
                showAlert($alert, 'success');

                updateRequestItems(res.items);
            }
        });
    });


    $('body').on('click','.edit-delivery-page .save-main-form', function () {

        var $form = $('#main-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: $form.serialize(),
                success: function (res) {
                    if (res.status === 'error') {
                        var errors = 'Enter correct data and retry.'
                        if (res.errors && res.errors.length > 0) {
                            errors = res.errors.join('<br>');
                        }
                        $alert.html(errors);
                        showAlert($alert, 'danger', -1);
                    }
                    else if (res.status === 'success') {
                        $alert.html('Delivery request updated.');
                        showAlert($alert, 'success');
                        document.location.reload();
                    }
                }
            });
        }
    });

    $('.delivery-info-page #open-edit-popup').on('click', function () {
        $('#editableArea').load($(this).data('url'), function () {
            $('#Delivery').val($('#Delivery').attr('data-value'));
            $('.date-picker').removeAttr("data-val-date");
            initDatePicker();
            onEditPopupOpened();
        });
    })

});