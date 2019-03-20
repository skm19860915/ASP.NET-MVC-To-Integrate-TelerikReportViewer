$('#PayrollPeriodItemID').on('change', function () {
   
    $.get($(this).attr('data-getresources'), {
        PayrollPeriodItemID: $(this).val(),
        },
        function (Resources) {
            
            var $ddl = $('#ResourceID');
            $ddl.find('option').remove();
            $ddl.append($('<option>').text("Select").val("0"));
            for (var i = 0; i < Resources.Resources.length; i++) {

                var p = Resources.Resources[i];

                $ddl.append($('<option>').text(p.Resource).val(p.ViewID));
            }
        });
    });
    
$('#ResourceID').on('change', function () {  
    var q = $('#PayrollPeriodItemID').val();
    if (q != "") {
        $.post('/Sections/Timekeeper/Index',
            {
                PayrollPeriodItemIDProject: null,
                PayrollPeriodItemIDEmp: null,
                PayrollPeriodItemID: $('#PayrollPeriodItemID').val(),
                ResourceID: $(this).val()
            },
            function (res) {
                if (res.status == undefined && res != null) {
                    var timesheetdetails = document.getElementById("timesheetdetails");
                    timesheetdetails.innerHTML = res;
                }

            });
    }
    });

$('#PayrollPeriodItemIDProject').on('change', function () {   
    $.post($(this).attr('data-gettimekeeper'),
        {
            PayrollPeriodItemID:null,
            PayrollPeriodItemIDProject: $('#PayrollPeriodItemIDProject').val(),
            PayrollPeriodItemIDEmp:null
          //  ResourceID: $(this).val()
        },
        function (res) {           
            if (res.status == undefined && res != null) {
                var projectdetails = document.getElementById("projectdetails");
                projectdetails.innerHTML = res;
            }          
        });

});
$('#projectId').on('change', function () {
    $.post($(this).attr('data-gettimekeeper'),
        {
            projectId:$(this).val(),
            PayrollPeriodItemID: null,
            PayrollPeriodItemIDProject: $('#PayrollPeriodItemIDProject').val(),
            PayrollPeriodItemIDEmp: null
            //  ResourceID: $(this).val()
        },
        function (res) {
            if (res.status == undefined && res != null) {
                var projectdetails = document.getElementById("projectdetails");
                projectdetails.innerHTML = res;
            }
        });

});

$('#PayrollPeriodItemIDEmp').on('change', function () {
    $.post($(this).attr('data-gettimekeeper'),
        {
            PayrollPeriodItemID:null,
            PayrollPeriodItemIDProject: null,          
            PayrollPeriodItemIDEmp: $('#PayrollPeriodItemIDEmp').val(),
            //  ResourceID: $(this).val()
        },
        function (res) {          
            if (res.status == undefined && res != null) {
                var employeedetails = document.getElementById("employeedetails");
                employeedetails.innerHTML = res;
            }
        });
});

