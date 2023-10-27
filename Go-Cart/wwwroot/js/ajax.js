var priceRange = document.getElementById('priceRange');
var selectedPrice = document.getElementById('selectedPrice');
var sortBySelect = document.getElementById('sortBy');

const sizeButtons = document.querySelectorAll(".filter-button-group button");
const colorButtons = document.querySelectorAll(".color-button");
function showToast() {
    var toastEl = document.getElementById('toast');
    toastEl.classList.remove('d-none');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();

    setTimeout(function () {
        toast.hide();
    }, 3000);
}

function showNotification() {
    var toastEl = document.getElementById('notification');
    toastEl.classList.remove('d-none');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();

    setTimeout(function () {
        toast.hide();
    }, 3000);
}

function showErrorNotification() {
    var toastEl = document.getElementById('error-notification');
    toastEl.classList.remove('d-none');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();

    setTimeout(function () {
        toast.hide();
    }, 3000);
}

$(document).ready(function () {
    // -------------------------------Search------------------------------------------
    var searchInput = $('#searchInput');

    searchInput.autocomplete({
        source: [],
        minLength: 0,
        appendTo: searchInput.parent(),
    });

    searchInput.on('input', function () {
        var searchTerm = searchInput.val();

        $.ajax({
            url: '/Product/AutoComplete',
            type: 'GET',
            data: { searchTerm: searchTerm },
            success: function (response) {
                searchInput.autocomplete('option', 'source', response);

                searchInput.autocomplete('search', '');
            }
        });
    });

    // -------------------------------Cart Slider------------------------------------------
    $(document).on('click', '#remove-cartSlider', function () {
        var productId = $(this).data("product-id");
        $.ajax({
            url: '/Cart/RemoveFromCart',
            type: 'POST',
            data: {
                productId: productId,
            },
            success: function () {
                RefreshCartSlider();
            },
            error: function () {
            }
        });
    });

    function RefreshCartSlider() {
        $.ajax({
            url: '/Cart/RefreshCartPartial',
            type: 'GET',
            data: {
                partialView:'cartSlider',
            },
            success: function (result) {
                $('#cartSlider-container').html(result);
            },
            error: function () {
            }
        });
    }

    // -------------------------------Cart------------------------------------------
    let AddCartBtn = document.querySelectorAll(".add-cart");
    AddCartBtn.forEach(item => {
        item.onclick = function () {
            var productId = $(item).data('product-id');
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'POST',
                data: {
                    productId: productId,
                },
                success: function () {
                    showNotification();
                    RefreshCartSlider();
                },
                error: function () {
                    showErrorNotification();
                }
            });
        }
    });
    $(document).on('click', '.positive', function () {
        var productId = $(this).data("product-id");
        var quantityElement = $(this).siblings(".amount");
        var quantity = parseInt(quantityElement.text());
        quantity++;
        quantityElement.text(quantity);
        updateQuantity(productId, quantityElement);
        console.log("+")
    });

    $(document).on('click', '.minus', function () {
        var productId = $(this).data("product-id");
        var quantityElement = $(this).siblings(".amount");
        var quantity = parseInt(quantityElement.text());
        if (quantity > 1) {
            quantity--;
            quantityElement.text(quantity);
            updateQuantity(productId, quantityElement);
        }
        console.log("-")
    });

    function updateQuantity(productId, quantityElement) {
        var quantity = parseInt(quantityElement.text());
        if (quantity > 0) {
            $.ajax({
                type: "POST",
                url: '/Cart/UpdateQuantity',
                data: { productId: productId, quantity: quantity },
                success: function () {
                    console.log("test");
                    RefreshCart()
                },
                error: function () {
                },
            });
        }
    }

    $(document).on('click', '#remove-cart', function () {
        var productId = $(this).data("product-id");
        $.ajax({
            url: '/Cart/RemoveFromCart',
            type: 'POST',
            data: {
                productId: productId,
            },
            success: function () {
                RefreshCart();
            },
            error: function () {
            }
        });
    });

    function RefreshCart() {
        $.ajax({
            url: '/Cart/RefreshCartPartial',
            type: 'GET',
            data: {
                partialView: 'cartItems',
            },
            success: function (result) {
                $('#cartItems-container').html(result);
            },
            error: function () {
            }
        });
    }

    // -------------------------------WishList------------------------------------------
    let wishlistBtn = document.querySelectorAll(".addToWL");
    wishlistBtn.forEach(item => {
        var productId = $(item).data('product-id');

        $.ajax({
            url: '/WishList/IsAddedToWishlist',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                if (response === true) {
                    item.firstElementChild.classList.add("fas", "fa-heart", "text-danger");
                }
            },
            error: function (xhr, status, error) {
            }
        });

        item.onclick = function () {
            var isCurrentlyAdded = item.firstElementChild.classList.contains("text-danger");
            var updatedIsAdded = !isCurrentlyAdded;

            var url = updatedIsAdded ? '/WishList/AddToWishList' : '/WishList/RemoveFromWishList';
            var requestData = { productId: productId };

            $.ajax({
                url: url,
                type: 'POST',
                data: requestData,
                success: function (response) {
                    if (updatedIsAdded) {
                        item.firstElementChild.classList.remove("fas", "fa-heart");
                        item.firstElementChild.classList.add("fas", "fa-heart", "text-danger");
                        var notificationText = document.getElementById("notification-text");
                        notificationText.textContent = "Product Added Successfully"
                        showToast();
                    }
                    else {
                        item.firstElementChild.classList.remove("fas", "fa-heart", "text-danger");
                        item.firstElementChild.classList.add("far", "fa-heart");
                        var notificationText = document.getElementById("notification-text");
                        notificationText.textContent = "Product Removed Successfully"
                        showToast();
                    }
                },
                error: function (xhr) {
                }
            });
        }
    });

    $(document).on('click', '#remove-wishlist', function () {
        var productId = $(this).data("product-id");
        removeFromWishList(productId);
    });

    function removeFromWishList(productId) {
        $.ajax({
            type: "POST",
            url: '/WishList/RemoveFromWishList',
            data: { productId: productId },
            success: function () {
                refreshWishList();
            },
            error: function () {
            }
        });
    }

    function refreshWishList() {
        $.ajax({
            url: '/WishList/GetWishListItems',
            type: 'GET',
            data: {
            },
            success: function (result) {
                $('#wishlist-container').html(result);
            },
            error: function () {
            }
        });
    }
    // -------------------------------Rating------------------------------------------
    $(".star-rating .star").click(function () {
        var value = $(this).data("value");
        $("#rating-value").val(value);
        $(".star-rating .star").removeClass("checked");
        $(this).prevAll().addBack().addClass("checked");
    });

    var productIdDetails = $('#product-id-details').val();

    $.ajax({
        url: '/Rating/GetUserRating',
        type: 'Get',
        data: {
            productId: productIdDetails,
        },
        success: function (result) {
            var requiredStars = result;
            $(".star-rating .star").slice(0, requiredStars).addClass("checked");
        },
        error: function () {
        }
    });

    $('.star-rating .star').click(function () {
        var value = $(this).data('value');
        $.ajax({
            url: '/Rating/Rate',
            type: 'POST',
            data: {
                value: value,
                productId: productIdDetails,
            },
            success: function () {
                refreshRating();
            },
            error: function () {
            }
        });
    });

    function refreshRating() {
        $.ajax({
            url: '/Rating/GetRating',
            type: 'GET',
            data: {
                productId: productIdDetails
            },
            success: function (result) {
                $('#rating-container').html(result); // Update the comments section
            },
            error: function () {
            }
        });
    }

    // -------------------------------Comments------------------------------------------
    $('#coment').keypress(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            submitReviewForm();
        }
    });

    $(document).on('click', '.addreview-btn', function () {
        submitReviewForm();
    });

    
    function submitReviewForm() {
        var review = $('#coment').val();
        $.ajax({
            url: '/Product/AddReview',
            type: 'POST',
            data: {
                review: review,
                productId: productIdDetails,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                refreshComments();
                $('#coment').val('');
            },
            error: function () {
            }
        });
    }

    function refreshComments() {
        $.ajax({
            url: '/Product/GetComments',
            type: 'GET',
            data: {
                productId: productIdDetails
            },
            success: function (result) {
                $('#comments-container').html(result); // Update the comments section
            },
            error: function () {
            }
        });
    }

    // -------------------------------Filter------------------------------------------
    priceRange.addEventListener('input', function () {
        selectedPrice.innerText = priceRange.value;
        var activeSizeButton = document.querySelector(".filter-button-group button.active");
        var activeColorButton = document.querySelector(".color-button.active");

        var sizeId = activeSizeButton ? $(activeSizeButton).data("size-id") : null;
        var colorId = activeColorButton ? $(activeColorButton).data("color-id") : null;
        var sortBy = sortBySelect.value;

        applyFilter(sizeId, colorId, sortBy);
    });

    sortBySelect.addEventListener('change', function () {
        selectedPrice.innerText = priceRange.value;
        var activeSizeButton = document.querySelector(".filter-button-group button.active");
        var activeColorButton = document.querySelector(".color-button.active");

        var sizeId = activeSizeButton ? $(activeSizeButton).data("size-id") : null;
        var colorId = activeColorButton ? $(activeColorButton).data("color-id") : null;
        var sortBy = sortBySelect.value;

        applyFilter(sizeId, colorId, sortBy);
    });

    sizeButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            sizeButtons.forEach(function (btn) {
                btn.classList.remove("active");
            });

            this.classList.add("active");
            var sizeId = $(this).data("size-id");

            var activeColorButton = document.querySelector(".color-button.active");
            var colorId = activeColorButton ? $(activeColorButton).data("color-id") : null;
            var sortBy = sortBySelect.value;

            applyFilter(sizeId, colorId, sortBy);
        });
    });

    colorButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            colorButtons.forEach(function (btn) {
                btn.classList.remove("active");
            });

            this.classList.add("active");
            var colorId = $(this).data("color-id");

            var activeSizeButton = document.querySelector(".filter-button-group button.active");
            var sizeId = activeSizeButton ? $(activeSizeButton).data("size-id") : null;
            var sortBy = sortBySelect.value;

            applyFilter(sizeId, colorId, sortBy);
        });
    });

    function applyFilter(sizeId, colorId, sortBy) {
        var categoryId = $('#category-id').val();
        $.ajax({
            url: '/Category/FilteredProducts',
            type: 'GET',
            data: {
                categoryId: categoryId,
                price: priceRange.value,
                sizeId: sizeId,
                colorId: colorId,
                sortBy: sortBy,
            },
            success: function (result) {
                $('#filtered-products').html(result);
            },
            error: function () {
            }
        });
    }

    var clearButton = document.getElementById('clearFilters');

    clearButton.addEventListener('click', function () {
        sortBySelect.selectedIndex = 0;

        priceRange.value = 5000;
        selectedPrice.innerText = '5000';

        sizeButtons.forEach(function (button) {
            button.classList.remove('active');
        });

        colorButtons.forEach(function (button) {
            button.classList.remove('active');
        });

        applyFilter(null, null, 'Sort by');
    });

});
