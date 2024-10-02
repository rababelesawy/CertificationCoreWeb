$(document).ready(function () {
    $("#theme").on("change", function () {
        var url = $(this).attr("data-url").replace("0", $(this).val());
        $.get(url, function (data) {
            $("#products").html(data);
        });
    });
});

function UpdateProductsTable(data, status, xhr) {
    $("#divResult").html(data);
}