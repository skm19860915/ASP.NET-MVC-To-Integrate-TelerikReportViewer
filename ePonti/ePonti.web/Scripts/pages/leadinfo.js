function getContactData() {
    $.get(_getContactDataUrl, { ContactID: $('#ClientID').val() }, function (res) {
        if (res) {
            res.addr = res.addr || {};
            $('#Address1').val( res.addr.Address1);
            $('#Address2').val(res.addr.Address2);
            $('#City').val(res.addr.City);
            $('#State').val(res.addr.State);
            $('#Country').val(res.addr.Country);
            $('#Zip').val(res.addr.Zip);

            $('#Phone').val(res.phone && res.phone.Phone);
            $('#Email').val(res.email && res.email.Email);
            maskPlainValues();
        }
    });
}
function addCurrentProjComm() {

    var $select = $('#select-comm');
    var $templ = $('.add-comm-template');
    var $last = $('.last-row');

    var selected = $select.find('option:selected');
    if (selected.val() != "" && $('.comm-contacts .comm-contact .comm-contact-id[value=' + selected.val() + ']').length == 0) {
        $last.before(
            $templ
                .clone()
                .removeClass('data-template add-comm-template')
                .find('.name')
                .text(selected.text())
                .end()
                .find('.comm-contact-id')
                .val(selected.val())
                .end());
        $select.val('');
    }
}

$('body').on('click', '.add-comm', function () { addCurrentProjComm(); });
$('body').on('click', '.remove-comm', function () {
    $(this).closest('tr').remove();
});

(function () {

    var $addRow = $('.lead-contact-add-row');

    $('.add-lead-contact').on('click', function () {
        $addRow.show();
    });
    $('.cancel-lead-contact').on('click', function () {
        $addRow.hide();
    });

    $('.save-lead-contact').on('click', function () {
        var $sContact = $('#select-lead-contact');
        var $sRel = $('#select-lead-rel');
        var $templ = $('.add-contact-template');

        if ($sContact.val() === "") { $sContact.addClass('input-validation-error') } else { $sContact.removeClass('input-validation-error') }
        if ($sRel.val() === "") { $sRel.addClass('input-validation-error') } else { $sRel.removeClass('input-validation-error') }

        if ($sContact.val() === "" || $sRel.val() === "") return;

        var $optionContact = $sContact.find('option:selected');
        var $optionRel = $sRel.find('option:selected');

        $.post(_addLeadContactUrl,
            { LeadID:$('#leadID').val(), ContactID:$optionContact.val(), RelationshipID:$optionRel.val()},
            function (res) {

            if (res && res.status == 'success') {
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
            }
        })

    });

    $('body').on('change', '#ClientID', function () {
        getContactData();
    });
    (function () {
        //if new quote page, then also load contact info
        if (!$('#LeadID').val() && _isCreate) {
            getContactData();
        }
    })();


})();
