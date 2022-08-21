
function ShowNotification(from, align, type, icon, message) {
    color = Math.floor((Math.random() * 4) + 1);

    $.notify({
        icon: icon,
        message: message

    }, {
        type: type,
        timer: 1000,
        placement: {
            from: from,
            align: align
        }
    });
}