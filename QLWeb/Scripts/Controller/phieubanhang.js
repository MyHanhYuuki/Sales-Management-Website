$(document).ready(function () {
    var orderItems = [];
    //Button thêm sản phẩm để kiểm tra
    $('#add').click(function () {
        //Kiểm tra sản phẩm chọn
        var isValidItem = true;

        if ($('#tenHangHoa').val() == '') {
            isValidItem = false;
            $('#productItemError').text('Chưa có sản phẩm nào được chọn!');
        }
        else {
            $('#productItemError').hide();
        }

        var errorQuantity = 0;
        errorQuantity = CheckQuantity(errorQuantity);
        var error = errorQuantity;

        //Thêm sản phẩm
        if (isValidItem == true && error == 0) {

            var i, j;
            var string_value_product = $('#maHangHoa').val().trim();

            var productID = string_value_product.slice(0, 10); // SS00001

            if (orderItems.length > 0) {
                var test = true;
                var productIdOfTable = "";
                var row = document.getElementById('productTable').rows.length;
                for (var i = 1; i < row; i++) {
                    productIdOfTable = document.getElementById("productTable").rows[i].cells[0].innerHTML;

                    if (productIdOfTable == productID) {
                        test = false;
                        $('#maHangHoa').siblings('span.error').css('visibility', 'visible');
                        break;
                    }
                }

                if (test == true) {
                    $('#maHangHoa').siblings('span.error').css('visibility', 'hidden');
                    orderItems.push({
                        MaHangHoa: $('#maHangHoa').val().trim(),
                        TenHangHoa: $("#tenHangHoa").val().trim(),
                        Gia: $('#gia').val().trim().replace(/,/gi, ""),
                        ThanhTien: $('#thanhTien').val().trim().replace(/,/gi, ""),

                        DonViTinh: $('#donViTinh').val().trim(),
                        SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                        soLuong: parseInt($('#soLuong').val().trim()),
                    });

                    //Xóa trắng 
                    $('#maHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#gia').val('');
                    $('#donViTinh').val('');
                    $('#soLuongHienTai').val('');
                    $('#soLuong').val('');

                    //Điền lại vào phiếu
                    GeneratedItemsTable();
                    TinhTongTien();
                }
            }

            if (orderItems.length == 0) {
                $('#productStock').siblings('span.error').css('visibility', 'hidden');
                orderItems.push({
                    MaHangHoa: $('#maHangHoa').val().trim(),
                    TenHangHoa: $('#tenHangHoa').val().trim(),
                    Gia: $('#gia').val().trim().replace(/,/gi, ""),
                    ThanhTien: $('#thanhTien').val().trim().replace(/,/gi, ""),
                    DonViTinh: $('#donViTinh').val().trim(),
                    SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                    soLuong: parseInt($('#soLuong').val().trim()),
                });

                //Xóa trắng 
                $('#maHangHoa').focus().val('');
                $('#tenHangHoa').val('');
                $('#gia').val('');
                $('#thanhTien').val('');
                $('#donViTinh').val('');
                $('#soLuongHienTai').val('');
                $('#soLuong').val('');


                //Điền lại vào phiếu
                GeneratedItemsTable();
                TinhTongTien();
            }
        }
    });


    $('#print').click(function () {
        Print();
    });

    //Phương thức in phiếu Bán hàng
    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%; text-align:center"/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Số lượng</th><th>Giá</th><th>Thành tiền</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.soLuong));
            $row.append($('<td/>').html(formatNumber(val.Gia)));
            $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=500'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Bán hàng</p>')

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">')
        popupWin.document.write('Thông tin phiếu Bán hàng');
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu Bán hàng: ');
        popupWin.document.write($('#soPhieuBanHang').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<td>')
        popupWin.document.write('Ngày bán: ');
        popupWin.document.write($('#ngayBanHang').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#tenNhanVien').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Khách hàng: ');
        popupWin.document.write($('#tenKhachHang').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số điện thoại: ');
        popupWin.document.write($('#soDienThoai').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<td>')
        popupWin.document.write('Ghi chú: ');
        popupWin.document.write($('#ghiChu').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br>');
        popupWin.document.write('Danh sách sản phẩm');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:center">')
        popupWin.document.write('Nhân viên bán hàng')
        popupWin.document.write('<br>')
        popupWin.document.write('(Ký tên)')
        popupWin.document.write('</p>')
        popupWin.document.write('</html>');
        popupWin.document.close();
    }

    //Save button click function
    $('#submit').click(function () {
        //Kiểm tra xem có sản phẩm nào được thêm chưa
        var isAllValid = true;
        var errorQuantity = 0;
        errorQuantity = CheckTenKhachHang(errorQuantity);
        errorQuantity = CheckSoDienThoai(errorQuantity);
        error = errorQuantity;
        if (orderItems.length == 0) {
            $('#orderItems').html('<span class="messageError" style="color:red;">Phải có ít nhất 1 hàng hóa</span>');
            isAllValid = false;
        }

        //Nếu có sản phẩm rồi thì tạo object rồi gọi Ajax
        if (isAllValid && error == 0) {
            var data = {
                soPhieuBanHang: $('#soPhieuBanHang').val().trim(),
                ngayBan: $('#ngayBanHang').val().trim(),
                tenNhanVien: $('#tenNhanVien').val().trim(),
                tenKhachHang: $('#tenKhachHang').val().trim(),
                soDienThoai: $('#soDienThoai').val().trim(),
                tongTien: $('#tongTien').val().trim().replace(/,/gi, ""),
                ghiChu: $('#ghiChu').val().trim(),
                chiTietPhieuBanHang: orderItems
            }

            $(this).val('Please wait...');

            $.ajax({
                url: "/BanHang/LuuPhieuBanHang",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //Kiểm tra nếu thành công thì lưu vô database
                    if (d.status == true) {

                        orderItems = [];
                        $('#soPhieuBanHang').val('');
                        $('#ngayBanHang').val('');
                        $('#soLuong').val('');
                        $('#gia').val('');
                        $('#thanhTien').val('');
                        $('#tenNhanVien').val('');
                        $('#ghiChu').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Admin/BanHang/';
                    }
                    else {
                        SetAlert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Bán Hàng');
                }
            });
        }
    });
    //Show sản phẩm được thêm
    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable" class="table table-bordered" />');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Số Lượng</th><th>Giá</th><th>Thành tiền</th><th>Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.soLuong));
                $row.append($('<td/>').html(formatNumber(val.Gia)));
                $row.append($('<td/>').html(formatNumber(val.ThanhTien)));
                var $remove = $('<input type="button" value="Xóa" style="padding:1px 20px" class="btn-danger"/>');
                $remove.click(function (e) {
                    e.preventDefault();
                    orderItems.splice(i, 1);
                    GeneratedItemsTable();

                    if (orderItems.length == 0) {
                        $('#tongTien').val(0);
                    } else {
                        TinhTongTien();
                    }

                    ClearValue();
                    $('#maHangHoa').focus().val('');
                });
                $row.append($('<td/>').html($remove));
                $tbody.append($row);

            });
            console.log("current", orderItems);
            $table.append($tbody);
            $('#orderItems').html($table);
        }
        else {
            $('#orderItems').html('');
        }
    }

    function TinhTongTien() {
        var amount;

        var total = 0.0;
        var row = document.getElementById('productTable').rows.length;
        for (var i = 1; i < row; i++) {
            amount = document.getElementById("productTable").rows[i].cells[4].innerHTML.replace(/,/gi, "");

            total += parseFloat(amount);
        }
        $('#tongTien').val(formatNumber(parseFloat(total)));
    }
});

