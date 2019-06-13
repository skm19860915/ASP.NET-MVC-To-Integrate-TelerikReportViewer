/* Set the width of the side navigation to 250px and the left margin of the page content to 250px */
function openNav() {
    document.getElementById("ePontiSideNav").style.width = "250px";
    document.getElementById("header").style.marginLeft = "250px";
    document.body.style.backgroundColor = "rgba(0,0,0,0.4)";
}

/* Set the width of the side navigation to 0 and the left margin of the page content to 0 */
function closeNav() {
    document.getElementById("ePontiSideNav").style.width = "0";
    document.getElementById("header").style.marginLeft = "0";
    document.body.style.backgroundColor = "white";
}

/* Displays the Editable Fields for Activities */
function openEdit() {    
    document.getElementById("editableArea").style.width = "100%";
    document.getElementById("viewArea").style.width = "0";
    document.body.style.backgroundColor = "white";

    var approvedArea = document.getElementById("approvedArea");
    if (approvedArea) {
        approvedArea.style.width = "0";
    }
}

/* This sets the Editable Fields to hide displaying the Viewable ones */
function closeEdit() {
    document.getElementById("editableArea").style.width = "0";
    //document.getElementById("approvedArea").style.width = "0";
    document.getElementById("viewArea").style.width = "100%";
    document.body.style.backgroundColor = "white";

    var approvedArea = document.getElementById("approvedArea");
    if (approvedArea) {
        approvedArea.style.width = "0";
    }
}

/* Displays the Editable Fields for Activities */
function openApprove() {
    document.getElementById("editableArea").style.width = "0";
    document.getElementById("approvedArea").style.width = "100%";
    document.getElementById("viewArea").style.width = "0";
    document.body.style.backgroundColor = "white";
}

/* This sets the Editable Fields to hide displaying the Viewable ones */
function closeApprove() {
    document.getElementById("editableArea").style.width = "0";
    document.getElementById("approvedArea").style.width = "0";
    document.getElementById("viewArea").style.width = "100%";
    document.body.style.backgroundColor = "white";
}

/* This is for the Uploading Account Logo */
var uploadfiles = document.querySelector('#fileinput');
if (uploadfiles) {
    uploadfiles.addEventListener('change', function () {
        var files = this.files;
        for (var i = 0; i < files.length; i++) {
            previewImage(this.files[i]);
        }

    }, false);
}

/* This is to display the Account Logo on the Upload page */
function previewImage(file) {
    var galleryId = "gallery";

    var gallery = document.getElementById(galleryId);
    var imageType = /image.*/;

    if (!file.type.match(imageType)) {
        throw "File Type must be an image";
    }

    var thumb = document.createElement("div");
    thumb.classList.add('thumbnail'); // Add the class thumbnail to the created div

    var img = document.createElement("img");
    img.file = file;
    thumb.appendChild(img);
    gallery.appendChild(thumb);

    // Using FileReader to display the image content
    var reader = new FileReader();
    reader.onload = (function (aImg) { return function (e) { aImg.src = e.target.result; }; })(img);
    reader.readAsDataURL(file);
}

/*To display the success/error message after form submit*/
function showAlert($el, type, duration) {
    //duration: pass -1 for permanent msg

    if (!$el) { return; }

    duration = duration || 5000;
    $el.removeClass('alert-success alert-danger');
    $el.addClass('alert-' + type);//type can be 'success', 'danger'
    $el.show();

    if (duration > 0) {
        setTimeout(function () { $el.hide() }, duration);
    }
    return true;
}

function fillDropdown($el, items, textField, valueField) {
    if (!$el || !items) return;

    $el.find('option').remove();
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        $el.append($('<option>').attr('value', item[valueField]).text(item[textField]));
    }

}

//disable ajax caching
$.ajaxSetup({ cache: false });

function goBack() { location.replace(document.referrer); }

function initDateTimePicker() {
    if (!$.fn.datetimepicker) { return; }
    $(".datetime-picker").not('.has-dtpicker').datetimepicker({
        format: 'yyyy-mm-dd HH:ii P',
        showMeridian: true,
        autoclose: true,
        todayBtn: true
    }).addClass('has-dtpicker');
}
initDateTimePicker();

function initDatePicker() {
    if (!$.fn.datetimepicker) { return; }
    $(".date-picker").not('.has-dpicker').datetimepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayBtn: true,
        minView: 2
    }).addClass('has-dpicker');
}
initDatePicker();

