var optionHelpers = {

    getNewRow: function (gridId) {
        var $grid = $('#' + gridId);
        var $templ = $grid.find('.data-template');
        return {
            $templ: $templ,
            $row: $templ.clone().removeClass('data-template')
        };
    },

    makeRowEditable: function ($row) {

        var $tr = $row;
        var $fields = $tr.find('td').not('.actions');
        $fields.each(function () {
            var $field = $(this);
            var data = $field.text();
            var type = $field.data('type') || 'text';
            $field
                .data('originalValue', data)
                .html(function () {
                    return type === "check" ?
                        $("<input class='edit-text' type='checkbox'>").prop('checked', (data === null || data === "No" || data === false) ? false : true) :
                        $("<input class='form-control edit-text' type='text'>").val(data);
                });
        });
    },

    restoreRow: function ($tr, resetValuesToOriginal) {

        //if new row, just remove it
        if ($tr.hasClass('new')) {
            $tr.remove();
            return;
        }

        //restore
        var $fields = $tr.find('td').not('.actions');
        $fields.each(function () {
            var $field = $(this);
            var type = $field.data('type') || 'text';
            var data = resetValuesToOriginal ? $field.data('originalValue') :
                (type === 'check' ? ($field.find('.edit-text').prop('checked') === true ? 'Yes' : 'No') : $field.find('.edit-text').val());
            $field.text(data);
        });
        $tr.removeClass('edit');
    },

    scrollToMid: function ($el) {
        $('html, body').animate({
            scrollTop: $el.offset().top - ($(window).height() - $el.outerHeight(true)) / 2
        }, 200);
        $el.find('.edit-text').first().focus();
    }

    , handleSpecialCasesOnSave: function ($tr, $parentTab) {
        optionHelpers.sortOrderedGridOnSave($tr, $parentTab);
        optionHelpers.handleSingleItemGridOnSave($parentTab);
        optionHelpers.updateViewDetailsUrl($tr);
    }

    , sortOrderedGridOnSave: function ($tr, $parentTab) {

        if ($tr.find('.order').length > 0) {
            var rowOrder = Number($tr.find('.order').text());
            console.log('rowOrder', rowOrder);
            if ($.isNumeric(rowOrder)) {
                var $allRows = $parentTab.find('.item-row').not('.data-template');
                var isInserted = false;
                $allRows.each(function () {
                    var $currentRow = $(this);
                    var currentOrder = Number($currentRow.find('.order').text());
                    if (rowOrder < currentOrder) {
                        $currentRow.before($tr);
                        isInserted = true;
                        return false;
                    }
                });

                if (!isInserted) {
                    $allRows.last().after($tr);
                }
            }
        }
    }

    , handleSingleItemGridOnSave: function ($parentTab) {
        console.log($parentTab);
        var gridID = $parentTab.attr('id');
        var addButton = $('[data-grid=' + gridID + ']');
        if (addButton.data('singleitem') === true) {
            addButton.hide();
        }
    }

    , updateViewDetailsUrl: function ($tr) {

        var $a = $tr.find('.view-details');
        if ($a) {
            var itemID = $tr.find('.item-id').val();
            $a.attr('href', $a.attr('href').replace('ITEMID', itemID));
        }
    }
};

$(function () {

    //add item
    $('.add-item').on('click', function () {
        var $el = optionHelpers.getNewRow($(this).data('grid'));
        var $row = $el.$row;
        $row.addClass('edit new');
        optionHelpers.makeRowEditable($row);

        //if contains order, then insert new order value
        if ($row.find('.order').length > 0) {
            var $orderField = $el.$templ.closest('.grids').find('.item-row').not('.data-template').last().find('.order');
            var nextOrder = 0;
            if ($orderField.closest('.item-row').hasClass('edit')) {
                nextOrder = $orderField.find('.edit-text').val();
            } else {
                nextOrder = $orderField.text();
            }
            $row.find('.order').find('.edit-text').val((Number(nextOrder)||0) + 1);
        }

        $el.$templ.before($row);
        optionHelpers.scrollToMid($row);
    });

    //edit
    $('body').on('click', '.item-row .edit', function (e) {
        console.log('edit...');
        var $edit = $(e.target);

        var $tr = $edit.closest('.item-row');
        $tr.addClass('edit');
        optionHelpers.makeRowEditable($tr);
        
    });

    //cancel items
    $('body').on('click', '.item-row .cancel', function (e) {
        console.log('cancel...');
        e.preventDefault();
        var $cancel = $(e.target);
        var $tr = $cancel.closest('.item-row');
        optionHelpers.restoreRow($tr, true);
    });

    //save
    $('body').on('click', '.item-row .save', function (e) {
        console.log('saving...');
        e.preventDefault();
        var $save = $(e.target);
        var $tr = $save.closest('.item-row');
        var $fields = $tr.find('td').not('.actions');

        var dataToPost = {};
        var isValidationError = false;
        $fields.each(function () {
            var $field = $(this);            
            var $input = $field.find('.edit-text');            
            var name = $field.data('name');
            var type = $field.data('type') || 'text';            
            var data = type === "check" ? $input.prop('checked') : $input.val();

            if (type === "text") {
                if ($field.hasClass('order')) {
                    if (!$.isNumeric(data)) {
                        $input.addClass('input-validation-error');
                        isValidationError = true;
                    } else {
                        $input.removeClass('input-validation-error');
                    }
                }
                if ($field.hasClass('name')) {
                    if (data == null || data.trim().length == 0) {
                        $input.addClass('input-validation-error');
                        isValidationError = true;
                    } else {
                        $input.removeClass('input-validation-error');
                    }
                }
            }
            dataToPost[name] = data;
        });
        if (isValidationError) {
            return;
        }

        dataToPost['itemID'] = $tr.find('.item-id').val();
        dataToPost['ViewID'] = $tr.find('.item-id').val();
        var $parentTab = $tr.closest('.grids');
        console.log('dataToPost:', dataToPost);        
        $.post($parentTab.data('edit'), dataToPost, function (res) {
                if (res) {
                if (res.status === 'success') {
                    $tr.find('.item-id').val(res.itemID);
                    $tr.removeClass('new');
                    optionHelpers.restoreRow($tr, false);
                    optionHelpers.handleSpecialCasesOnSave($tr, $parentTab);
                    $tr.highlight();
                }
            }
        });
    });

    //delete
    $('body').on('click', '.item-row .delete', function (e) {
        console.log('deleting...');
        e.preventDefault();
        var $delete = $(e.target);
        var $tr = $delete.closest('.item-row');

        //if new item, then remove directly
        if ($tr.hasClass('new')) {
            $tr.remove();
            return;
        }

        var itemID = $tr.find('.item-id').val();
        console.log(itemID);
        $.post($tr.closest('.grids').data('delete'), { itemID: itemID }, function (res) {
            if (res) {

                if (res.status === 'success') {
                    $tr.remove();
                }
                else if (res.status === 'IN_USE') {
                    showOverlayMsg($tr, 'Option in use. Cannot be deleted.');
                }
            }
        });
    });
});