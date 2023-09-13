    $(document).ready(function () {
        $(".star-rating .star").click(function () {
            var value = $(this).data("value");
            $("#rating-value").val(value);
            $(".star-rating .star").removeClass("checked");
            $(this).prevAll().addBack().addClass("checked");
        });
    });
