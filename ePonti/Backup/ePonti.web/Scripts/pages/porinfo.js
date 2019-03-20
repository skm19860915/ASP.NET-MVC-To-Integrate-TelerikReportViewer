$(function () {
    $('#RequestedDate').val($('#RequestedDate').attr('data-value'));
    //$('.date-picker').removeAttr("data-val-date");

    $('#open-edit-popup').on('click', function () {
        $('#editableArea').load($(this).data('url'), function () {
            $('.date-picker').each(function () { $(this).val($(this).attr('data-value')) });
            $('.date-picker').removeAttr("data-val-date");
            initDatePicker();
            onEditPopupOpened();
        });
    });

    $('body').on('click', '.create-por-page .save-main-form', function () {

        var $form = $('#main-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            var data = {
                RequestedDate: $('#RequestedDate').val(),
                ProjectID: queryString('projectid'),
                Items: queryString('items').split(','),
                ShipToID: $('#ShipToID').val()
            };

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: data,
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
                        $alert.html('POR created.');
                        showAlert($alert, 'success');
                        document.location = _previousUrl;
                    }
                }
            });
        }
    });

    $('body').on('click', '.edit-por-page .save-main-form', function () {

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
                        $alert.html('Purchase Order Request updated.');
                        showAlert($alert, 'success');
                        document.location = '/Sections/Procurement';
                    }
                }
            });
        }
    });

    $(function () {
        $('.date-picker').each(function () { $(this).val($(this).attr('data-value')) });
        $('.date-picker').removeAttr("data-val-date");
        initDatePicker();
        initNumberSpinner();
        onEditPopupOpened();
    })

    $('body').on('click', '.update-custody-page .save-main-form', function () {
       
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
                        $alert.html('Custody updated.');
                        showAlert($alert, 'success');
                        document.location = _previousUrl;
                    }
                }
            });
        }
    });

    $('.transfer').on('click', function () {
        var url = $(this).data('url');

        var items = $('#porItems').find('.check:checked').siblings('.item-id')
            .map(function (i, e) {return $(e).val() }).get().join(',');

        url = url.replace('ITEMIDS', items);

        document.location = url;
    });
});
