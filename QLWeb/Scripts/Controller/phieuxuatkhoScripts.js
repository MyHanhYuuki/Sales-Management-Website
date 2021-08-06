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
        errorQuantity = CheckSoLuongXuat(errorQuantity);
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
                        SoLuong: parseInt($('#soLuongXuat').val().trim()),
                        Gia: parseInt($('#giaBan').val().trim()),
                        ThanhTien: parseInt($('#soLuongXuat').val().trim()) * parseInt($('#giaBan').val().trim().replace(/,/gi, "")),
                    });

                    //Xóa trắng 
                    $('#maHangHoa').focus().val('');
                    $('#tenHangHoa').val('');
                    $('#donViTinh').val('');
                    $('#soLuongHienTai').val('');
                    $('#soLuongXuat').val('');
                    $('#giaBan').val('');
                    $('#thanhTien').val('');

                    //Điền lại vào phiếu
                    GeneratedItemsTable();
                    TinhTongTien();
                }
            }

            if (orderItems.length == 0) {
                $('#productStock').siblings('span.error').css('visibility', 'hidden');
                orderItems.push({
                    MaHangHoa: $('#maHangHoa').val().trim(),
                    TenHangHoa: $("#tenHangHoa").val().trim(),
                    DonViTinh: $('#donViTinh').val().trim(),
                    SoLuong: parseInt($('#soLuongXuat').val().trim()),
                    Gia: parseInt($('#giaBan').val().trim()),
                    ThanhTien: parseInt($('#soLuongXuat').val().trim()) * parseInt($('#giaBan').val().trim().replace(/,/gi, "")),
                });

                //Xóa trắng 
                $('#maHangHoa').focus().val('');
                $('#tenHangHoa').val('');
                $('#donViTinh').val('');
                $('#soLuongHienTai').val('');
                $('#soLuongXuat').val('');
                $('#giaBan').val('');
                $('#thanhTien').val('');

                //Điền lại vào phiếu
                GeneratedItemsTable();
                TinhTongTien();
            }
        }
    });

    $('#print').click(function () {
        Print();
    });

    //Phương thức in phiếu xuất kho
    function Print() {
        var toPrint = document.getElementById('Items');
        var $table = $('<table id="productTables" style="border: solid; width:100%; text-align:center"/>');
        $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Xuất</th><th>Giá Bán</th><th>Thành Tiền</th></tr></thead>');
        var $tbody = $('<tbody/>');
        $.each(orderItems, function (i, val) {
            var $row = $('<tr style="border:solid">');
            $row.append($('<td/>').html(val.MaHangHoa));
            $row.append($('<td/>').html(val.TenHangHoa));
            $row.append($('<td/>').html(val.DonViTinh));
            $row.append($('<td/>').html(val.SoLuong));
            $row.append($('<td/>').html(val.Gia));
            $row.append($('<td/>').html(val.ThanhTien));
            $tbody.append($row);
        });
        console.log("current", orderItems);
        $table.append($tbody);
        $('#Items').html($table);

        var popupWin = window.open('', '_blank', 'width=800,height=500'); //create new page     
        popupWin.document.open(); //open new page
        popupWin.document.write('<html><body onload="window.print()">')

        popupWin.document.write('<p style="text-align:center; font-weight: bold; font-size: 30px">Phiếu Xuất Kho</p>')

        popupWin.document.write('<table style="border:solid; width:100%"; text-align:center">')
        popupWin.document.write('Thông tin phiếu xuất kho');
        popupWin.document.write('<tr><td>')
        popupWin.document.write('Số phiếu xuất kho: ');
        popupWin.document.write($('#soPhieuXuatKho').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<td>')
        popupWin.document.write('Ngày xuất: ');
        popupWin.document.write($('#ngayXuat').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<tr><td>')
        popupWin.document.write('Nhân viên: ');
        popupWin.document.write($('#tenNhanVien').val().trim());
        popupWin.document.write('</td>')

        popupWin.document.write('<td>')
        popupWin.document.write('Tổng tiền: ');
        popupWin.document.write($('#tongTien').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('<td>')
        popupWin.document.write('Lý do xuất: ');
        popupWin.document.write($('#lyDoXuat').val().trim());
        popupWin.document.write('</td></tr>')

        popupWin.document.write('</table>')

        popupWin.document.write('<br>');
        popupWin.document.write('Danh sách sản phẩm');
        popupWin.document.write(toPrint.innerHTML);

        popupWin.document.write('<p style="text-align:center">')
        popupWin.document.write('Nhân viên xuất')
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
                SoPhieuXuatKho: $('#soPhieuXuatKho').val().trim(),
                NgayXuat: $('#ngayXuat').val().trim(),
                MaNhanVien: $('#maNhanVien').val().trim(),
                TongTien: parseFloat($('#tongTien').val().trim().replace(/,/gi, "")),
                LyDoXuat: $('#lyDoXuat').val().trim(),
                chiTietPhieuXuatKho: orderItems
            }

            $(this).val('Please wait...');

            $.ajax({
                url: "/XuatKho/LuuPhieuXuatKho",
                type: "POST",
                data: JSON.stringify(data),
                dataType: "JSON",
                contentType: "application/json",
                success: function (d) {
                    //Kiểm tra nếu thành công thì lưu vô database
                    if (d.status == true) {

                        orderItems = [];
                        $('#soPhieuXuatKho').val('');
                        $('#ngayXuat').val('');
                        $('#tenNhanVien').val('');
                        $('#lyDoXuat').val('');
                        $('#orderItems').empty();
                        window.location.href = '/Admin/XuatKho/';
                    }
                    else {
                        SetAlert("Something wrong! Please try again", "error");
                    }
                    $('#submit').val('Lưu Phiếu Xuất Kho');
                },
                error: function () {
                    alert('Error. Please try again.');
                    $('#submit').val('Lưu Phiếu Xuất Kho');
                }
            });
        }
    });

    //Show sản phẩm được thêm
    function GeneratedItemsTable() {
        if (orderItems.length > 0) {
            var $table = $('<table id="productTable" class="table table-bordered" />');
            $table.append('<thead><tr><th>Mã Hàng Hóa</th><th>Tên Hàng Hóa</th><th>Đơn Vị Tính</th><th>Số Lượng Xuất</th><th>Giá Bán</th><th>Thành Tiền</th><th>Hành Động</th></tr></thead>');
            var $tbody = $('<tbody/>');
            $.each(orderItems, function (i, val) {
                var $row = $('<tr/>');
                $row.append($('<td/>').html(val.MaHangHoa));
                $row.append($('<td/>').html(val.TenHangHoa));
                $row.append($('<td/>').html(val.DonViTinh));
                $row.append($('<td/>').html(val.SoLuong));
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
            amount = document.getElementById("productTable").rows[i].cells[5].innerHTML.replace(/,/gi, "");

            total += parseFloat(amount);
        }
        $('#tongTien').val(formatNumber(parseFloat(total)));
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
        $.getJSON('/XuatKho/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#donViTinh").val(row.DonViTinh);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#giaBan").val(row.GiaBan);
                    });
                }
            });
    })
});

