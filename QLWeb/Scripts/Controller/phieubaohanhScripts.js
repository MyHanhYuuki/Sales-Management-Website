$(document).ready(function () {
    var orderItems = [];

    $('#print').click(function () {
        Print();
    });

    //Phương thức in phiếu bảo hành
    function Print() {
        var popupWin = window.open('', '_blank', 'width=800,height=500'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">');

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Bảo Hành</p>');

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">');
        popupWin.document.write('Thông tin phiếu bảo hành');
        popupWin.document.write('<tr><td>');
        popupWin.document.write('Số phiếu bảo hành: ');
        popupWin.document.write($('#soPhieuBaoHanh').val().trim());
        popupWin.document.write('</td>');

        popupWin.document.write('<td>');
        popupWin.document.write('Ngày lập: ');
        popupWin.document.write($('#ngayLap').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('<tr><td>');
        popupWin.document.write('Ngày giao: ');
        popupWin.document.write($('#ngayGiao').val().trim());
        popupWin.document.write('</td>');

        popupWin.document.write('<td>');
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#tenNhanVien').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('<tr><td>');
        popupWin.document.write('Tên khách hàng: ');
        popupWin.document.write($('#tenKhachHang').val().trim());
        popupWin.document.write('</td>');

        popupWin.document.write('<td>');
        popupWin.document.write('Số điện thoại: ');
        popupWin.document.write($('#soDienThoai').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('<tr><td>');
        popupWin.document.write('Lý do bảo hành: ');
        popupWin.document.write($('#ghiChu').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('</table>');

        popupWin.document.write('<br>');
        popupWin.document.write('Thông tin sản phẩm bảo hành');

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">');
        popupWin.document.write('<tr><td>');
        popupWin.document.write('Mã sản phẩm: ');
        popupWin.document.write($('#maHangHoa').val().trim());
        popupWin.document.write('</td>');

        popupWin.document.write('<td>');
        popupWin.document.write('Tên sản phẩm: ');
        popupWin.document.write($('#tenHangHoa').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('<tr><td>');
        popupWin.document.write('Model name: ');
        popupWin.document.write($('#modelName').val().trim());
        popupWin.document.write('</td></tr>');

        popupWin.document.write('</table>')

        popupWin.document.write('<p style="text-align:center">');
        popupWin.document.write('Nhân viên bảo hành');
        popupWin.document.write('<br>');
        popupWin.document.write('(Ký tên)');
        popupWin.document.write('</p>');
        popupWin.document.write('</html>');
        popupWin.document.close();
    }

    //Save button click function
    $('#submit').click(function () {
        var errorQuantity = 0;
        errorQuantity = CheckNgayGiao(errorQuantity);
        errorQuantity = CheckTenKhachHang(errorQuantity);
        errorQuantity = CheckSoDienThoai(errorQuantity);
        var error = errorQuantity;
        if (error == 0) {
            orderItems.push({
                MaHangHoa: $('#maHangHoa').val().trim(),
            });

            var data = {
                SoPhieuBaoHanh: $('#soPhieuBaoHanh').val().trim(),
                NgayLap: $('#ngayLap').val().trim(),
                NgayGiao: $('#ngayGiao').val().trim(),
                MaNhanVien: $('#maNhanVien').val().trim(),
                TenKhachHang: $('#tenKhachHang').val().trim(),
                SoDienThoai: $('#soDienThoai').val().trim(),
                GhiChu: $('#ghiChu').val().trim(),
                chiTietPhieuBaoHanh: orderItems
            }

            $(this).val('Please wait...');

            $.ajax({
                url: "/BaoHanh/LuuPhieuBaoHanh",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //Kiểm tra nếu thành công thì lưu vô database
                    if (d.status == true) {
                        orderItems = [];
                        $('#soPhieuBaoHanh').val('');
                        $('#ngayLap').val('');
                        $('#ngayGiao').val('');
                        $('#tenNhanVien').val('');
                        $('#tenKhachHang').val('');
                        $('#soDienThoai').val('');
                        $('#ghiChu').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Admin/BaoHanh/';
                    }
                    else {
                        SetAlert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Bảo Hành');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Bảo Hành');
                }
            });
        }
    })
});


// Phương thức chỉ nhập vào số
function checkNumber(e, element) {
    var charcode = (e.which) ? e.which : e.keyCode;
    if (charcode > 31 && (charcode < 48 || charcode > 57)) {
        return false;
    }
    return true;
}

//Ẩn lỗi khi user điền vào input text
function HideErrorProductName() {
    if (document.getElementById('tenHangHoa').value != '') {
        $('#tenHangHoa').siblings('span.error').css('visibility', 'hidden');
    }
}

$(document).ready(function () {
    $('#maHangHoa').ready(function () {
        $.getJSON('/BaoHanh/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#modelName").val(row.ModelName);
                    });
                }
            });
    })
});

$(document).ready(function () {
    $('#maHangHoa').on("change", function () {
        $.getJSON('/BaoHanh/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#modelName").val(row.ModelName);
                    });
                }
            });
    });
});

$(document).ready(function () {
    $('#ngayGiao').on("keyup input propertychange paste change", function () {
        CheckNgayGiao();
    });

    $('#tenKhachHang').on("keyup input propertychange paste change", function () {
        CheckTenKhachHang();
    });

    $('#soDienThoai').on("keyup input propertychange paste change", function () {
        CheckSoDienThoai();
    });
});

 //Kiểm tra ràng buôc
function CheckNgayGiao(error) {

    if (!($('#ngayGiao').val().trim() != '')) {
        $(".messageErrorinputNgayGiao").text("Nhập ngày giao!");
        $(".notifyinputNgayGiao").slideDown(250).removeClass("hidden");
        $("#checkNgayGiao").addClass("error");
        error++;
    }
    else {
        $(".notifyinputNgayGiao").addClass("hidden");
        $("#checkNgayGiao").removeClass("error");
    }
    return error;
}

function CheckTenKhachHang(error) {
    if (!($('#tenKhachHang').val().trim() != '')) {
        $(".messageErrorinputTenKhachHang").text("Nhập tên khách hàng!");
        $(".notifyinputTenKhachHang").slideDown(250).removeClass("hidden");
        $("#checkTenKhachHang").addClass("error");
        error++;
    }
    else {
        $(".notifyinputTenKhachHang").addClass("hidden");
        $("#checkTenKhachHang").removeClass("error");
    }
    return error;
}

function CheckSoDienThoai(error) {
    if (!($('#soDienThoai').val().trim() != '')) {
        $(".messageErrorinputSoDienThoai").text("Nhập số điện thoại!");
        $(".notifyinputSoDienThoai").slideDown(250).removeClass("hidden");
        $("#checkSoDienThoai").addClass("error");
        error++;
    }
    else {
        $(".notifyinputSoDienThoai").addClass("hidden");
        $("#checkSoDienThoai").removeClass("error");
    }
    return error;
}