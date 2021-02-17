$(document).ready(function () {
    var alertmessage = $("#alertmessage");

    $('#frmBlog').ajaxForm({
        beforeSend: function () {
            $("#btnSubmitBlog").attr("disabled", true);
        },
        success: function (data) {
            alertmessage.removeClass();
            alertmessage.html(data.Message);

            if (data.Status == 1) {
                alertmessage.addClass("alert alert-success");
                $("#btnSubmitBlog").attr("disabled", false);
                clearForm();
            }
            else {
                alertmessage.addClass("alert alert-danger");
                $("#btnSubmitBlog").attr("disabled", false);
                $("#btnSubmitBlog").add("disabled", "");
            }
            $("#btnSubmitBlog").attr("disabled", false);
        },
        complete: function (xhr) {
            if (xhr.status != 200) {
                alertmessage.html(xhr.responseText);
                alertmessage.addClass("alert alert-danger");
                $("#btnSubmitBlog").attr("disabled", false);
                $("#btnSubmitBlog").add("disabled", "");
            }
            $("#btnSubmitBlog").attr("disabled", false);
        }
    });
});

function clearForm() {
    //$("#txtHireName").val('');
    //$("#txtHireEmail").val('');
    //$("#txtHirePhone").val('');
    //$("#txtHireMessage").val('');
    //clearCheckboxes();
}

