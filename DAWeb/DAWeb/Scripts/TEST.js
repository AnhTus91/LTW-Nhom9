<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script>
        $(function () {
            $("#searchBox").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/Product/SearchProducts", // Đường dẫn đến Action Method
                        dataType: "json",
                        data: {
                            term: request.term // Gửi từ khóa tìm kiếm
                        },
                        success: function (data) {
                            response(data); // Hiển thị gợi ý
                        }
                    });
                },
                minLength: 2, // Số ký tự tối thiểu để bắt đầu tìm kiếm
                select: function (event, ui) {
                    // Xử lý khi người dùng chọn một gợi ý
                    // Ví dụ: chuyển hướng đến trang chi tiết sản phẩm
                    window.location.href = "/Product/ChiTietSanPham/" + ui.item.id;
                }
            });
        });
    </script>