$(document).ready(function () {
    $('#maHangHoa').on("change", function () {
        $.getJSON('/XuatKho/LoadThongTinHangHoa',
            { id: $('#maHangHoa').val() },
            function (data) {
                if (data != null) {
                    $.each(data, function (index, row) {
                        $("#tenHangHoa").val(row.TenHangHoa);
                        $("#donViTinh").val(row.DonViTinh);
                        $("#soLuongHienTai").val(row.SoLuongTon);
                        $("#giaBan").val(row.GiaBan);
                    });
                }
            });
    });
});

function formatNumber(num) {
    //Cach 3 so 0 la 1 dau phay
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}


$(document).ready(function () {
    $("#soLuongXuat").on('keydown input propertychange paste change', function () {
        CheckSoLuongXuat();
    });
});

// Kiểm tra số lượng nhập
function CheckSoLuongXuat(error) {
    var soLuongXuat = parseInt($('#soLuongXuat').val());
    var soLuongHienTai = parseInt($('#soLuongHienTai').val());
    if ($('#soLuongXuat').val() == 0 || (!($('#soLuongXuat').val().trim() != '' && !isNaN($('#soLuongXuat').val().trim())))) {
        $(".messageErrorinputSoLuongXuat").text("Nhập số lượng!");
        $(".notifyinputSoLuongXuat").slideDown(250).removeClass("hidden");
        $("#soLuongXuat").addClass("error");
        error++;
    }
    else {
        if (soLuongXuat > soLuongHienTai) {
            $(".messageErrorinputSoLuongXuat").text("Số lượng không hợp lệ!");
            $(".notifyinputSoLuongXuat").slideDown(250).removeClass("hidden");
            $("#soLuongXuat").addClass("error");
            error++;
        }
        else {
            $(".notifyinputSoLuongXuat").addClass("hidden");
            $("#soLuongXuat").removeClass("error");
        }
    }
    return error;
}

$(document).ready(function () {
    TinhThanhTien();
    $("#soLuongXuat").on("keydown keyup", function () {
        TinhThanhTien();
    });
});

function TinhThanhTien() {
    if (document.getElementById('soLuongXuat').value == '' || document.getElementById('giaBan').value == 0) {
        document.getElementById('thanhTien').value = 0;
    }
    else {
        var unitPrice = document.getElementById('giaBan').value.replace(/,/gi, "");
        var quantity = document.getElementById('soLuongXuat').value;
        var result = parseInt(unitPrice) * parseInt(quantity);
        if (!isNaN(result)) {
            document.getElementById('thanhTien').value = formatNumber(result);
        }
    }
}