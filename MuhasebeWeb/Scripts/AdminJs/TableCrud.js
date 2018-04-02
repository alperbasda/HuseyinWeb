
$(document).ready(function () {
    skip = 0;
});

var skip = 0;
function Remove(item) {
        $.ajax({
            dataType: 'JSON',
            type: 'GET',
            url: '../Remove',
            data: { 'str': $(item).parents('td').attr('id') },
            contentType: 'application/json; charset:Utf-8',
            success: function (data) {
                $(item).parents('td').parents('tr').remove().fadeOut(300);
            },
        })
};

function UpdateData() {

};

$('#BtnNext').click(function () {
    skip += 20;
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../Next',
        data: { 'type': $(this).data('id'),'skip' : skip },
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

$('#BtnSearch').click(function() {
    $('td').hide();
    $.ajax({
        dataType: 'JSON',
        type: 'GET',
        url: '../SearchParameter',
        data: { 'type': $(this).data('id'), 'skip': skip, 'filter' : $('#TbSearch').val(),'type2': $('#TbFilter').val() },
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
$('#TbSearch').keyup(function() {

    if ($(this).val().length==0) {
        $.each($('td'), function (index, item) {
            if ($(item).css('display')=='none') {
                $(item).show();
            } else {
                $(item).remove();
            }
            
        })  
        
    }
})
