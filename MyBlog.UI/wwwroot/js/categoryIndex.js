﻿$(document).ready(function ()
{
    // DataTables start here. 

    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "order": [[6, "desc"]],
        buttons:
        [
            {
                text: 'Ekle',
                attr: {id: "btnAdd"},
                className: 'btn btn-success',
                action: function (e, dt, node, config) { }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config)
                {
                    $.ajax(
                    {
                        type: 'GET',
                        url: '/Admin/Category/GetAllCategories/',
                        contentType: "application/json",
                        beforeSend: function ()
                        {
                            $('#categoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data); //Gelen string veri(Json) objeye cevirilir...
                            console.log(categoryListDto); //Gelen categoryListDto objelerini console'da goruntuleyebilmek icin yazdik...
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = String.Empty;
                                $.each(categoryListDto.Categories.$values, function (index, category) { //Json'a Parse edilecegi icin $values kullandik...

                                    tableBody += `
                                    <tr>
                                        <td>${category.Id}</td>
                                        <td>${category.Name}</td>
                                        <td>${category.Description}</td>
                                        <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                        <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                        <td>${category.Note}</td>
                                        <td>${convertToShortDate(category.CreatedDate)}</td>
                                        <td>${category.CreatedByName}</td>
                                        <td>${convertToShortDate(category.ModifiedDate)}</td>
                                        <td>${category.ModifiedByName}</td>
                                        <td>
                                            <button class="btn btn-primary btn-sm btn-update" data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                            <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                        </td>
                                    </tr>`;
                                });
                                $('#categoriesTable > tbody').replaceWith(tableBody);
                                $('.spinner-border').hide();
                                $('#categoriesTable').fadeIn(1400);
                            } else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#categoriesTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            }
        ],
        language:
        {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria":
            {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select":
            {
                "rows":
                {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    // DataTables end here 

    // Ajax GET / Getting the _CategoryAddPartial as Modal Form starts from here. 

    $(function ()
    {
        const url = '/Admin/Category/Add';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function ()
        {
            $.get(url).done(function (data) { //modalPlaceHolder div'ine modal'imizi yerlestirdik...

                placeHolderDiv.html(data); //"data" : "_CategoryAddPartial"'dir. Burada placeHolderDiv'in html'ini _CategoryAddPartial ile doldurduk..
                placeHolderDiv.find(".modal").modal('show'); //placeHolderDiv icinde class=modal olan div'i bul, onu placeHolderDiv'in modal'i yap ve goster...
            });
        });
        // Ajax GET / Getting the _CategoryAddPartial as Modal Form ends here. 

        // Ajax POST / Posting the FormData as CategoryAddDto starts from here. 

        placeHolderDiv.on('click', '#btnSave', function (event)
        {
            event.preventDefault(); //Kendi click islemini onlemis olduk(aksi halde sayfa yenilenecekti)... 
            const form = $('#form-category-add');
            const actionUrl = form.attr('action'); //action=_CategoryAddPartial.cshtml sayfasindaki asp-action="Add"'tir...
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) //Buradaki data, dataToSend ile Json formatinda Controller'a gonderdigimiz verilerin, Controller'da islenip bize yine Json formatinda donmus halidir...
            { 
                console.log(data);
                const categoryAddAjaxModel = jQuery.parseJSON(data); //Data objeye cevirilir...
                console.log(categoryAddAjaxModel);
                const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial); //categoryAddAjaxModel icindeki CategoryAddPartial'deki '.modal-body' yi al...
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid)
                {
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow = `
                        <tr name="${categoryAddAjaxModel.CategoryDto.Category.Id}">
                            <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                            <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                            <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                            <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                            <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                            <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                            <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                            <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                            <td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                            </td>
                        </tr>`;
                    const newTableRowObject = $(newTableRow); //string'den JS/Jquery objesine cevrildi...
                    newTableRowObject.hide();
                    $('#categoriesTable').append(newTableRowObject);
                    newTableRowObject.fadeIn(3500);
                    toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı İşlem!');
                } 
                else
                {
                    let summaryText = String.Empty;
                    $('#validation-summary > ul > li').each(function () //#validation-summary'in icindeki ul'nin icindeki li'ler(Bizim ModelState != true ise 'validation-summary' div'i icinde bir ul ve onun icinde de li'ler olusur...
                    {
                        let text = $(this).text(); //this: li'ler(foreach ile dondugumuz her li)
                        summaryText = `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            });
        });
    });
    // Ajax POST / Posting the FormData as CategoryAddDto ends here. 

    //Ajax POST / Deleting a Category starts from here 

    $(document).on('click', '.btn-delete', function (event)
    {
        event.preventDefault(); //butonun kendi gorevi varsa, o gorevi iptal eder...
        const id = $(this).attr('data-id'); //this=basilan butondur
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find('td:eq(1)').text();
        Swal.fire(
        {
            title: 'Silmek istediğinize emin misiniz?',
            text: `${categoryName} adlı kategori silinicektir!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum.',
            cancelButtonText: 'Hayır, silmek istemiyorum.'
        }).then((result) => { //Evet butonuna basilirsa result='true' olur...

            if (result.isConfirmed)
            {
                $.ajax(
                {
                    type: 'POST',
                    dataType: 'json',
                    data: { categoryId: id },
                    url: '/Admin/Category/Delete',
                    success: function (data) 
                    {
                        const categoryDto = jQuery.parseJSON(data);

                        if (categoryDto.ResultStatus === 0)
                        {
                            Swal.fire(
                                'Silindi!',
                                `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir.`,
                                'success'
                            );
                            tableRow.fadeOut(3500);
                        }
                        else
                        {
                            Swal.fire(
                            {
                                icon: 'error',
                                title: 'Başarısız İşlem!',
                                text: `${categoryDto.Message}`,
                            });
                        }
                    },
                    error: function (err) 
                    {
                        console.log(err);
                        toastr.error(`${err.responseText}`, "Hata!")
                    }
                });
            }
        });
    });
    //Ajax POST / Deleting a Category ends from here 

    // Ajax POST / Updating a Category starts from here

    $(function ()
    {
        const url = '/Admin/Category/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click', '.btn-update', function (event)
        {
            event.preventDefault();
            const id = $(this).attr('data-id'); //Bu event'in gerceklestigi butonun Id'si(o da zaten categoryId'dir) alindi......
            $.get(url, { categoryId: id }).done(function (data) //Gidecegi yerde beklenen categoryId=id yapmis olduk...
            { 

                placeHolderDiv.html(data); //placeHolderDiv icindeki div'i gonderdigimiz data ile doldur(ancak gizli oldugu icin goremiyoruz)......
                placeHolderDiv.find('.modal').modal('show'); //Gizli olan modal'i gosterir...

            }).fail(function ()
            {
                toastr.error("Bir hata oluştu...");
            });
        });

        placeHolderDiv.on('click', '#btnUpdate', function (event)
        {
            event.preventDefault();
            const form = $('#form-category-update');
            const actionUrl = form.attr('action'); //'action' attribute icinde bir url var(/Admin/Category/Update) ve biz bu url'ye formumuzu gonderecegiz...
            const dataToSend = form.serialize(); //'dataToSend' gonderecegimiz veridir. dataToSend categoryUpdateDto'dur...

            $.post(actionUrl, dataToSend).done(function (data) //Form icindeki bilgileri Ajax/Post islemi ile action'a gonderecegiz...
            { 
                const categoryUpdateAjaxModel = jQuery.parseJSON(data); //Gelen datayi objeye cevirdik...
                console.log(categoryUpdateAjaxModel);
                const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial); //categoryUpdateAjaxModel icindeki CategoryUpdatePartial'dan .modal-body'yi sectik(gelen PartialView'i ModalForm icine eklemeliyiz/degistirmeliyiz)...
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                if (isValid)
                {
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow = `
                        <tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
                            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                            <td>${convertFirstLetterToUpperCase(categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.CreatedByName}</td>
                            <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                            <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                            <td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                            </td>
                        </tr>`;

                    const newTableRowObject = $(newTableRow);
                    const categoryTableRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`); //categoryTableRow eski kategori bilgileridir...
                    newTableRowObject.hide();
                    categoryTableRow.replaceWith(newTableRowObject);
                    newTableRowObject.fadeIn(3500);
                    toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, "Başarılı İşlem!")
                }
                else
                {
                    let summaryText = String.Empty;
                    $('#validation-summary > ul > li').each(function ()
                    {
                        let text = $(this).text();
                        summaryText = `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }).fail(function (response)
            {
                console.log(response);
            });
        })
    });
    // Ajax POST / Updating a Category ends from here

});
