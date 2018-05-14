var branchGroups = [];
var branchs = [];
var companyTypes = [];
var companies = [];
var sortedCompanies = [];
var calculates = [];
var dates = [];
var i = 0;
var selectableHead = "<label style='color:#61D2B4'>Seçim Yapınız</label><input type='text' class='col-lg-12' autocomplete='off' placeholder='Arama...'>";
var selectionHead = "<label style='color:#61D2B4'>Seçimleriniz</label><input type='text' class='col-lg-12' autocomplete='off' placeholder='Arama...'>";
var selectableFooter = "<label style='color:#61D2B4;cursor:pointer' onclick='selectAll(this);'>Tümünü Seç</label>";
var selectionFooter = "<label style='color:#61D2B4;cursor:pointer' onclick='deSelectAll(this);'>Tümünü Kaldır</label>";

$(document).ready(function () {
    $('.scrolldiv').scrollWidth;
});

function selectAll(item) {

    $(item).parent('.ms-selectable').parent('.ms-container').prev().multiSelect('select_all');
    switch (i) {
        case 0:
            branchGroups = [];
            $.each($('#branchGroupSelector').children('option'), function (index, item) {
                branchGroups.push(parseInt($(item).val()));
            })
            break;
        case 1:
            branchs = [];
            $.each($('#branchSelector').children('option'), function (index, item) {
                branchs.push(parseInt($(item).val()));

            })
            break;
        case 2:
            companyTypes = [];
            $.each($('#companyTypeSelector').children('option'), function (index, item) {
                companyTypes.push(parseInt($(item).val()));

            })
            break;
        case 3:
            companies = [];
            $.each($('#companySelector').children('option'), function (index, item) {
                companies.push(parseInt($(item).val()));

            })
            break;
        case 4:
            calculates = [];
            $.each($('#calculateSelector').children('option'), function (index, item) {
                calculates.push(parseInt($(item).val()));

            })
            break;
            alert("Hata Oluştu. Eksik veya Yanlış Sonuç Almamak İçin Lütfen Sayfayı yenileyiniz");
        default:

    }



}
function deSelectAll(item) {

    $(item).parent('.ms-selection').parent('.ms-container').prev().multiSelect('deselect_all');
    switch (i) {
        case 0:
            branchGroups = [];
            break;
        case 1:
            branchs = [];
            break;
        case 2:
            companyTypes = [];
            break;
        case 3:
            companies = [];
            break;
        case 4:
            calculates = [];
            break;
        default:
            alert("Hata Oluştu. Eksik veya Yanlış Sonuç Almamak İçin Lütfen Sayfayı yenileyiniz");

    }



}

function newQuery() {
    window.location.href = "#features";
    location.reload();
}

