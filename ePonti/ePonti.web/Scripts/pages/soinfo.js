$(function () {
    $('#Date').val($('#Date').attr('data-value'));
    $('.date-picker').removeAttr("data-val-date");

    $('#open-edit-popup').on('click', function () {
        $('#editableArea').load($(this).data('url'), function () {
            $('#Date').val($('#Date').attr('data-value'));
            $('.date-picker').removeAttr("data-val-date");
            initDatePicker();
            onEditPopupOpened();
        });
    })

    $('body').on('click', '.save-main-form', function () {

        var isNew = document.location.href.indexOf('/create') >= 0;

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
                        $alert.html('Sales Order saved.');
                        showAlert($alert, 'success');

                        if (isNew) {
                            document.location.href = $('#so-url').val().replace('NEWID',res.CorID);
                        } else {
                            document.location.reload();
                        }
                    }
                }
            });
        }
    });

    $('.approve-so').on('click', function () {

        var data = {
            SOID: $('#SOID').val(),
            ProjectID: $('#ProjectID').val()
        };

        $.post($(this).data('url'), data, function (res) {

            var $alert = $('.approve-alert');

            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Approved!');
                showAlert($alert, 'success', 3000);

                document.location.reload();
            }
        });
    });


});


 