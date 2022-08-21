(function ($) {
    $.fn.VNSFileUploder = function () {
        this.each(function (e) {
            var fileLoder = this;
            var btn = $(fileLoder).data('btn');

            if (btn) {
                $(btn).click(function () {
                    $(fileLoder).trigger('click');
                })
            }

            var nameHolder = $(fileLoder).data('name');
            var imgPreview = $(fileLoder).data('preview');
            $(fileLoder).change(function () {
                var files = $(fileLoder).prop('files')[0];
                if (files) {
                    if (files.type.indexOf('image') < 0) {
                        $(fileLoder).val('');
                        if (nameHolder) {
                            $(nameHolder).html('');
                        }
                        return false;
                    }
                    if (imgPreview) {
                        var reader = new FileReader();

                        reader.onload = function (e) {
                            $(imgPreview).attr('src', e.target.result);
                        }

                        reader.readAsDataURL(files);
                    }
                    if (nameHolder) {
                        $(nameHolder).html(files.name)
                    }
                }
            })
        })
        return this;
    }
}(jQuery))

//var fileUploder = $('.vns-file');
//debugger;
//if (fileUploder) {
//    for (var i = 0; i < fileUploder.length; i++) {
//        var fl_btn = $(fileUploder[i]).data('btn');
//        if (fl_btn) {
//            $(fl_btn).click(function () {
//                debugger;
//                $(fileUploder[i]).trigger('click');
//            })
//        }
//        $(fileUploder[i]).change(function () {
//            debugger;
//            var files = $(fileUploder[i]).prop('files')[0];
//            if (files) {
//                if (files.type.indexOf('image') < 0) {
//                    $(fl_btn[i]).val('');
//                    return false;
//                }
//                var nameHolder = $(fl_btn[i]).data('name');
//                $(nameHolder).html(files.name)
//            }
//        })

//    }
//}

