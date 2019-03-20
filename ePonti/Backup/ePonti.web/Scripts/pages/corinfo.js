$(function () {
    $(document).ready(function () {
        $(".nav-tabs li a").click(function () {
            $.cookie('saved-tab', $(this).attr("href").replace("#", ""));
        });
        if ($.cookie('saved-tab') != '') {
            $('.nav-tabs a[href="#' + $.cookie('saved-tab') + '"]').tab('show');
        }
    });
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
                        $alert.html('COR saved.');
                        showAlert($alert, 'success');

                        if (isNew) {
                            document.location.href = $('#cor-url').val().replace('NEWID',res.CorID);
                        } else {
                            document.location.reload();
                        }
                    }
                }
            });
        }
    });

    $('.update-scope').on('click', function () {

        var data = {
            CorID: $('#CorID').val(),
            Scope: $('#corScope').val()
        };
        
        $.post($(this).data('url'), data, function (res) {

            var $alert = $('.scope-alert');

            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Scope updated.');
                showAlert($alert, 'success', 3000);
            }
        });
    });


    $('.approve-cor').on('click', function () {

        var data = {
            CorID: $('#CorID').val(),
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


 