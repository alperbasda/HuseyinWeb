
$(document).ready(function () {
    skip = 0;
});

var skip = 0;
function Remove(item) {
    if (confirm('Veri silinsin mi?')) {
        $.ajax({
            dataType: 'JSON',
            type: 'GET',
            url: '../Remove',
            data: { 'str': $(item).parents('td').attr('id') },
            contentType: 'application/json; charset:Utf-8',
            success: function (data) {
                alert("Silme işlemi bitti");
                $(item).parents('td').parents('tr').remove().fadeOut(300);
            },
        })
    }
};


$('#BtnNext').click(function () {
    skip += 20;
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Next',
        data: { 'type': $(this).data('id'), 'skip': skip },
        contentType: 'application/json; charset:Utf-8',
        beforeSend: function () {
            $(this).attr('value', 'Bekleyiniz');
        },
        success: function (data) {
            $(this).attr('value', 'Devamını Gör');
            $('tbody').append(data);
        },
    })

})

$('#BtnSearch').click(function () {
    $('td').hide();
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../SearchParameter',
        data: { 'type': $(this).data('id'), 'skip': skip, 'filter': $('#TbSearch').val(), 'type2': $('#TbFilter').val() },
        contentType: 'application/json; charset:Utf-8',
        beforeSend: function () {
            $(this).attr('value', 'Bekleyiniz');
        },
        success: function (data) {
            $(this).attr('value', 'Ara');
            $('tbody').append(data);
        },
    })
})
$('#TbSearch').keyup(function () {

    if ($(this).val().length == 0) {
        $.each($('td'), function (index, item) {
            if ($(item).css('display') == 'none') {
                $(item).show();
            } else {
                $(item).remove();
            }

        })

    }
})

var updateItem;
function Update(item) {
    var ilk = true;
    $.each($(item).parent().parent().children('td'), function (index, itm) {
        if ($(itm).children().length == 0) {
            $(itm).append('<input type="text" data-id="' + $(itm).attr('name') + '" class="form-control" value="' + $(itm).html() + '"/>');
            $(itm).val(" ");
        }
        else if (ilk) {
            ilk = false;
            $(itm).append('<i class="fa fa-save" style="cursor:pointer;font-size:3em;" onclick="save(this)"></i>');
        }
    });
    updateItem = $(item);
    $(item).remove();
}

function save(item) {
    var values = [];
    values.push(window.location.pathname[window.location.pathname.length - 1]);
    $.each($(item).parent().parent().children('td'), function (index, itm) {
        if ($(itm).children('input').length != 0) {
            values.push($(itm).children('input').val());
            $(itm).html($(itm).children('input').val().toString());
            $(itm).children('input').remove();
        }
    });
    $.ajax({
        dataType: 'JSON',
        type: 'POST',
        url: '../Update',
        data: JSON.stringify(values),
        contentType: 'application/json;',
        traditional: true,
        success: function(response) {
            $(item).parent().append(updateItem);
            $(item).remove();
            if (response == 0) {
                alert("Girdiğiniz ikincil bağlantı anahtarı bulunamadı");
            }
        }

    });


}