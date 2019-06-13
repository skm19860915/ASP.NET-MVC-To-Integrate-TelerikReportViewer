$(function () {

    $('#Price_Margin').val(Number($('#Price_Margin').data('value')).toFixed(0) + '%')
    $('#Price_Markup').val(Number($('#Price_Markup').data('value')).toFixed(0) + '%')

    $('.update-project').on('click', function() {

        var data = {
            ItemID: $('#ItemID').val(),
            AreaID: $('#AreaID').val(),
            DivisionID: $('#DivisionID').val()
        };

        var $btn = $(this);
        var $alert = $btn.closest('.tab-pane').find('.main-form-alert');
        $.post($btn.data('url'), data, function (res) {
            if (res.status === 'error') {
                var errors = 'Some error occurred. Please retry.'
                if (res.errors && res.errors.length > 0) {
                    errors = res.errors.join('<br>');
                }
                $alert.html(errors);
                showAlert($alert, 'danger', -1);
            }
            else if (res.status === 'success') {
                $alert.html('Project Info updated.');
                showAlert($alert, 'success');
            }
        });
    });

    $('body').on('click', '.update-price', function () {

        var $tab = $(this).closest('.tab-pane');

        var $form = $('#price-form');
        if ($form.valid()) {
            $tab.find('.validation-summary-errors').find('li').remove();
            var $alert = $tab.find('.main-form-alert');

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
                        $alert.html('Item price updated.');
                        showAlert($alert, 'success');
                    }
                }
            });
        }
    });

    $('#labor-stage').on('change', function () {

        $.get($(this).data('url'), { ItemID: $('#ItemID').val(), ProjectID: $('#ProjectID').val(), StageID: $(this).val() }, function (res) {

            if (res && res.data && res.data.length > 0) {
                showStageDetails(res.data);
            }
        });
    });
    var showStageDetails = function (StageDetails) {
        
        var $table = $('.stageDetails');
        var $templ = $table.find('.data-template');
        $table.find('.stageDetail').not('.data-template').remove();

        var totalHours = 0;
        var totalSell = 0;
        for (var i = StageDetails.length-1; i >=0 ; i--) {

            var data = StageDetails[i];          
            var row = $templ.clone()
                .removeClass('data-template')
                .find('.subName').text(data.SubName).end()
                .find('.cost').text(data.LaborCost).end()
                .find('.price').text(data.LaborPrice).end()
                .find('.perc').text((data.LaborFactor * 100).toFixed(0)+'%').end()
                .find('.hours').text((data.Hours).toFixed(2)).end()
                .find('.sell').text((data.SellPrice || 0).toFixed(2)).end()
                .find('.substage-id').val(data.StageSubID).end()
                .find('.item-id').val(data.ViewID).end()
                .find('.index').val(i).end()
                .find('.projectstage-id').val(data.StageID).end()
                .find('.labor-tax').text((data.LaborTax || 0).toFixed(2)).end()
            .find('.stagemaster-id').val(data.StageMasterID).end()
            
            $templ.after(row);

            totalSell += (data.SellPrice || 0);
            totalHours += (data.Hours || 0);
        }
        $('.subtotal-sell').text(totalSell.toFixed(2));
        $('.subtotal-hours').text(totalHours.toFixed(2));
        $('.total-sell').text((totalSell + (Number($('.labor-tax').text()) || 0)).toFixed(2));
        initInputFormats();
    }

    $('.update-labor').on('click', function () {

        var $tab = $(this).closest('.tab-pane');

        var data = [];
        var projectID = $('#ProjectID').val();
        var stageID = $('#labor-stage').val();
        var UnitHours = $('#labor-unit-hours').val();
     
        $('.stageDetails .stageDetail').each(function (i, e) {

            var $e = $(e);
            var index = $e.find('.index').val();
            data.Index = index;

            data[index] = {};            
            data[index].Index = index;
            // data[index].StageMasterID = stageID;
            data[index].StageMasterID = $e.find('.stagemaster-id').val();
            data[index].ProjectID = projectID;
            data[index].SubStageID = $e.find('.substage-id').val();
            data[index].ItemID = $e.find('.item-id').val();
            //data[index].SubName = $e.find('.subName').text();
            data[index].CostPerHour = $e.find('.cost').text();
            data[index].PricePerHour = $e.find('.price').text();
            data[index].Percent = $e.find('.perc').text();
            data[index].SellPerHour = $e.find('.sell').text();
            data[index].ProjectStageID = $e.find('.projectstage-id').val();            
            data[index].Hours = $e.find('.hours').text();
            data[index].UnitHours = UnitHours;
        });

        var $alert = $tab.find('.main-form-alert');
        $.post($(this).data('url'), { Data: data }, function (res) {
            if (res && res.status !== 'NOT_SAVED') {
                showStageDetails(res.data);               
                $alert.html("Labor details updated.");
                showAlert($alert, 'success');
                 document.location = 'javascript:history.go(0)'
            }
            else {

                $alert.html("Some error occurred. Please retry.");
                showAlert($alert, 'danger', -1);
            }
        });

    });

    //load grid on page-load
    $('#labor-stage').trigger('change');

});