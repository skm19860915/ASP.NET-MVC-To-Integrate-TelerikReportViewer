function getContactData() {
    $.get(_getContactDataUrl, { ContactID: $('#ClientID').val() }, function (res) {
        if (res) {
            res.addr = res.addr || {};
            $('#Address1').val(res.addr.Address1);
            $('#Address2').val(res.addr.Address2);
            $('#City').val(res.addr.City);
            $('#State').val(res.addr.State);
            $('#Country').val(res.addr.Country);
            $('#Zip').val(res.addr.Zip);
            $('#JobPhone').val(res.phone && res.phone.Phone);
            $('#Email').val(res.email && res.email.Email);
            maskPlainValues();
        }
    });
}

$(function () {
    $('#StartDate').val($('#StartDate').attr('data-value'));
    $('.date-picker').removeAttr("data-val-date");

    $('#open-edit-popup').on('click', function () {
        $('#editableArea').load(_quoteEditViewUrl, function () {
            $('#StartDate').val($('#StartDate').attr('data-value'));
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
                        $alert.html('Quote saved.');
                        showAlert($alert, 'success');
                        var url = _quoteDetailUrl.replace("QUOTEID", res.quoteid);

                        //if (isNew) {
                        //    url = _quoteDetailUrl + res.quoteid;
                        //}

                        document.location = url;
                    }
                }
            });
        }
    });

    $('body').on('change', '#ClientID', function () {
        getContactData();
    });

    (function () {
        //if new quote page, then also load contact info
        if (!$('#ParentLeadID').val() && _isCreate) {
            getContactData();
        }
    })();

    $('body').on('click', '.add-new', function () {
        var $tab = $(this).closest('.tab-pane');
        var $selectCont = $tab.find('.select-new-cont');

        updateStageDdlItems();
        $selectCont.show();
    });

    $('body').on('change', '#area .select-new', function () {
        var $input = $('#area #area-name');

        if ($(this).val() === "") {
            $input.removeClass('input-validation-error').parent().show();
        } else {
            $input.parent().hide();
        }
    });

    var showStageDetails = function (StageDetails) {
        var $table = $('.stageDetails');
        var $templ = $table.find('.data-template');
        $table.find('.stageDetail').not('.data-template').remove();

        var total = 0;
        for (var i = StageDetails.length - 1; i >= 0; i--) {

            var data = StageDetails[i];

            var row = $templ.clone()
                .removeClass('data-template')
                .find('.subName').text(data.SubStage).end()
                .find('.costPerHour').val(data.Cost).end()
                .find('.pricePerHour').val(data.Price).end()
                .find('.perc').val((data.Factor * 100).toFixed(0)+"%").end()
                .find('.sellPerHour').text((data.Sell || 0).toFixed(2)).end()
                .find('.substage-id').val(data.SubID).end()
                .find('.item-id').val(data.StageMasterID).end()
                 .find('.projectstage-id').val(data.ViewID).end()
                .find('.index').val(i).end();
            $templ.after(row);
            total += (data.Sell || 0);           
        }       
        $('.totalPerHour').text(total.toFixed(2));
        initInputFormats();
    }
    var updateStageDdlItems = function () {
        var $select = $('#select-stages');
        var $stages = $('.stages .stage');
        $select.find('option').each(function (i, e) {
            $e = $(e);
            if ($stages.find('.item-id[value="' + $e.val() + '"]').length > 0) {
                $e.addClass('hide');
            } else {
                $e.removeClass('hide');
            }
        });
        $stages = $('.stages .stage');
        $select.val($select.find('option').not('.hide').eq(0).val() || '');//select first visible item
    }

    var updateCostCodes = function (codeCodes) {

        var $cost = $('#cost');
        var $templ = $cost.find('.data-template');
        $cost.find('.item-row').not('.data-template').remove();

        $.each(codeCodes, function () {
            var data = this;
            var row = $templ.clone().removeClass('data-template');

            row = row.find('.code').text(data.CostCode).end()
                .find('.desc').text('').end()
                .find('.pno').text(data.Project_ || "").end()
                .find('.row-id').val(data.ViewID).end()
                .find('.item-id').val(data.itemID).end();

            $templ.after(row);
        });
    }

    $('body').on('click', '.add-confirm', function () {
        var $tab = $(this).closest('.tab-pane');
        var $select = $tab.find('.select-new');

        if ($tab.find('.item-id[value="' + $select.val() + '"]').length > 0) return;

        var projectID = $('#QuoteID').val();
        var StageIDs = $select.val();
        if (projectID === "" || StageIDs === null) return;

        $tab = $(this).closest('.tab-pane');
        var $table = $('.stageDetails');

        var data = { StageIDs: StageIDs, ProjectID: projectID };

        $.post(_postStagesUrl, data, function (res) {

            if (res && res.status === 'success') {
                var row = $tab.find('.data-template.stages')
                    .clone()
                    .removeClass('data-template')
                    .find('.item-name')
                    .text($select.find('option:selected').text())
                    .end()
                    .find('.item-id')
                    .val($select.val())
                    .end();

                var $selectCont = $tab.find('.select-new-cont');

                $selectCont.after(row);
                row.click();
                $selectCont.hide();
                updateStageDdlItems();
                updateCostCodes(res.costCodes);
            }
        });
    });

    $('body').on('click', '.stages', function () {

        var projectID = $('#QuoteID').val();
        if (projectID === "") return;
        var $stageClicked = $(this).find('.stage');
        $('.stages .stage.active').removeClass('active');

        $.get(_getStageDetailsUrl, { ProjectID: $('#QuoteID').val(), StageID: $(this).find('.item-id').val() }, function (res) {


            if (res && res.data && res.data.length > 0) {
                showStageDetails(res.data);
            }
            //else {
            //    $templ.after($templ.clone().removeClass('data-template'));
            //}

            $stageClicked.addClass('active')
        });
    });

    $('body').on('click', '.add-stage-row', function () {
        var $table = $('.stageDetails');
        var $templ = $table.find('.data-template');
        $templ.after($templ.clone().removeClass('data-template'));
    });

    $('body').on('click', '.save-stage-details', function () {

        var data = [];
        var projectID = $('#QuoteID').val();
        var stageID = $('.stages .active .item-id').val();

        $('.stageDetails .stageDetail').each(function (i, e) {

            var $e = $(e);
            var index = $e.find('.index').val();
            data.Index = index;

            data[index] = {};
            data[index].Index = index;
            data[index].StageMasterID = stageID;
            data[index].ProjectID = projectID;
            data[index].SubStageID = $e.find('.substage-id').val();
            data[index].ProjectStageID = $e.find('.projectstage-id').val();
            data[index].SubName = $e.find('.subName').text();
            data[index].CostPerHour = $e.find('.costPerHour').val();
            data[index].PricePerHour = $e.find('.pricePerHour').val();
            data[index].Percent = $e.find('.perc').val();
            data[index].SellPerHour = $e.find('.sellPerHour').text();
        });
        console.log(data);
        $.post(_postStagesDetailsUrl, { Data: data }, function (res) {
            if (res && res.status !== 'NOT_SAVED') {
                showStageDetails(res.data);
            }
        });

    });

    (function () {

        var additionalTab = '.additional-tab';//selector
        var projectID = $('#QuoteID').val();



        var addCostRow = function (row, data) {
            return row.find('.code').text(data.CostCode).end()
                .find('.desc').text('').end()
                .find('.pno').text(data.Project_ || "").end()
                .find('.row-id').val(data.ViewID).end()
                .find('.item-id').val(data.itemID).end();
        }
        var addPayTypeRow = function (row, data) {

            if (data.Default === true) {
                $('#pay .item-row').not('.data-template').find('.default').text('No');
            }

            $('#paytype-default').prop('checked', false);

            return row.find('.type').text(data.Pay_Type).end()
                .find('.factor').text(data.Factor).end()
                .find('.row-id').val(data.ViewID).end()
                .find('.default').text((data.Default === true) ? "Yes" : "No").end()
                .find('.item-id').val(data.itemID).end();
        }
        var addTaxRow = function (row, data) {
            $('#taxes .add-item').hide();
            return row.find('.code').text(data.Tax).end()
                .find('.salesRate').text((data.SalesRate || 0).toFixed(4)).end()
                .find('.laborRate').val((data.LaborRate || 0).toFixed(4)).end()
                .find('.row-id').val(data.ViewID).end()
                .find('.item-id').val(data.itemID).end();
        }
        var addAreaRow = function (row, data) {
            $('#area-name').val('');
            $('#area-order').val('');
            updateAreaDivisionGrid(data, $('#area'));
        }
        var addDivisionRow = function (row, data) {
            $('#division-order').val('');
            updateAreaDivisionGrid(data, $('#divisions'));
        }
        var addCommRow = function (row, data) {
            return row.find('.user').text(data.User).end()
                .find('.row-id').val(data.ViewID).end()
                .find('.item-id').val(data.itemID).end();
        }

        //extra data to post
        var getPayTypeExtra = function () {
            var data = [];
            data.push({ name: 'IsDefault', value: $('#pay').find('#paytype-default').prop('checked') });
            return data;
        }
        var getAreaExtra = function () {
            var data = [];
            var $tab = $("#area");

            //order
            $input = $tab.find('#area-order');
            var order = $input.val();
            if (order === "") {
                var order = $tab.find('.item-row').not('.data-template').length + 1;
                $input.val(order);
            }
            if (!$.isNumeric(order)) {
                $input.addClass('input-validation-error');
                return "INVALID";
            } else {
                $input.removeClass('input-validation-error');
            }
            data.push({ name: 'Order', value: order });

            //name
            var $input = $tab.find('#area-name');
            if ($('#area .select-new').val() === "") {

                var name = $input.val();
                if (name === "") {
                    $input.addClass('input-validation-error');
                    return "INVALID";
                } else {
                    $input.removeClass('input-validation-error');
                }
                data.push({ name: 'CustomAreaName', value: name });
            }
            return data;
        }
        var getDivisionExtra = function () {
            var data = [];
            var $tab = $('#divisions');
            var $input = $tab.find('#division-order');

            var order = $input.val();

            if (order === "") {
                var order = $('#divisions').find('.item-row').not('.data-template').length + 1;
                $input.val(order);
            }

            if (!$.isNumeric(order)) {
                $input.addClass('input-validation-error');
                return "INVALID";
            } else {
                $input.removeClass('input-validation-error');
            }

            data.push({ name: 'Order', value: order });

            return data;
        }

        $('body').on('click', additionalTab + ' .add-item', function () {
            var $tab = $(this).closest(additionalTab); console.log($tab)
            $tab.find('.select-new-cont').show();
        });

        $('body').on('click', additionalTab + ' .update-item', function () {

            var $tab = $(this).closest(additionalTab);
            var $select = $tab.find('.select-new');

            var itemID = $select.val();

            var data = { ItemID: itemID, ProjectID: $('#QuoteID').val() };

            var extraDataCollector = ({
                'pay': getPayTypeExtra,
                'area': getAreaExtra,
                'divisions': getDivisionExtra
            })[$tab.attr('id')];

            if (extraDataCollector) {
                var extraData = extraDataCollector();
                if (extraData === "INVALID") return;
                if (extraData && extraData.length > 0) {
                    for (var i = 0; i < extraData.length; i++) {
                        var ed = extraData[i];
                        data[ed.name] = ed.value;
                    }
                }
            }

            $.post($tab.data('submit'), data, function (res) {
                if (res && res.status === "success") {

                    var newRow = $tab.find('.data-template')
                        .clone()
                        .removeClass('data-template');

                    var rowUpdater = (({
                        'cost': addCostRow,
                        'pay': addPayTypeRow,
                        'taxes': addTaxRow,
                        'area': addAreaRow,
                        'divisions': addDivisionRow,
                        'comm': addCommRow
                    })[$tab.attr('id')]);
                    newRow = rowUpdater(newRow, res.data);
                    if (newRow)
                        $tab.find('.data-template').before(newRow);

                    $tab.find('.select-new-cont').hide();
                }
            });
        });

        $('body').on('click', additionalTab + ' .item-row .delete', function () {

            var $tab = $(this).closest(additionalTab);
            var itemID = $(this).find('.row-id').val();
            var $row = $(this).closest('.item-row');

            $.post($tab.data('delete'), { ItemID: itemID, ProjectID: $('#QuoteID').val() }, function (res) {
                $row.remove();

                if ($tab.attr('Id') === 'taxes') {
                    $('#taxes .add-item').show();
                }

            });
        });


    })();
});