function formatNumber(num) {
    //Cach 3 so 0 la 1 dau phay
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

// Phương thức chỉ nhập vào số
function checkNumber(e, element) {
    var charcode = (e.which) ? e.which : e.keyCode;
    if (charcode > 31 && (charcode < 48 || charcode > 57)) {
        return false;
    }
    return true;
}

function formatNumber(num) {
    //Cach 3 so 0 la 1 dau phay
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

//Ẩn lỗi khi user điền vào input text
function HideErrorProductName() {
    if (document.getElementById('tenHangHoa').value != '') {
        $('#tenHangHoa').siblings('span.error').css('visibility', 'hidden');
    }
}

$(document).ready(function () {
    $('#maHangHoa').ready(function () {
        $.getJSON('/BanHang/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#donViTinh").val(row.DonViTinh);
                        $("#gia").val(formatNumber(row.GiaBan));
                    });
                }
            });
    });
})

$(document).ready(function () {
    $('#maHangHoa').on("change", function () {
        $.getJSON('/BanHang/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#donViTinh").val(row.DonViTinh);
                        $("#gia").val(formatNumber(row.GiaBan));
                    });
                }
            });
    });
})

$(document).ready(function () {
    $("#soLuong").on('keyup keydown', function () {
        CheckQuantity();
        $("#thanhTien").val(formatNumber($("#soLuong").val() * $("#gia").val().replace(/,/gi, "")));
    });
});

// Kiểm tra số lượng
function CheckQuantity(error) {
    var soLuongBan = parseInt($('#soLuong').val());
    var soLuongHienTai = parseInt($('#soLuongHienTai').val());
    if ($('#soLuong').val() == 0 || (!($('#soLuong').val().trim() != '' && !isNaN($('#soLuong').val().trim())))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuong").addClass("error");
        error++;
    }
    else {
        if (soLuongBan > soLuongHienTai) {
            $(".messageErrorinputQuantity").text("Số lượng không hợp lệ!");
            $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
            $("#soLuong").addClass("error");
            error++;
        }
        else {
            $(".notifyinputQuantity").addClass("hidden");
            $("#soLuong").removeClass("error");
        }
    }

    return error;
}

$(document).ready(function () {
    $("#tenKhachHang").on('keyup keydown', function () {
        CheckTenKhachHang();
    });
});

// Kiểm tra tên khách hàng
function CheckTenKhachHang(error) {
    if ($('#tenKhachHang').val() == '') {
        $(".messageErrorinputTenKhachHang").text("Vui lòng nhập tên khách hàng!");
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

$(document).ready(function () {
    $("#soDienThoai").on('keyup keydown', function () {
        CheckSoDienThoai();
    });
});

// Kiểm tra số điện thoại
function CheckSoDienThoai(error) {
    if ($('#soDienThoai').val() == '') {
        $(".messageErrorinputSoDienThoai").text("Vui lòng nhập số điện thoại!");
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