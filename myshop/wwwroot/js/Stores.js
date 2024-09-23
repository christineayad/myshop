var dtble;
$(document).ready(function(){

    loaddata();
});
function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url":"/Admin/Store/GetData"
        },
        "columns": [
            { "data": "name" },
            { "data": "address" },
            { "data": "phoneNumber" },
            { "data": "storekeeper" },
            { "data": "minum_order" },
            { "data": "isMain" },
            {

                "data": "id",
                "render": function (data) {
                    return `

<a href="/Admin/Store/Edit/${data}" class="btn btn-success"><i class="bi bi-pencil-square"></i></a>
<a  onclick="DeleteItem('/Admin/Store/Delete/${data}')" class="btn btn-danger"><i class="bi bi-trash-fill"></i></a>`
                }
                }
           

        ]

        })
}
function DeleteItem(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({ /*delete from DB*/
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dtble.ajax.reload();
                        toaster.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }
                }

            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}