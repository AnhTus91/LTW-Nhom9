document.addEventListener('DOMContentLoaded', function() {
    var toggleButton = document.querySelector('.navbar-toggle');
    var sidebar = document.getElementById('sidebar');
    var overlay = document.querySelector('.sidebar-overlay');

    //Nếu như nhấn vào nút
    toggleButton.addEventListener('click', function () {
        //Thì thêm hoặc gỡ bỏ lớp CSS active của sidebar để mở hoặc đóng search sidebar
        sidebar.classList.toggle('active');
        overlay.classList.toggle('active');
    });

    //Nếu như nhấn vào bên ngoài sidebar, nhấn vào lớp phủ bên ngoài
    overlay.addEventListener('click', function () {
        //Thì ngược lại phía trên, ẩn đi menu sidebar
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
    });
});