function initMultiDatePicker() {
    if (!$.fn.datepicker) { return; }
    $(".multi-datepicker").not('.has-mdpicker').datepicker({
        format: 'yyyy-mm-dd',
        multidate: true,
        multidateSeparator: ", "
    }).addClass('has-mdpicker');
}
initMultiDatePicker();

//this spins a number in 1/4 increments
function initNumberSpinner() {
    if (!$.fn.TouchSpin) { return; }
    var $el = $('.number-spinner');
    $el.not('.has-spinner').TouchSpin({
        verticalbuttons: true,
        min: 0,
        max: 99999,
        step: $el.data('step') || .25,
        decimals: $el.data('decimals') || 2
    }).addClass('has-spinner');
}
initNumberSpinner();

//this spins a mumber in whole increments
function initNumberSpinner1() {
    if (!$.fn.TouchSpin) { return; }
    var $el = $('.number-spinner1');
    $el.not('.has-spinner').TouchSpin({
        verticalbuttons: true,
        min: 0,
        max: 99999,
        step: $el.data('step') || 1,
        decimals: $el.data('decimals') || 0
    }).addClass('has-spinner');
}
initNumberSpinner1();

//validate hidden fields also
$.validator.setDefaults({ ignore: "" });

function queryString(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)", 'i'),
        results = regex.exec(location.search);
    return results === null ? "" : (function () {
        try { return decodeURIComponent(results[1].replace(/\+/g, " ")); }
        catch (ex) { return unescape(results[1].replace(/\+/g, " ")); }
    })();
}

//Phone format
function initPhoneFormat() {
    if (!$.fn.mask) { return; }
    $(".format-phone").not('.is-formatted-phone').mask('(000) 000-0000')
        .addClass('is-formatted-phone');
}

//percent format
function initPercFormat() {
    if (!$.fn.mask) { return; }
    $(".format-perc").not('.is-formatted-perc,.data-template,.data-template .format-perc').mask('0.00', { reverse: true })
        .addClass('is-formatted-perc');
}

//currency and other deicmal upto 2 decimal
function initDecimal2Format() {
    var formatField = function (input) {
        var $i = input;
        var val = $i.val();
        if (!val || val.length === 0 || isNaN(val)) { return; }
        $i.val(Number(val).toFixed("2"));
    }

    $(".format-decimal2").not('.is-formatted-decimal2,.data-template,.data-template .format-decimal2').on('change', function () {
        formatField($(this));
    })
    .addClass('is-formatted-decimal2')
    .each(function () { formatField($(this));});
}

function initInputFormats() {
    initPhoneFormat();
    initPercFormat();
    initDecimal2Format();
}
initInputFormats();

var onEditPopupOpened = function () {
    initInputFormats();
}

//to update the values already in plain format
var maskPlainValues = function () {
    var t = $('.format-phone');
    t.each(function () {
        var $e = $(this);

        if ($e.is('input') && $e.val() !== $e.masked()) {
            $e.val($e.masked());
        } else if ($e.is('div')) {
            if ($e.hasClass('format-phone')) { $e.mask('(000) 000-0000'); }
        }
    });
};
maskPlainValues();

jQuery.fn.highlight = function () {
    $(this).each(function () {
        var el = $(this);
        $("<div/>")
            .width(el.outerWidth())
            .height(el.outerHeight())
            .css({
                "position": "absolute",
                "left": el.offset().left,
                "top": el.offset().top,
                "background-color": "#ffff99",
                "opacity": ".7",
                "z-index": "9999999"
            }).appendTo('body').fadeOut(1000).queue(function () { $(this).remove(); });
    });
}

var showOverlayMsg = function ($el, msg, bgcolor, duration) {

    bgcolor = bgcolor || "#ffdede";
    duration = duration || 2000;

    var $msg = $("<div/>")
        .width($el.outerWidth())
        .height($el.outerHeight())
        .text(msg)
        .addClass('custom-font')
        .css({
            "position": "absolute",
            "left": $el.offset().left,
            "top": $el.offset().top,
            "background-color": bgcolor,
            "opacity": "1",
            "z-index": "9999999",
            "padding": "10px",
            "font-size": "14px"
        }).appendTo('body');
    setInterval(function () {
        $msg.fadeOut(1000).queue(function () { $(this).remove(); });
    }, duration);
}
