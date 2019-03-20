function loaddropzone(ctrl_id, url, acceptedFiles,loaderimage)
{
    var divtext = $('#' + ctrl_id).text();
    var myDropzone = new Dropzone('#' + ctrl_id, {
        maxFiles: 100,
        url: url,
        acceptedFiles: acceptedFiles, //"image/jpg, image/jpeg, image/png, application/pdf, application/docx",
        addRemoveLinks: false,
        uploadMultiple: true,
        init: function () {
            this.on("processing", function () {
                $('#' + ctrl_id).html('<img src="' + loaderimage+'")" class="uploadloader" />');
            });
            this.on("success", function (file, data) {
                //......
                window.location.href = window.location.href;
            });
            this.on("removedfile", function (file) {
                //....
            });
            this.on("error", function (file, message) {
                this.removeAllFiles();
                //this.addFile(file);
                console.log(file);
                if (!file.accepted) {
                    console.log("Tj");
                    $('#' + ctrl_id).html("Wrong format");
                }
                else {
                    $('#' + ctrl_id).html(divtext);
                }
            });
            this.on("complete", function (data) {
                //var res = eval('(' + data.xhr.responseText + ')');
            });
        },
    });
}
function loadFilterFunctions() {
    $(document).on("change", '.checklists input[type="checkbox"]', function () {
        var checkids = [];
        $(this).parents('.checklists').find('input[type="checkbox"]:checked').each(function () {
            checkids.push($(this).val());
        });
        $(this).parents(".checklists").find('input[type="hidden"]').val(checkids.join(","));
        if ($(this).hasClass("SelectedTypes")) {
            getSubTypes(checkids);
        }
    });
    $(".clearleftfilter").click(function () {
        //$(this).parents(".leftfilters").find('input[type="hidden"]').val("");
        //$(this).parents(".leftfilters").find('input[type="checkbox"]').prop('checked', false);
        window.location.href = window.location.href;
    });
    $(".checklists").each(function () {
        var hdval = $(this).find('input[type="hidden"]').val().trim();
        if (hdval != "") {
            var selIds = hdval.split(",");
            for (var i = 0; i < selIds.length; i++) {
                $(this).find('input[type="checkbox"][value="' + selIds[i] + '"]').prop('checked', true);
            }
            if ($(this).find(".SelectedTypes").length > 0) {
                getSubTypes(selIds);
            }
        }
    });
}
function loadPagingForm() {
    var paging = $(".paging");
    var leftfilters = $(".leftfilters");
    if (paging.length > 0 && leftfilters.length > 0)
    {
        leftfilters.find('input[type="hidden"]').each(function () {
            paging.find("form").append($(this).clone());
        });
        leftfilters.find('input[type="text"]').each(function () {
            paging.find("form").append($(this).clone().attr("type","hidden"));
        });
    }
}