//Contacts
(function () {
    $(document).ready(function () {
        $(".nav-tabs li a").click(function () {
            $.cookie('saved-tab', $(this).attr("href").replace("#", ""));
        });
        if ($.cookie('saved-tab') != '') {
            $('.nav-tabs a[href="#' + $.cookie('saved-tab') + '"]').tab('show');
        }
    });
    var $addRow = $('.project-contact-add-row');

    $('.add-project-contact').on('click', function () {
        $addRow.show();
    });
    $('.cancel-project-contact').on('click', function () {
        $addRow.hide();
    });

    $('.save-project-contact').on('click', function () {
        var $sContact = $('#select-project-contact');
        var $sRel = $('#select-project-rel');
        var $templ = $('.add-contact-template');

        if ($sContact.val() === "") { $sContact.addClass('input-validation-error') } else { $sContact.removeClass('input-validation-error') }
        if ($sRel.val() === "") { $sRel.addClass('input-validation-error') } else { $sRel.removeClass('input-validation-error') }

        if ($sContact.val() === "" || $sRel.val() === "") return;

        var $optionContact = $sContact.find('option:selected');
        var $optionRel = $sRel.find('option:selected');

        $.post(_addQuoteContactUrl,
            { QuoteID: $('#QuoteID').val(), ContactID: $optionContact.val(), RelationshipID: $optionRel.val() },
            function (res) {

                if (res && res.status === 'success') {
                    var newRow = $templ.clone()
                        .find('.contact-name')
                        .text($optionContact.text())
                        .end()
                        .find('.contact-rel')
                        .text($optionRel.text())
                        .end()
                        .find('.contact-link')
                        .attr('href', $templ.find('.contact-link').attr('href').replace('CONTACTID', $optionContact.val()))
                        .end()
                        .removeClass('data-template add-contact-template');

                    $sContact.val('');
                    $sRel.val('');
                    $addRow.hide();

                    $addRow.before(newRow);
                    window.location.href = window.location.href;
                }
            })

    });

})();

