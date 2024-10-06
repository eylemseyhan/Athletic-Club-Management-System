$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var query = $(this).val();
        $.ajax({
            url: '@Url.Action("SearchStudents", "Home")',
            type: 'GET',
            data: { searchQuery: query },
            success: function (data) {
                $('#studentTableBody').html(data);
            }
        });
    });
});