$(document).on('click', '.InsertPaySubmit', function () {   
        var data = [];
        var sign = $('#submitSigned').val();
        var state = $('#submitState').prop('checked');
        var j = 0;
        $('.timekeeper-grids .timesheet-item-row').each(function (i, e) {           
            var $e = $(e);            
            var index = $e.find('.check:checked').siblings('.index').val();
            var PayrollID = $e.find('.check:checked').siblings('.PayrollID').val();
            var Hours = $e.find('.check:checked').closest('td').siblings('.Hours').text();
            if (index != null) {
                data.Index = j;
                data[j] = {};
                data[j].Index = index;
                data[j].PayrollID = PayrollID;
                data[j].SubmittedHours = Hours;
                data[j].SubmitSigned = sign;
                data[j].Hours = Hours;
                j = j + 1;
            }
        });
                     
        var $alert = $('.main-form-alert');
        if (state == true) {
           // var data = { SubmitSigned: sign, SubmittedHours: total, PayrollID: PayrollPeriodItemID };            
            $.post($(this).data('url'), { Data: data }, function (res) {
                console.log(res);
                if (res.res == 'success') {
                   
                    // showStageDetails(res.data);
                    $alert.html("Data Submitted.");
                    showAlert($alert, 'success');
                   $('#submitSigned').val("");
                   $('#submitState').val("FALSE");
                    var q = $('#PayrollPeriodItemID').val();
                    if (q != "") {
                        $.post('/Sections/Timekeeper/Index',
                            {
                                PayrollPeriodItemIDProject: null,
                                PayrollPeriodItemIDEmp: null,
                                PayrollPeriodItemID: $('#PayrollPeriodItemID').val(),
                                ResourceID: $('#ResourceID').val()
                            },
                            function (res) {
                                if (res.status == undefined && res != null) {
                                    var timesheetdetails = document.getElementById("timesheetdetails");
                                    timesheetdetails.innerHTML = res;
                                }

                            });
                    }
                }
                else {

                    $alert.html("Some error occurred. Please retry.");
                    showAlert($alert, 'danger', -1);
                }
            });
        }
        else
        {
            $alert.html("Please check accurate state checkbox before submitting.");
            showAlert($alert, 'danger', -1);
        }

    });

    $('#ProjectIDProject').on('change', function () {

        $.get($(this).attr('data-getprojects'),
            {
                PayrollPeriodID: $('#PayrollPeriodIDProject').val(),
                ProjectID: $(this).val()
            },
            function (Timekeeper) {
                if (Timekeeper) {
                    var $grid = $('.projects-grids');
                    var $templ = $grid.find('.data-template');
                    $grid.find('.timesheet-item-row').not('.data-template').remove();
                    for (var i = 0; i < Timekeeper.Timekeeper.length; i++) {
                        var item = Timekeeper.Timekeeper[i];
                        console.log(item);

                        var $row = $templ.clone()
                        .removeClass('data-template');
                        console.log($row);
                        $row
                        .find('.check').attr('data-itemid', item.FromItemID).attr('data-masteritemid', item.FromItemID).end()
                        .find('.item').text(item.Item).end()
                        .find('.qty').text(item.Qty).end()
                        .find('.por').text(item.FromPorID).end()
                        .find('.custody').text(item.FromCustodyLocationID);

                        $templ.after($row);
                    }
                  
                }

            });
    });

    $(document).on('click', '.project-Approve', function (e) {

        e.preventDefault();
        var PayrollPeriodID = $('#PayrollPeriodItemIDProject').val();
        var ProjectID = $('#projectId').val();
                var data = [];
        var j = 0;
        $('.projects-grids .projects-item-row').each(function (i, e) {         
            var $e = $(e);          
            var index = $e.find('.checkproject:checked').siblings('.index1').val();
            var PayrollID = $e.find('.checkproject:checked').siblings('.PayrollID1').val();
            if (index != null) {
                data.Index = j;
                data[j] = {};
                data[j].Index = index;
                data[j].PayrollID = PayrollID;
                j = j + 1;
            }
        });

        var $alert = $('.main-form-alert1');
       
            // var data = { SubmitSigned: sign, SubmittedHours: total, PayrollID: PayrollPeriodItemID };            
            $.post($(this).data('url'), { Data: data }, function (res) {             
                if (res.status == 'success') {

                    // showStageDetails(res.data);
                    $alert.html("Projects Approved.");
                    showAlert($alert, 'success');
                    $.post('/Sections/Timekeeper/Index',
        {
            projectId: ProjectID,
            PayrollPeriodItemID: null,
            PayrollPeriodItemIDProject: PayrollPeriodID,
            PayrollPeriodItemIDEmp: null
            //  ResourceID: $(this).val()
        },
        function (res) {
            if (res.status == undefined && res != null) {                
                var projectdetails = document.getElementById("projectdetails");
                projectdetails.innerHTML = res;
            }
        });
                   // document.location = "/Sections/Timekeeper";
                }
                else {

                    $alert.html("Some error occurred. Please retry.");
                    showAlert($alert, 'danger', -1);
                }
            });
      
    });

    $(document).on('click', '.createtimeits', function (e) {
        e.preventDefault();
        var url = $(this).data('url');
        //var projectId = $('.timekeeper-grids').find('.check:checked').siblings('.project-id')
        //    .map(function (i, e) {
        //        return $(e).val()
        //    }).get().join(',');

        //var pid = projectId.split(',');
        var pid = $('#PayrollPeriodItemID').val();
        url = url.replace('PAYROLLPERIODITEMID', pid);
        document.location = url;
    });

    $(document).on('click', '.payinfo', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var PayrollID = $(this).data('pid');
        var pid = null;
        if ($('#PayrollPeriodItemID').val() != "") { pid = $('#PayrollPeriodItemID').val(); }
        if ($('#PayrollPeriodItemIDEmp').val() != "") { pid = $('#PayrollPeriodItemIDEmp').val(); }
        if ($('#PayrollPeriodItemIDProject').val() != "") { pid = $('#PayrollPeriodItemIDProject').val(); }
        url = url.replace('PERIOD', pid);
        url = url.replace('PAYROLLID', PayrollID);
        document.location = url;
    });

     $(document).on('click','.project-reason', function (e) {
        e.preventDefault();        
        var url = $(this).data('url');
        var PayrollID1 = $('.timekeeper-grids').find('.check:checked').siblings('.PayrollID1').val();
            //.map(function (i, e) {
            //    return $(e).val()
            //}).get().join(',');

        //var pid = projectId.split(',');
       // var pid = $('#PayrollPeriodItemID').val();
        url = url.replace('PayrollID', PayrollID1);
        document.location = url;
    });

     $(document).on('click', '.empinfo', function (e) {
        e.preventDefault();      
        var url = $(this).attr('href');            
        var pid = $('#PayrollPeriodItemIDEmp').val();
        url = url.replace('PERIOD', pid);       
        document.location = url;
    });

     $(document).on('click', '.approve', function (e) {
         var pid = $('#PayrollPeriodItemIDEmp').val();
        e.preventDefault(); 
        var data = [];
        var j = 0;
        $('.employee-grids .employee-item-row').each(function (i, e) {        
            var $e = $(e);         
            var PayrollID = $e.find('.employeecheck:checked').siblings('.PayrollID2').val();
            var ViewID = $e.find('.employeecheck:checked').siblings('.ViewID').val();
            if (PayrollID != null) {
                data.Index = j;
                data[j] = {};
                data[j].Index = j;
                data[j].PayrollID = PayrollID;
                data[j].ItemPayPeriodId = pid;
                data[j].ViewID = ViewID;
                j = j + 1;
            }
        });

        var $alert = $('.main-form-alert2');

        // var data = { SubmitSigned: sign, SubmittedHours: total, PayrollID: PayrollPeriodItemID };            
        $.post($(this).data('url'), { Data: data }, function (res) {
           
            if (res.status == 'success') {

                // showStageDetails(res.data);
                $alert.html("Item Approved.");
                showAlert($alert, 'success');
                $.post('/Sections/Timekeeper/Index',
    {
        PayrollPeriodItemID: null,
        PayrollPeriodItemIDProject: null,
        PayrollPeriodItemIDEmp: $('#PayrollPeriodItemIDEmp').val(),
        //  ResourceID: $(this).val()
    },
    function (res) {
        if (res.status == undefined && res != null) {
            var employeedetails = document.getElementById("employeedetails");
            employeedetails.innerHTML = res;
        }
    });
               // document.location = "/Sections/Timekeeper";
            }
            else {

                $alert.html("Some error occurred. Please retry.");
                showAlert($alert, 'danger', -1);
            }
        });
                
     });
   
     var url = document.URL;
     var pid = null, rid = null, projectid = null;
     if (url.includes("?"))
     {
   
         pid  = getParameterByName("pid", url);
         var a = pid;
         if (a != "") {
             var tab = getParameterByName("tab", url);
             if (tab == "timesheet") {               
                 rid = getParameterByName("rid", url);
                 $.post('/Sections/Timekeeper/Index',
              {
                  PayrollPeriodItemIDProject: null,
                  PayrollPeriodItemIDEmp: null,
                  PayrollPeriodItemID: pid,
                  ResourceID: rid
              },
              function (res) {
                  if (res.status == undefined && res != null) {  
                     
                      var timesheetdetails = document.getElementById("timesheetdetails");
                      timesheetdetails.innerHTML = res;               
                      $('li').removeClass('active');
                      $("a[href='#" + tab + "']").parent().addClass('active');
                      $(".tab-pane").removeClass('active in');
                      $("#" + tab).addClass('active in');
                  }
              });
                 $('#PayrollPeriodItemID').val(pid);
                 $('#ResourceID').val(rid);
             }
             else if (tab == "employees") {
                 $.post('/Sections/Timekeeper/Index',
               {
                   PayrollPeriodItemIDProject: null,
                   PayrollPeriodItemIDEmp: pid,
                   PayrollPeriodItemID: null,
                   ResourceID: null
               },
               function (res) {
                   if (res.status == undefined && res != null) {
                       var employeedetails = document.getElementById("employeedetails");
                       employeedetails.innerHTML = res;                 
                       $('li').removeClass('active');
                       $("a[href='#" + tab + "']").parent().addClass('active');
                       $(".tab-pane").removeClass('active in');
                       $("#" + tab).addClass('active in');
                   }
               });
                 $('#PayrollPeriodItemIDEmp').val(pid);
             }
             else if (tab == "projects") {
                 pid = getParameterByName("pid", url)
                 $.post('/Sections/Timekeeper/Index',
                {
                    PayrollPeriodItemIDProject: pid,
                    PayrollPeriodItemIDEmp: null,
                    PayrollPeriodItemID: null,
                    ResourceID: null
                },
                function (res) {
                    if (res.status == undefined && res != null) {                  
                        var projectdetails = document.getElementById("projectdetails");
                        projectdetails.innerHTML = res;
                        $('li').removeClass('active');
                        $("a[href='#" + tab + "']").parent().addClass('active');
                        $(".tab-pane").removeClass('active in');
                        $("#"+tab).addClass('active in');
                    }
                });
                 $('#PayrollPeriodItemIDProject').val(pid);
             }
 
         }
     }

      function getParameterByName(name, url) {
          if (!url) url = window.location.href;
          name = name.replace(/[\[\]]/g, "\\$&");
          var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
              results = regex.exec(url);
          if (!results) return null;
          if (!results[2]) return '';
          return decodeURIComponent(results[2].replace(/\+/g, " "));
      }
 