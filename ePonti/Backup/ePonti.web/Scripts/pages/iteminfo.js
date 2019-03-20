$(function () {
    $('#Margin').val(Number($('#Margin').data('value')).toFixed(0) + '%')
    $('#Markup').val(Number($('#Markup').data('value')).toFixed(0) + '%')   
    $(document).on('click', '.editItem', function () {

        var $form = $('#main-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: $form.serialize(),
                success: function (res) {
                    console.log(res);
                    if (res.status == 'error') {

                        var errors = 'Enter correct data and retry.'
                        if (res.errors && res.errors.length > 0) {
                            errors = res.errors.join('<br>');
                        }
                        $alert.html(errors);
                        showAlert($alert, 'danger', -1);
                    }
                    else if (res.status = 'success') {
                        $alert.html('Note saved.');
                        showAlert($alert, 'success');
                        document.location = 'javascript:history.go(0)'
                    }
                }
            });
        }
    })

    $(document).on('click', '.update-labor', function () {

        var $form = $('#main-formlbr');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');
            var q = $('.drpstage').val();
            $('#flagval').val("1");
            getdata($form, $alert, q);
        }
    })

    $(document).on('change', '.drpstage', function () {
        $('#flagval').val("2");
        var $form = $('#main-formlbr');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');
            var q = $(this).val();
            getdata($form, $alert, q);
        }
    })
    function getdata($form, $alert, q) {
        var data = { stage: q };
        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            success: function (res) {

                if (res.status == undefined && res != null) {
                    var labordiv = document.getElementById("laborDetails");
                    labordiv.innerHTML = res;
                }
                else if (res.status == 'error') {
                    var errors = 'Enter correct data and retry.'
                    if (res.errors && res.errors.length > 0) {
                        errors = res.errors.join('<br>');
                    }
                    $alert.html(errors);
                    showAlert($alert, 'danger', -1);
                }
                else if (res.status = 'success') {
                    $alert.html('Note saved.');
                    showAlert($alert, 'success');
                    document.location = '/Common/items/details/' + res.ViewID;
                }

            }
        });
    }

    $(document).on('click', '.update-price-item', function () {

        var $form = $('#update-price-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: $form.serialize(),
                success: function (res) {
                    console.log(res);
                    if (res.status == 'error') {

                        var errors = 'Enter correct data and retry.'
                        if (res.errors && res.errors.length > 0) {
                            errors = res.errors.join('<br>');
                        }
                        $alert.html(errors);
                        showAlert($alert, 'danger', -1);
                    }
                    else if (res.status = 'success') {
                        $alert.html('Note saved.');
                        showAlert($alert, 'success');
                        document.location = 'javascript:history.go(0)'
                    }
                }
            });
        }
    })

    $(document).on('click', '.update-desc', function () {

        var $form = $('#update-desc-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: $form.serialize(),
                success: function (res) {
                    console.log(res);
                    if (res.status == 'error') {

                        var errors = 'Enter correct data and retry.'
                        if (res.errors && res.errors.length > 0) {
                            errors = res.errors.join('<br>');
                        }
                        $alert.html(errors);
                        showAlert($alert, 'danger', -1);
                    }
                    else if (res.status = 'success') {
                        $alert.html('Note saved.');
                        showAlert($alert, 'success');
                        document.location = 'javascript:history.go(0)'
                    }
                }
            });
        }
    })
  
    $('#labor-stage').on('change', function () {

        $.get($(this).data('url'), { ItemID: $('#ItemID').val(), ProjectID: $('#ProjectID').val(), StageID: $(this).val() }, function (res) {

            if (res && res.data && res.data.length > 0) {
                showStageDetails(res.data);
            }
        });
    });

    var showStageDetails = function (StageDetails) {
        console.log(StageDetails);
        var $table = $('.stageDetails');
        var $templ = $table.find('.data-template');
        $table.find('.stageDetail').not('.data-template').remove();

        var totalHours = 0;
        var totalSell = 0;
        for (var i = 0; i < StageDetails.length; i++) {

            var data = StageDetails[i];

            var row = $templ.clone()
                .removeClass('data-template')
                .find('.subName').text(data.SubName).end()
                .find('.cost').val(data.LaborCost).end()
                .find('.price').val(data.LaborPrice).end()
                .find('.perc').val((data.LaborFactor * 100).toFixed(0)).end()
                .find('.hours').val((data.Hours).toFixed(2)).end()
                .find('.sell').val((data.SellPrice || 0).toFixed(2)).end()
                .find('.substage-id').val(data.StageSubID).end()
                .find('.item-id').val(data.ViewID).end()
                .find('.index').val(i).end()
                .find('.projectstage-id').val(data.StageID).end()
            .find('.stagemaster-id').val(data.StageMasterID).end()

            $templ.after(row);

            totalSell += (data.SellPrice || 0);
            totalHours += (data.Hours || 0);
        }
        $('.subtotal-sell').text(totalSell.toFixed(2));
        $('.subtotal-hours').text(totalHours.toFixed(2));
        $('.total-sell').text((totalSell + (Number($('.labor-tax').val()) || 0)).toFixed(2));
        initInputFormats();
    }

    //$('.update-labor').on('click', function () {

    //    var $tab = $(this).closest('.tab-pane');

    //    var data = [];
    //    var projectID = $('#ProjectID').val();
    //    var stageID = $('#labor-stage').val();
    //    var UnitHours = $('#labor-unit-hours').val();
    //    console.log("labor-unit-hours=" + UnitHours);
    //    $('.stageDetails .stageDetail').each(function (i, e) {

    //        var $e = $(e);
    //        var index = $e.find('.index').val();
    //        data.Index = index;

    //        data[index] = {};
    //        data[index].Index = index;
    //        // data[index].StageMasterID = stageID;
    //        data[index].StageMasterID = $e.find('.stagemaster-id').val();
    //        data[index].ProjectID = projectID;
    //        data[index].SubStageID = $e.find('.substage-id').val();
    //        data[index].ItemID = $e.find('.item-id').val();
    //        //data[index].SubName = $e.find('.subName').text();
    //        data[index].CostPerHour = $e.find('.cost').val();
    //        data[index].PricePerHour = $e.find('.price').val();
    //        data[index].Percent = $e.find('.perc').val();
    //        data[index].SellPerHour = $e.find('.sell').val();
    //        data[index].ProjectStageID = $e.find('.projectstage-id').val();
    //        data[index].Hours = $e.find('.hours').val();
    //    });

    //    var $alert = $tab.find('.main-form-alert');
    //    $.post($(this).data('url'), { Data: data }, function (res) {
    //        if (res && res.status !== 'NOT_SAVED') {
    //            showStageDetails(res.data);

    //            $alert.html("Labor details updated.");
    //            showAlert($alert, 'success');
    //        }
    //        else {

    //            $alert.html("Some error occurred. Please retry.");
    //            showAlert($alert, 'danger', -1);
    //        }
    //    });

    //});

    //load grid on page-load
    $('#labor-stage').trigger('change');

});