function delMemberFromDept(ids) {
    swal({
        title: "Are you sure?",
        text: "You want to remove this member!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger',
        confirmButtonText: 'Yes, delete Member!',
        cancelButtonText: "No, Keep Member!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
function (isConfirm) {
    if (isConfirm) {
        var ProductViewModel = {
            ProductId: ids,
        }
        $.ajax({
            url: $path + '/Members/Delete?id=' + ids,
            contentType: 'application/json; charset=utf-8',
            type: 'GET',
        })
            .success(function (result) {
                if (result == 'success') {

                    window.location.reload();
                } else {
                    swal("Not Deleted!", "An Error Occured", "error");
                }
            })
            .error(function (xhr, status) {
                swal("Not Deleted!", "An Error Occured", "error");
            })

    } else {
        swal("Cancelled", "Your Product is still intact)", "error");
    }
});

}

function delMemberFromUnit(ids) {
    swal({
        title: "Are you sure?",
        text: "You want to remove this member this Unit!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger',
        confirmButtonText: 'Yes, delete Member!',
        cancelButtonText: "No, Keep Member!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
function (isConfirm) {
    if (isConfirm) {
        var ProductViewModel = {
            ProductId: ids,
        }
        $.ajax({
            url: $path + '/Members/DeleteFromUnit?id=' + ids,
            contentType: 'application/json; charset=utf-8',
            type: 'GET',
        })
            .success(function (result) {
                if (result == 'success') {

                    window.location.reload();
                } else {
                    swal("Not Deleted!", "An Error Occured", "error");
                }
            })
            .error(function (xhr, status) {
                swal("Not Deleted!", "An Error Occured", "error");
            })

    } else {
        swal("Cancelled", "Your Member stays)", "error");
    }
});

}


function delDues(ids) {
    swal({
        title: "Are you sure?",
        text: "You want to remove this member this Payment!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger',
        confirmButtonText: 'Yes, delete !',
        cancelButtonText: "No, Keep !",
        closeOnConfirm: false,
        closeOnCancel: false
    },
function (isConfirm) {
    if (isConfirm) {
        var ProductViewModel = {
            ProductId: ids,
        }
        $.ajax({
            url: $path + '/Finance/DeleteDues?id=' + ids,
            contentType: 'application/json; charset=utf-8',
            type: 'GET',
        })
            .success(function (result) {
                if (result == 'success') {

                    window.location.reload();
                } else {
                    swal("Not Deleted!", "An Error Occured", "error");
                }
            })
            .error(function (xhr, status) {
                swal("Not Deleted!", "An Error Occured", "error");
            })

    } else {
        swal("Cancelled", "Payment Type Intact)", "error");
    }
});

}