//quote details 
function refreshParts() {
    //var $parts = $('#parts');
    //var $btn = $('.part-refresh');
    //$.post($btn.data('url'), { projectId: $('#QuoteID').val() }, function (res) {
    //    var $templ = $parts.find('.data-template');
    //    $parts.find('.grids .part').not('.data-template').remove();
    //    console.log($templ);
    //    for (var i = 0; i < res.length; i++) {
    //        var item = res[i];

    //        var row = $templ.clone().removeClass('data-template')
    //            .find('.part-id').val(item.ViewID).end()
    //            .find('.name').text(item.Item).end()
    //            .find('.area').text(item.Area).end()
    //            .find('.stage').text(item.Stage).end()
    //            .find('.qty').text(item.Qty.toFixed(2)).end()
    //            .find('.price').text(item.Price.toFixed(4)).end()
    //            .find('.hrs').text(item.Hrs.toFixed(2)).end()
    //            .find('.labor').text(item.Labor.toFixed(4)).end()
    //            .find('.installed').text(item.Installed.toFixed(4)).end();

    //        $templ.before(row);
    //    }
    //});
    window.location.href = window.location.href;
}
$(function () {

    var $parts = $('#parts');

    var getSelectedParts = function () {
        return $('#parts .part .partCheck:checked').siblings('.part-id').map(function () { return $(this).val() }).get();
    };

    //parts buttons
    $('.part-duplicate,.part-delete ').on('click', function () {
        var $btnClicked = $(this);
        $.post($btnClicked.data('url'), { ItemIDs: getSelectedParts(), projectId: $('#QuoteID').val() }, function (res) {
            if (res.status === true) {

                var msg = '';
                if ($btnClicked.hasClass('part-duplicate')) {
                    msg = 'Items duplicated!';
                }
                else if ($btnClicked.hasClass('part-delete')) {
                    msg = 'Items deleted!';
                }

                $('.part-alert').text(msg);
                showAlert($('.part-alert'), 'success', 5000);
                refreshParts();
            } else {
                $('.part-alert').text('Some error occurred! Please retry.');
                showAlert($('.part-alert'), 'danger', 5000);
            }
        });
    })

    $('.part-refresh').on('click', function () {
        refreshParts();
    })

    $('.part-replace').on('click', function () {
        var $btnClicked = $(this);
        var selectedItems = getSelectedParts();
        if (selectedItems.length == 0) {
            //TODO: show msg
            return;
        }

        document.location = $btnClicked.data('url').replace('REPLACEITEMS', selectedItems.join(','));
    })
});

