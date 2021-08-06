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
                        DonViTinh: $('#donViTinh').val().trim(),
                        SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                        SoLuongKiemTra: parseInt($('#soLuongKiemTra').val().trim()),
                    });

                    //Xóa trắng 
                    $('#maHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#donViTinh').val('');
                    $('#soLuongHienTai').val('');
                    $('#soLuongKiemTra').val('');

                    //Điền lại vào phiếu
                    GeneratedItemsTable();
                }
            }

            if (orderItems.length == 0) {
                $('#productStock').siblings('span.error').css('visibility', 'hidden');
                orderItems.push({
                    MaHangHoa: $('#maHangHoa').val().trim(),
                    TenHangHoa: $("#tenHangHoa").val().trim(),
                    DonViTinh: $('#donViTinh').val().trim(),
                    SoLuongHienTai: parseInt($('#soLuongHienTai').val().trim()),
                    SoLuongKiemTra: parseInt($('#soLuongKiemTra').val().trim()),
                });

                //Xóa trắng 
                $('#maHangHoa').focus().val('');
                $('#tenHangHoa').val('');
                $('#donViTinh').val('');
                $('#soLuongHienTai').val('');
                $('#soLuongKiemTra').val('');


                //Điền lại vào phiếu
                GeneratedItemsTable();
            }
        }
    });


    $('#print').click(function () {
        Print();
    });

    //Phương thức in phiếu kiểm kho
    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%; text-align:center"/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Hiện Có</th><th>Số Lượng Kiểm Tra</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaNhanVien));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuongHienTai));
            $row.append($('<td/>').html(val.SoLuongKiemTra));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=500'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Kiểm Kho</p>')

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">')
        popupWin.document.write('Thông tin phiếu kiểm kho');
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu kiểm kho: ');
        popupWin.document.write($('#soPhieuKiemKho').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<td>')
        popupWin.document.write('Ngày kiểm: ');
        popupWin.document.write($('#ngayKiemKho').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#tenNhanVien').val().trim());
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
        popupWin.document.write('Nhân viên kho')
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
        if (orderItems.length == 0) {
            $('#orderItems').html('<span class="messageError" style="color:red;">Phải có ít nhất 1 hàng hóa</span>');
            isAllValid = false;
        }

        //Nếu có sản phẩm rồi thì tạo object rồi gọi Ajax
        if (isAllValid) {
            var data = {
                SoPhieuKiemKho: $('#soPhieuKiemKho').val().trim(),
                NgayKiemKho: $('#ngayKiemKho').val().trim(),
                MaNhanVien: $('#maNhanVien').val().trim(),
                GhiChu: $('#ghiChu').val().trim(),
                chiTietPhieuKiemKho: orderItems
            }

            $(this).val('Please wait...');

            $.ajax({
                url: "/KiemKho/LuuPhieuKiemKho",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //Kiểm tra nếu thành công thì lưu vô database
                    if (d.status == true) {

                        orderItems = [];
                        $('#soPhieuKiemKho').val('');
                        $('#ngayKiemKho').val('');
                        $('#tenNhanVien').val('');
                        $('#ghiChu').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Admin/KiemKho/';
                    }
                    else {
                        SetAlert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Kiểm Kho');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Kiểm Kho');
                }
            });
        }
    });
    //Show sản phẩm được thêm
    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable" class="table table-bordered" />');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Hiện Có</th><th>Số Lượng Kiểm Tra</th><th>Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuongHienTai));
                $row.append($('<td/>').html(val.SoLuongKiemTra));
                var $remove = $('<input type="button" value="Xóa" style="padding:1px 20px" class="btn-danger"/>');
                $remove.click(function (e) {
                    e.preventDefault();
                    orderItems.splice(i, 1);
                    GeneratedItemsTable();
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
        $.getJSON('/KiemKho/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#donViTinh").val(row.DonViTinh);
                    });
                }
            });
    });
});

$(document).ready(function () {
    $('#maHangHoa').on("change", function () {
        $.getJSON('/KiemKho/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#donViTinh").val(row.DonViTinh);
                    });
                }
            });
    });
});

$(document).ready(function () {
    $("#soLuongKiemTra").on('keyup input propertychange paste change', function () {
        CheckQuantity();
    });
});

// Kiểm tra số lượng
function CheckQuantity(error) {
    var soLuongKiemTra = parseInt($('#soLuongKiemTra').val());
    var soLuongHienTai = parseInt($('#soLuongHienTai').val());
    if ($('#soLuongKiemTra').val() == 0 || (!($('#soLuongKiemTra').val().trim() != '' && !isNaN($('#soLuongKiemTra').val().trim())))) {
        $(".messageErrorinputQuantity").text("Nhập số lượng!");
        $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
        $("#soLuongKiemTra").addClass("error");
        error++;
    }
    else {
        if (soLuongKiemTra > soLuongHienTai) {
            $(".messageErrorinputQuantity").text("Số lượng không hợp lệ!");
            $(".notifyinputQuantity").slideDown(250).removeClass("hidden");
            $("#soLuongKiemTra").addClass("error");
            error++;
        }
        else {
            $(".notifyinputQuantity").addClass("hidden");
            $("#soLuongKiemTra").removeClass("error");
        }
    }
    return error;
}