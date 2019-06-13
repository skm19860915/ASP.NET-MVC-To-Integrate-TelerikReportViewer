$(function () {

    function updateWoTasks(tasks) {
        var $grid = $('.tasks');
        var $templ = $grid.find('.data-template');

        if (tasks) {
            $grid.find('.task').not($templ).remove();

            for (var i = 0; i < tasks.length; i++) {

                var $row = $templ.clone().removeClass('data-template');

                var task = tasks[i];

                var itemInfoUrl = $row.find('.open-info').attr('href').replace('ITEMID', task.ItemID);
                $row.find('.assignmentId').val(task.ViewID).end()
                    .find('.open-info').attr('href', itemInfoUrl).end()
                    .find('.part').text(task.Part || task.Task).end()
                    .find('.estHours').text((task.ProjectHours||0).toFixed(4)).end()
                    .find('.actHours').text((task.ActHrs||0).toFixed(4)).end()
                    .find('.status').text(task.C_Comp).end();

                $grid.append($row);
            }
        }

    }

    $('.wo-assign .save-main-form').on('click', function () {
        var resources = [];

        var index = 0;
        $('.resources .resource').each(function (i, e) {

            var $e = $(e);
            //data.Index = index;

            if (!$e.find('.check').is(':checked'))
                return true;

            resources[index] = {};
            resources[index].Index = index;
            resources[index].SiteUserID = $e.find('.siteUserID').val();
            resources[index].Dates = $e.find('.dates').val();
            index++;
        });

        var data = {
            Items: queryString('items'),
            ProjectID: queryString('pid'),
            EstimatedHours: $('#EstimatedHours').val(),
            StatusID: $('#StatusID').val(),
            WorkOrderTypeID: queryString('wotype'),
            Resources: resources
        };

        var $alert = $('.main-form-alert');
        var prevUrl = $(this).data('previous');
        $.post($(this).data('save'), { Model: data }, function (res) {
            if (res.status === 'error') {
                var errors = 'Enter correct data and retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Work Order saved.');
                showAlert($alert, 'success');


                document.location = prevUrl;
            }
        });
    });

    $('.add-items-page .save-main-form').on('click', function () {
        var items = $('.items .item .check:checked').map(function (i, e) { return $(e).siblings('.itemid').val() }).get();

        var data = {
            Items: items,
            WoID: $('#woid').val()
        };

        var $alert = $('.main-form-alert');
        var prevUrl = $(this).data('previous');

        $.post($(this).data('additem'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Items added to work order.');
                showAlert($alert, 'success');

                document.location = prevUrl;
            }
        });

    });

    $('.wo-details-page .update-actuals').on('click', function () {
        var assignmentIds = $('.tasks .task .check:checked').map(function (i, e) { return $(e).siblings('.assignmentId').val() }).get();

        var data = {
            WoID: $('.woID').val(),
            woTypeID: $('.woTypeID').val(),
            AssignmentIDs: assignmentIds,
            ActualDate: $('#woActualDateId').val(),
            ActualHours: $('#woActualHoursId').val(),
            PercentCompleted: $('#woSetId').val()
        };

        var $alert = $('.actuals-alert');

        $.post($(this).data('updateactuals'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Actuals updated.');
                showAlert($alert, 'success');

                updateWoTasks(res.tasks);
            }
        });
    });

    $('.wo-details-page .remove-tasks').on('click', function () {
        var assignmentIds = $('.tasks .task .check:checked').map(function (i, e) { return $(e).siblings('.assignmentId').val() }).get();

        var data = {
            WoID: $('.woID').val(),
            AssignmentIDs: assignmentIds
        };

        var $alert = $('.tasks-alert');

        $.post($(this).data('removetasks'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Tasks removed.');
                showAlert($alert, 'success', 3000);

                updateWoTasks(res.tasks);
            }
        });
    });

    $('body').on('click', '.wo-edit-page .save-main-form', function () {
        debugger
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
                        $alert.html('Work Order updated.');
                        showAlert($alert, 'success');
                        document.location.reload();
                    }
                }
            });
        }
    });

    $('.wo-details-page #open-edit-popup').on('click', function () {
        $('#editableArea').load($(this).data('url'), function () {
            $('#Assigned').val($('#Assigned').attr('data-value'));
            $('.date-picker').removeAttr("data-val-date");
            initDatePicker();
            onEditPopupOpened();
        });
    })

});