//grid edit
$(function () {

    $('body').on('click', '.grids .item-row .edit', function (e) {
        e.preventDefault();
        var $edit = $(e.target);

        var $tr = $edit.closest('.item-row');
        var $fields = $tr.find('.editable');
        $fields.each(function () {
            var $field = $(this);
            var data = $field.text().slice(0,-1);
            var type = $field.data('type') || 'text';
            console.log(type);
            console.log(data);
            $field
                .data('originalValue', data)
                .html(function () {
                   
                    return type === "check" ?
                        $("<input class='edit-text' type='checkbox'>").prop('checked', (data == null || data == "No" || data == false) ? false : true) :
                        $("<input class='form-control edit-text' type='text'>").val(data);
                });
        });
        $tr.addClass('edit').find('.edit-container').hide().end().find('.save-container').show();
    });

    $('body').on('click', '.grids .item-row .cancel', function (e) {
        e.preventDefault();
        var $cancel = $(e.target);

        var $tr = $cancel.closest('.item-row');

        restoreRow($tr, true);
    });

    var restoreRow = function ($tr, resetValuesToOriginal) {

        var $fields = $tr.find('.editable');

        $fields.each(function () {
            var $field = $(this);
            var type = $field.data('type') || 'text';
            var data = resetValuesToOriginal ? $field.data('originalValue') :
                (type === 'check' ? ($field.find('.edit-text').prop('checked') == true ? 'Yes' : 'No') : $field.find('.edit-text').val());
            $field.text(data);
        });

        $tr.removeClass('edit').find('.edit-container').show().end().find('.save-container').hide();
    }

    $('body').on('click', '.grids .item-row .save', function (e) {
        console.log('saving...');
        e.preventDefault();
        var $save = $(e.target);
        var $tr = $save.closest('.item-row');
        var $fields = $tr.find('.editable');

        var dataToPost = {};
        var isValidationError = false;
        $fields.each(function () {
            var $field = $(this);
            var $input = $field.find('.edit-text');
            var name = $field.data('name');
            var type = $field.data('type') || 'text';
            var data = type === "check" ? $input.prop('checked') : $input.val();

            if (type === "text") {
                if (!$.isNumeric(data)) {
                    $input.addClass('input-validation-error');
                    isValidationError = true;
                } else {
                    $input.removeClass('input-validation-error');
                }
            }

            dataToPost[name] = data;
        });
        dataToPost['ProjectID'] = $('#QuoteID').val();
        if (isValidationError) {
            return;
        }

        dataToPost['itemID'] = $tr.find('.row-id').val();

        var $parentTab = $tr.closest('.additional-tab');
        $.post($parentTab.data('edit'), dataToPost, function (res) {
            if (res && res.status === 'success') {

                //if area or division grid, then sort by order
                var id = $parentTab.attr('id');
                if (id == 'area' || id == 'divisions') {
                    if (res.data) {
                        updateAreaDivisionGrid(res.data, $parentTab);
                    }
                } else {
                    restoreRow($tr, false);
                }
            }
        });
    });
});

function updateAreaDivisionGrid(GridData, $parentTab) {
    var $templ = $parentTab.find('.data-template');
    $parentTab.find('.item-row').not($templ).remove();

    for (var i = 0; i < GridData.length; i++) {
        var data = GridData[i];

        var row = $templ.clone().removeClass('data-template')
            .find('.area, .division').text(data.Area).end()
            .find('.order').text(data.DivisionOrder || data.AreaOrder || '').end()
            .find('.row-id').val(data.ViewID).end()
            .find('.item-id').val(data.itemID).end();

        $templ.before(row);
    }
}