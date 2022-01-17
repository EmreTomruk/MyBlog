$(document).ready(function ()
{
    // DataTables start here. 

    const dataTable = $('#usersTable').DataTable(
    {
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
        {
            text: 'Ekle',
            attr: {
                id: "btnAdd",
            },
            className: 'btn btn-success',
            action: function (e, dt, node, config) {
            }
        },
        {
            text: 'Yenile',
            className: 'btn btn-warning',
            action: function (e, dt, node, config)
            {
                $.ajax(
                {
                    type: 'Get',
                    url: '/Admin/User/GetAllUsers/',
                    contentType: "application/json",
                    beforeSend: function () {
                        $('#usersTable').hide();
                        $('.spinner-border').show();
                    },
                    success: function (data) 
                    {
                        const userListDto = jQuery.parseJSON(data); //Gelen string veri(Json) objeye cevirilir...
                        dataTable.clear();
                        console.log(userListDto); //Gelen userListDto objelerini console'da goruntuleyebilmek icin yazdik...

                        if (userListDto.ResultStatus === 0)
                        {
                            $.each(userListDto.Users.$values, function (index, user)  //Json'a Parse edilecegi icin $values kullandik...
                            {
                                const newTableRow = dataTable.row.add([
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.PhoneNumber,
                                    `<img src="/img/${user.Picture}" alt="${user.UserName}" class="my-image-table">`,

                                    `<button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}"><span class="fas fa-minus-circle"></span></button>`
                                ]).node();

                                const jqueryTableRow = $(newTableRow);
                                jqueryTableRow.attr('name', `${user.Id}`);
                            });

                            dataTable.draw();
                            $('.spinner-border').hide();
                            $('#usersTable').fadeIn(1400);
                        }
                        else
                        {
                            toastr.error(`${userListDto.Message}`, 'İşlem Başarısız!');
                        }
                    },
                    error: function (err) 
                    {
                        console.log(err);
                        $('.spinner-border').hide();
                        $('#usersTable').fadeIn(1000);
                        toastr.error(`${err.responseText}`, 'Hata!');
                    }
                });
            }
        }
        ],
        language: {
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
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    // DataTables end here 

    // Ajax GET / Getting the _UserAddPartial as Modal Form starts from here. 

    $(function () {
        const url = '/Admin/User/Add';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) //modalPlaceHolder div'ine modal'imizi(User Controller-Add Action(get)'dan donen) yerlestirdik...
            {
                placeHolderDiv.html(data); //"data" : "_UserAddPartial"'dir. Burada placeHolderDiv'in html'ini _UserAddPartial ile doldurduk..
                placeHolderDiv.find(".modal").modal('show'); //placeHolderDiv icinde class=modal olan div'i bul, onu placeHolderDiv'in modal'i yap ve goster...
            });
        });
        // Ajax GET / Getting the _UserAddPartial as Modal Form ends here.

        // Ajax POST / Posting the FormData as UserAddDto starts from here. 

        placeHolderDiv.on('click', '#btnSave', function (event) {
            event.preventDefault(); //Kendi click islemini onlemis olduk(aksi halde sayfa yenilenecekti)... 
            const form = $('#form-user-add');
            const actionUrl = form.attr('action'); //action=_UserAddPartial.cshtml sayfasindaki asp-action="Add"'tir...
            const dataToSend = new FormData(form.get(0));

            $.ajax(
            {
                url: actionUrl,
                type: 'Post',
                data: dataToSend,
                processData: false,
                contentType: false,

                success: function (data) //Buradaki data, dataToSend ile Json formatinda Controller'a gonderdigimiz verilerin, Controller'da islenip bize yine Json formatinda donmus halidir...
                {
                    console.log(data);
                    const userAddAjaxModel = jQuery.parseJSON(data); //Data objeye cevirilir...
                    console.log(userAddAjaxModel);
                    const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial); //userAddAjaxModel icindeki UserAddPartial'deki '.modal-body' yi al...
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                    if (isValid)
                    {
                        placeHolderDiv.find('.modal').modal('hide');

                        const newTableRow = dataTable.row.add([
                            userAddAjaxModel.UserDto.User.Id,
                            userAddAjaxModel.UserDto.User.UserName,
                            userAddAjaxModel.UserDto.User.Email,
                            userAddAjaxModel.UserDto.User.PhoneNumber,
                            `<img src="/img/${userAddAjaxModel.UserDto.User.Picture}" alt="${userAddAjaxModel.UserDto.User.UserName}" class="my-image-table">`,

                            `<button class="btn btn-primary btn-sm btn-update" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>`
                        ]).node();
                        const jqueryTableRow = $(newTableRow);
                        jqueryTableRow.attr('name', `${userAddAjaxModel.UserDto.User.Id}`);
                        dataTable.row(newTableRow).draw();
                        toastr.success(`${userAddAjaxModel.UserDto.Message}`, 'Başarılı İşlem!');
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
                },
                error: function (err)
                {
                    console.log(err);
                }
            });
        });
    });
    // Ajax POST / Posting the FormData as UserAddDto ends here. 

    //Ajax POST / Deleting a User starts from here 

    $(document).on('click', '.btn-delete', function (event)
    {
        event.preventDefault(); //butonun kendi gorevi varsa, o gorevi iptal eder...
        const id = $(this).attr('data-id'); //this=basilan butondur
        const tableRow = $(`[name="${id}"]`);
        const userName = tableRow.find('td:eq(1)').text();

        Swal.fire(
<<<<<<< HEAD
        {
            title: 'Silmek istediğinize emin misiniz?',
            text: `${userName} adlı kullanıcı silinecektir!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum.',
            cancelButtonText: 'Hayır, silmek istemiyorum.'
        }).then((result) => //Evet butonuna basilirsa result='true' olur...
        {
            if (result.isConfirmed)
=======
            {
                title: 'Silmek istediğinize emin misiniz?',
                text: `${userName} adlı kullanıcı silinecektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, silmek istiyorum.',
                cancelButtonText: 'Hayır, silmek istemiyorum.'
            }).then((result) => //Evet butonuna basilirsa result='true' olur...
>>>>>>> parent of 9f96e26 (Last Changes)
            {
                $.ajax(
                {
                    type: 'POST',
                    dataType: 'json',
                    data: { userId: id },
                    url: '/Admin/User/Delete',
                    success: function (data) 
                    {
                        const userDto = jQuery.parseJSON(data);

                        if (userDto.ResultStatus === 0)
                        {
                            Swal.fire(
                                'Silindi!',
                                `${userDto.User.UserName} adlı kullanıcı başarıyla silinmiştir.`,
                                'success'
                            );
                            dataTable.row(tableRow).remove().draw();
                        }
                        else
                        {
                            Swal.fire(
                            {
                                icon: 'error',
                                title: 'Başarısız İşlem!',
                                text: `${userDto.Message}`
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
    // Ajax Get / Getting the _UserUpdatePartial as Modal Form starts from here

    $(function ()
    {
        const url = '/Admin/User/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click', '.btn-update', function (event)
        {
            event.preventDefault();
            const id = $(this).attr('data-id'); //Bu event'in gerceklestigi butonun Id'si(o da zaten userId'dir) alindi......
            $.get(url, { userId: id }).done(function (data) //Gidecegi yerde beklenen userId=id yapmis olduk...
            {
                placeHolderDiv.html(data); //placeHolderDiv icindeki div'i gonderdigimiz data ile doldur(ancak gizli oldugu icin goremiyoruz)......
                placeHolderDiv.find('.modal').modal('show'); //Gizli olan modal'i gosterir...
            }).fail(function ()
            {
                toastr.error("Bir hata oluştu...");
            });
        });
        // Ajax Get / Getting the _UserUpdatePartial as Modal Form ends from here

        // Ajax POST / Updating a User starts from here

        placeHolderDiv.on('click', '#btnUpdate', function (event)
        {
            event.preventDefault();
            const form = $('#form-user-update');
            const actionUrl = form.attr('action'); //'action' attribute icinde bir url var(/Admin/User/Update) ve biz bu url'ye formumuzu gonderecegiz...
            const dataToSend = new FormData(form.get(0));

            $.ajax(
            {
                url: actionUrl,
                type: 'Post',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) //Form icindeki bilgileri Ajax/Post islemi ile action'a gonderecegiz...
                {
                    const userUpdateAjaxModel = jQuery.parseJSON(data); //Gelen datayi objeye cevirdik...
                    console.log(userUpdateAjaxModel);
                    const id = userUpdateAjaxModel.UserDto.User.Id;
                    const tableRow = $(`[name="${id}"]`);
                    const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial); //userUpdateAjaxModel icindeki UserUpdatePartial'dan .modal-body'yi sectik(gelen PartialView'i ModalForm icine eklemeliyiz/degistirmeliyiz)...
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                    if (isValid)
                    {
                        placeHolderDiv.find('.modal').modal('hide');
                        dataTable.row(tableRow).data(
                        [
                            userUpdateAjaxModel.UserDto.User.Id,
                            userUpdateAjaxModel.UserDto.User.UserName,
                            userUpdateAjaxModel.UserDto.User.Email,
                            userUpdateAjaxModel.UserDto.User.PhoneNumber,
                            `<img src="/img/${userUpdateAjaxModel.UserDto.User.Picture}" alt="${userUpdateAjaxModel.UserDto.User.UserName}" class="my-image-table">`,

                            `<button class="btn btn-primary btn-sm btn-update" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>`
                        ]);

                        tableRow.attr("name", `${id}`);
                        dataTable.row(tableRow).invalidate(); //dataTable'a tableRow'daki degisiklikleri bildiririz...
                        toastr.success(`${userUpdateAjaxModel.UserDto.Message}`, "Başarılı İşlem!")
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
                },
                error: function (error) 
                {
                    console.log(error);
                }
            });
        });
    });
    // Ajax POST / Updating a User ends from here

});