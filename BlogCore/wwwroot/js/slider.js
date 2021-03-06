﻿var dataTable;

$(function () {
    cargarDataTable();
});

function cargarDataTable() {
    dataTable = $("#tblSlider").DataTable({
        "ajax": {
            "url": "/admin/slider/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "25%" },
            { "data": "estado", "width": "15%" },
            {
                "data": "urlImagen",
                "render": function (urlImagen) { 
                    return `<img src = "../${urlImagen}" width="200px">`
                   
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class='text-center'>
                        <a href='/Admin/Slider/Edit/${data}' class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-edit'></i> 
                        Editar
                        </a>
                        &nbsp;
                        <a onclick=Delete('/Admin/Slider/Delete/${data}') class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                            <i class='fas fa-trash-alt'></i> 
                        Borrar
                        </a>
                    </div> `;
                },"width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No hay registros"
        },
        "width": "100%"
    });
}
function Delete(url) {
    
    swal({
        title: "Esta seguro de borrar?",
        text: "ESte contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnConfirm: true
    }, function () {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
    });
}