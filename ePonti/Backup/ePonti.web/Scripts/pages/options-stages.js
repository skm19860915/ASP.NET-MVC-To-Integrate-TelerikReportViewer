 

$(function () {
     
    $('#MiscEquip').val(($('#MiscEquip').data('value')||"0")+'%');
    $('#MiscParts').val(($('#MiscParts').data('value') || "0") + '%');     
    $('body').on('click', '.save-main-form', function () {             
        var isNew = document.location.href.indexOf('/create') >= 0;

        var $form = $('#main-form');
        if ($form.valid()) {
            $('.validation-summary-errors').find('li').remove();
            var $alert = $('.main-form-alert');

            var stageMasterID = $('#StageMasterID').val();

            var stageData = {
                StageMasterID: stageMasterID,
                Order: $('#Order').val(),
                CostCodeId: $('#CostCodeId').val(),
                MiscEquip: $('#MiscEquip').val(),
                MiscParts: $('#MiscParts').val()
            };

            var stageSubDetails = [];
            $('.stageDetails .stageDetail').each(function (i, e) {
                
                var $e = $(e);
                var index = $e.find('.index').val();
                //data.Index = index;
                stageSubDetails[index] = {};
                stageSubDetails[index].Index = index;
                stageSubDetails[index].StageMasterID = $e.find('.stagemaster-id').val();
                stageSubDetails[index].SubStageID = $e.find('.substage-id').val();
                stageSubDetails[index].CostPerHour = $e.find('.costPerHour').val();
                stageSubDetails[index].PricePerHour = $e.find('.pricePerHour').val();
                debugger
               // var qqqq = $e.find('.perc').val().slice(0, -1);
                stageSubDetails[index].Percent = $e.find('.perc').val();
            });

            stageData.StageSubDetails = stageSubDetails;
            
            console.log($form.attr('action'))
            console.log(stageData)

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: stageData,
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
                        $alert.html('Stage info saved.');
                        showAlert($alert, 'success');
                        document.location = $('.close-page').eq(0).attr('href');
                    }
                }
            });
        }
    });
      
     
});
 