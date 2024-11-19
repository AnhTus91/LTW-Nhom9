document.addEventListener("DOMContentLoaded", function () {
    //Lấy tham chiếu đến nút dùng để mở search sidebar dựa vào id = "searchToggle"
    const searchToggle = document.getElementById("searchToggle");
    //Lấy tham chiếu đến phần tử sidebar tìm kiếm dựa vào id = "searchSidebar"
    const searchSidebar = document.getElementById("searchSidebar");
    //Lấy tham chiếu đến phần tử lớp phủ(overlay) dựa vào id = "overlay".Phần tử này dùng để làm mờ nội dung phía sau sidebar khi nó mở
    const overlay = document.getElementById("overlay");

    //Nếu như nhấn vào nút
    searchToggle.addEventListener("click", function () {
         //Thì thêm hoặc gỡ bỏ lớp CSS active của sidebar để mở hoặc đóng menu sidebar
        searchSidebar.classList.toggle("active");
        overlay.classList.toggle("active");
    });

    //Nếu như nhấn vào bên ngoài sidebar, nhấn vào lớp phủ bên ngoài
    overlay.addEventListener("click", function () {
        //Thì ngược lại phía trên, ẩn đi search sidebar
        searchSidebar.classList.remove("active");
        overlay.classList.remove("active");
    });
});
