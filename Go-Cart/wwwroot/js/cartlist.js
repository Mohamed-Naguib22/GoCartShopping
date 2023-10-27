
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
    let circleText = document.querySelector('.circle-text');
    circleText.innerHTML = circleText.textContent.split("").map((char, index) => `<span style="transform:rotate(${index * 28.5}deg)" style="background-color:#555">${char}</span>`).join("");

});


