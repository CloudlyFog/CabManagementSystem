let path = '../../Data/Json/taxi.json';

let taxiData = $.getJSON(path, function () {
    $.ajax({
        url: path,
        data: $.getJSON(path, function (data) {
            return data
        })
    });
});
alert(taxiData);