$('#hesapla').click(function() {

    switch (i) {
    case 0:

        if (branchGroups.length != 0) {

            $.each(branchGroups,
                function(index, item) {
                    $.ajax({
                        dataType: 'JSON',
                        type: 'GET',
                        url: '../Home/BranchJson',
                        data: { 'id': parseInt(item) },
                        contentType: 'application/json; charset:Utf-8',
                        success: function(data) {

                            $('.groupPartial').fadeOut(1000);
                            $('.branchPartial').delay(1000).fadeIn(1000);
                            $.each(data,
                                function(index, d) {
                                    $('#branchSelector').multiSelect('addOption',
                                        { value: d.id, text: d.name + "-" + d.branchCode, index: 0 });
                                })
                            $('#branchSelector').multiSelect('refresh');
                            $('#branchSelector').multiSelect('deselect_all');


                        },

                    })
                })

            i++;
        } else {
            alert("Lütfen Branş Grubu seçiniz");
        }

        break;
    case 1:
        if (branchs != 0) {
            $('.branchPartial').fadeOut(1000);
            $('.typePartial').delay(1000).fadeIn(1000);
            $('#companyTypeSelector').multiSelect('refresh');
            $('#companyTypeSelector').multiSelect('deselect_all');

            i++;
        } else {
            alert("Lütfen Branş Seçiniz");
        }

        break;
    case 2:
        if (companyTypes.length != 0) {

            $.each(companyTypes,
                function(index, item) {

                    $.ajax({
                        dataType: 'JSON',
                        type: 'GET',
                        url: '../Home/CompanyJson',
                        data: { 'id': parseInt(item) },
                        contentType: 'application/json; charset:Utf-8',
                        success: function(data) {
                            $('.typePartial').fadeOut(1000);
                            $('.companyPartial').delay(1000).fadeIn(1000);
                            $.each(data,
                                function(index, d) {
                                    $('#companySelector').multiSelect('addOption',
                                        { value: d.id, text: d.name, index: 0 });
                                });
                            $('#companySelector').multiSelect('refresh');
                            $('#companySelector').multiSelect('deselect_all');


                        }

                    });


                });
            
            
            i++;
        } else {
            alert('Lütfen Şirket Tipi Seçiniz');
        }
        break;
    case 3:
        if (companies.length != 0) {
            $('.companyPartial').fadeOut(1000);
            $('.calcPartial').delay(1000).fadeIn(1000);
            $('#calculateSelector').multiSelect('refresh');
            $('#calculateSelector').multiSelect('deselect_all');


            i++;
        } else {
            alert('Lütfen Şirket Seçiniz');
        }
        break;
    case 4:
        if (calculates.length != 0) {
            $('.calcPartial').fadeOut(1000);
            $('.datePartial').delay(1000).fadeIn(1000);
            $(this).text('HESAPLA');
            i++;
        } else {
            alert("Lütfen Hesaplama Türü Seçiniz");
        }
        break;

    case 5:
        dates = $('#pickyDate').val().split(',');
        if (dates.length != 0) {

            $.ajax({
                url: '../Home/Hesapla',
                dataType: 'Json',
                type: 'GET',
                data: { 'branches': branchs, 'companies': companies, 'calculates': calculates, 'dates': dates },
                traditional: true,
                contentType: 'application/Json;charset:Utf-8',
                beforeSend: function() {
                    $('#loading').show();
                    window.location.href = '#calculateresultget';
                },
                success: function(data) {

                    $('#calculateresult').children().remove();
                    var dates = $('#pickyDate').val().split(',');
                    if (data == -2) {
                        alert("Lütfen geçerli tarih giriniz");
                        newQuery();
                    }
                    if (data == -1) {
                        alert("eksik bilgi girdiniz");
                        newQuery();
                    }
                    if (data[0] == 0) {
                        alert("formul Hatası");
                        newQuery();
                    } else {


                        var table = "";
                        var cn = "";
                        var mindexer = -1;
                        $.each(data,
                            function(index, item) {
                                if (mindexer != item.indexer) {
                                    if (item.indexer != 0) {
                                        table += '</tr></table></div></br></br>';
                                        $('#calculateresult').append(table);
                                        cn = "";
                                        table = "";
                                    }
                                    table = '<label>' +
                                        item.calcType +
                                        '</label></br><div class="row table-responsive"><table class="table table-bordered scrolldiv"><tr><th>#</th>';
                                    $.each(dates,
                                        function(index, tdate) {
                                            table += '<th>' + tdate + '</th>';
                                        })
                                    mindexer = item.indexer;
                                }

                                if (cn != item.companyName) {
                                    cn = item.companyName;
                                    table += '</tr><tr><td>' + item.companyName + '</td>';
                                }
                                table += '<td style="text-align:right">' + item.amount + '</td>';


                            })
                        table +=
                            '</tr></table></br><div class="btn-group col-lg-12"> <button class="btn btn-success" onclick="newQuery();">Yeni Sorgu</button></div>';
                        $('#calculateresult').append(table);
                        
                        $('#loading').hide(function() {
                            $('html, body').animate({
                                scrollTop: $("#calculateresult").offset().top-60
                            }, 1500);

                        });

                    }
                }
            })

            i = 0;

            $(this).remove();
            $('#services').fadeOut(1000);
            $('#portfolios').css('background-color', '#61d2b4');
        } else {
            alert('Lütfen Geçerli Bir Tarih Giriniz');
        }

        break;
    default:
        break;
    }


});



