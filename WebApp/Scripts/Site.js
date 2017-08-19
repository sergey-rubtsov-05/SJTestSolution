$.ajaxSetup({
    error: function(jqXhr, errorStatus, errorThrown) {
        alert((jqXhr.responseJSON && jqXhr.responseJSON.message) || errorThrown);
    }
});