
$(document).ready(function () {
    $("#cartList").hide();
    $(document).on('click', '#Carticon', function () {
        $("#cartList").fadeToggle(0);
        $("#cartList").removeClass("d-none");
    });
    $(document).on('click', function (event) {
        if (!$(event.target).closest("#Carticon").length &&
            !$(event.target).closest("#cartList").length &&
            $("#cartList").is(":visible"))
        {
            $("#cartList").fadeOut(0);
        }
    });
});