$('#calculateSelector').multiSelect({
    selectableHeader: selectableHead,
    selectionHeader: selectionHead,
    selectableFooter: selectableFooter,
    selectionFooter: selectionFooter,

    afterSelect: function (values) {
        calculates.push(parseInt(values));
    },
    afterDeselect: function (values) {
        calculates.remove(parseInt(values));
    },

    afterInit: function (ms) {
        var that = this,
            $selectableSearch = that.$selectableUl.prev(),
            $selectionSearch = that.$selectionUl.prev(),
            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
        .on('keydown', function (e) {
            if (e.which === 40) {
                that.$selectableUl.focus();
                return false;
            }
        });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
        .on('keydown', function (e) {
            if (e.which == 40) {
                that.$selectionUl.focus();
                return false;
            }
        });
    },


});


$('#branchGroupSelector').multiSelect({
    selectableHeader: selectableHead,
    selectionHeader: selectionHead,
    selectableFooter: selectableFooter,
    selectionFooter: selectionFooter,

    afterSelect: function (values) {
        branchGroups.push(parseInt(values));

    },
    afterDeselect: function (values) {
        branchGroups.remove(parseInt(values));
    },

    afterInit: function (ms) {
        var that = this,
            $selectableSearch = that.$selectableUl.prev(),
            $selectionSearch = that.$selectionUl.prev(),
            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
        .on('keydown', function (e) {
            if (e.which === 40) {
                that.$selectableUl.focus();
                return false;
            }
        });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
        .on('keydown', function (e) {
            if (e.which == 40) {
                that.$selectionUl.focus();
                return false;
            }
        });
    },


});

$('#branchSelector').multiSelect({
    selectableHeader: selectableHead,
    selectionHeader: selectionHead,
    selectableFooter: selectableFooter,
    selectionFooter: selectionFooter,

    afterSelect: function (values) {
        branchs.push(parseInt(values));

    },
    afterDeselect: function (values) {
        branchs.remove(parseInt(values));
    },

    afterInit: function (ms) {
        var that = this,
            $selectableSearch = that.$selectableUl.prev(),
            $selectionSearch = that.$selectionUl.prev(),
            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
        .on('keydown', function (e) {
            if (e.which === 40) {
                that.$selectableUl.focus();
                return false;
            }
        });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
        .on('keydown', function (e) {
            if (e.which == 40) {
                that.$selectionUl.focus();
                return false;
            }
        });
    },


});


$('#companyTypeSelector').multiSelect({
    selectableHeader: selectableHead,
    selectionHeader: selectionHead,
    selectableFooter: selectableFooter,
    selectionFooter: selectionFooter,

    afterSelect: function (values) {
        companyTypes.push(parseInt(values));

    },
    afterDeselect: function (values) {
        companyTypes.remove(parseInt(values));
    },
    afterInit: function (ms) {
        var that = this,
            $selectableSearch = that.$selectableUl.prev(),
            $selectionSearch = that.$selectionUl.prev(),
            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
        .on('keydown', function (e) {
            if (e.which === 40) {
                that.$selectableUl.focus();
                return false;
            }
        });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
        .on('keydown', function (e) {
            if (e.which == 40) {
                that.$selectionUl.focus();
                return false;
            }
        });
    },


});



$('#companySelector').multiSelect({
    selectableHeader: selectableHead,
    selectionHeader: selectionHead,
    selectableFooter: selectableFooter,
    selectionFooter: selectionFooter,

    afterSelect: function (values) {
        companies.push(parseInt(values));
    },
    afterDeselect: function (values) {
        companies.remove(parseInt(values));
    },

    afterInit: function (ms) {
        var that = this,
            $selectableSearch = that.$selectableUl.prev(),
            $selectionSearch = that.$selectionUl.prev(),
            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
        .on('keydown', function (e) {
            if (e.which === 40) {
                that.$selectableUl.focus();
                return false;
            }
        });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
        .on('keydown', function (e) {
            if (e.which == 40) {
                that.$selectionUl.focus();
                return false;
            }
        });
    },


});



$(function () {
    $("#pickyDate").datepicker({

        clearBtn: true,
        language: 'tr',
        multidate: 5,
        format: "mm-yyyy",
        viewMode: "months",
        minViewMode: "months",
        endDate: new Date(Date.now())

    });

});











Array.prototype.remove = function (x) {
    var i;
    for (i in this) {
        if (this[i].toString() == x.toString()) {
            this.splice(i, 1);
        }
    }
}
