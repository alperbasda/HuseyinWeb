

$('#saveCompanyType').click(function () {
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateCompanyType',
        data: { 'type': $('#companyTypeName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });
})

$('#saveBranchGroup').click(function () {

    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateBranchGroup',
        data: { 'name': $('#branchGroupName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });

})





$('#saveCalc').click(function () {

    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateCalculate',
        data: { 'name': $('#calcName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });

})

$('#saveBranch').click(function () {

    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateBranch',
        data: { 'branchCode': $('#branchCode').val(), 'groupId': $('#selectpickerbranch').val(), 'name': $('#branchName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });

})

$('#saveAccount').click(function () {

    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateAccount',
        data: { 'name': $('#accountName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });

})



$('#btnCreateBranch').click(function () {
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/BranchGroupSelector',
        contentType: 'application/Json',
        beforeSend: function () {
            $('#branchName').attr("placeholder", "Lütfen Bekleyiniz");
        },
        success: function (response) {
            $('#branchName').attr("placeholder", "Branş Giriniz");
            if (response == 0) {
                alert("Modalı yenileyiniz...");
            }
            if ($("#selectpickerbranch").length > 0) {
                $("#branchNameBody").children('select').remove();
                $("#branchNameBody").children('div').remove();
            }

            var a = "";
            $(response).each(function (index, item) {
                a += '<option value="' + item.id + '">' + item.name + '</option>';
            });

            $("#branchName").before('<select class="selectpicker" id="selectpickerbranch">' +
                a +
                '</select>');
            $('#selectpickerbranch').multiselect({
                enableFiltering: true,
                maxHeight: 200

            });
        },
        error: function (er) {
            alert(er);

        }


    });

})

var selectedOptions = [];
var selectedOptions2 = [];
$('#btnCreateAccountCalcRelation').click(function () {
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/EmptyCalcsSelector',
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0) {
                alert("Modalı yenileyiniz...");
            }
            if ($("#selectpickercalc").length > 0) {
                $("#calcAccountRelationBody").children().remove();

            }

            var a = "";
            $(response).each(function (index, item) {
                a += '<option value="' + item.id + '">' + item.name + '</option>';
            });

            $("#calcAccountRelationBody").append('<span class="label label-default">Hesaplama : </span><select class="selectpicker" id="selectpickercalc">' +
                a +
                '</select><label class="form-check-label">Tümünü Topla</label><input type="checkbox" id="sumAll"><br/><br/><br/>');
            $('#selectpickercalc').multiselect({
                enableFiltering: true
            });
        },
        error: function (er) {
            alert(er);

        }


    });
    selectedOptions = [];
    selectedOptions2 = [];
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/AccountSelector',
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0) {
                alert("Modalı yenileyiniz...");
            }

            var a = "";
            $(response).each(function (index, item) {
                a += '<option value="' + item.id + '">' + item.name + '</option>';
            });

            $("#calcAccountRelationBody").append('<span class="label label-default">Bölünen : </span><select class="selectpicker" multiple="multiple" id="selectpickeraccount">' +
                a +
                '</select><br/><br/><br/>');
            $('#selectpickeraccount').multiselect({
                enableFiltering: true,
                maxHeight: 200,
                onChange: function (option, checked) {
                    selectedOptions = [];
                    $.each($('#selectpickeraccount option:selected'), function (index, item) {
                        selectedOptions.push($(item).val());

                    });

                }

            });

            $("#calcAccountRelationBody").append('<span class="label label-default">Bölen : </span><select class="selectpicker" multiple="multiple" id="selectpickeraccount2">' +
                a +
                '</select><br/>');
            $('#selectpickeraccount2').multiselect({
                enableFiltering: true,
                maxHeight: 200,
                onChange: function (option, checked) {
                    selectedOptions2 = [];
                    $.each($('#selectpickeraccount2 option:selected'), function (index, item) {
                        selectedOptions2.push($(item).val());

                    });
                }

            });

        },
        error: function (er) {
            alert(er);

        }


    });

})


$('#btnCreateCompany').click(function () {
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CompanyTypeSelect',
        contentType: 'application/Json',
        beforeSend: function () {
            $('#companyName').attr("placeholder", "Lütfen Bekleyiniz");
        },
        success: function (response) {
            $('#companyName').attr("placeholder", "Şirket Giriniz");
            if (response == 0) {
                alert("Modalı yenileyiniz...");
            }
            if ($("#selectpickercompanytype").length > 0) {
                $("#companyNameBody").children('select').remove();
                $("#companyNameBody").children('div').remove();
            }

            var a = "";
            $(response).each(function (index, item) {
                a += '<option value="' + item.id + '">' + item.name + '</option>';
            });

            $("#companyName").before('<select class="selectpicker" id="selectpickercompanytype">' +
                 a +
                '</select>');
            $('#selectpickercompanytype').multiselect({
                enableFiltering: true
            });
        },
        error: function (er) {
            alert(er);

        }


    });

})

$('#saveCompany').click(function () {

    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateCompany',
        data: { 'typeId': $('#selectpickercompanytype').val(), 'name': $('#companyName').val() },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>')
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>')
        }


    });

})


$('#saveAccountCalcRelation').click(function () {
    var checked = "";
    if ($('#sumAll').is(':checked')) {
        alert("true");
        checked = "on";
    } else {
        alert("false");
        checked = "off";
    }
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Admin/CreateAccountCalcRelation',
        traditional: true,
        data: {
            'bolunen': JSON.stringify(selectedOptions),
            'bolen': JSON.stringify(selectedOptions2),
            'hesap': $('#selectpickercalc').val(),
            'sumAll': checked
        },
        contentType: 'application/Json',
        success: function (response) {
            if (response == 0)
                $('.notification')
                    .append(
                        '<div class="alert alert-danger"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Hata !!! </strong> Kayıt işlemi sırasında hata oluştu</div>');
            else if (response == -1)
                window.location.href = '../Admin/Login';
            else
                $('.notification')
                    .append(
                        '<div class="alert alert-success"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Başarılı !!! </strong> ' +
                        response +
                        ' Kayıt Edildi</div>');
        }


    });


});

$('#btnUploadData').click(function () {
    $('#uploadData').click();
});
$('#uploadData').change(function () {
    $('#uploadData').attr('disabled', 'true');
    $('#btnSaveData').attr('disabled', 'true');
    $('#saveData').html('Kapat');
    var percent = 0;
    $('#myprogress').attr('aria-valuenow', 0).css('width', 0 + '%').text(0 + '%');

    var data = new FormData();
    var files = $('#uploadData').get(0).files;
    data.append('file', files[0]);
    $("#saveData").html('Dosya Yükleniyor...');
    $.ajax({
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener('progress',
                function (e) {

                    if (e.lengthComputable) {
                        percent = Math.round((e.loaded / e.total) * 100);
                        $('#myprogress').attr('aria-valuenow', percent).css('width', percent + '%').text(percent + '%');

                    }
                });
            return xhr;


        },
        url: '../Admin/UploadData',
        type: 'POST',
        processData: false,
        data: data,
        dataType: 'json',
        contentType: false,
        success: function (turn) {
            if (turn == 0) {
                alert("Dosya Bulunamadı Sayfayı Yenileyiniz");
            }
            else if (turn == 0) {
                alert("Dosya uzantınız .xlsx olmalı");
            }
            $('#btnSaveData').removeAttr('disabled');
            $('#saveData').html('Yükleme Tamamlandı');
            $('#uploadData').removeAttr('disabled');
        }


    });

});