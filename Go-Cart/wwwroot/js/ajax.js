$(document).ready(function () {
    var searchInput = $('#searchInput');

    searchInput.autocomplete({
        source: [],
        minLength: 0,
        appendTo: searchInput.parent(),
    });

    searchInput.on('input', function () {
        var searchTerm = searchInput.val();

        $.ajax({
            url: '@Url.Action("AutoComplete", "Product")',
            type: 'GET',
            data: { searchTerm: searchTerm },
            success: function (response) {
                searchInput.autocomplete('option', 'source', response);

                searchInput.autocomplete('search', '');
            }
        });
    });
});
