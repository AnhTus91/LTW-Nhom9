document.addEventListener('DOMContentLoaded', function() {
    var toggleButton = document.querySelector('.navbar-toggle');
    var sidebar = document.getElementById('sidebar');
    var overlay = document.querySelector('.sidebar-overlay');

    // Toggle sidebar when button is clicked
    toggleButton.addEventListener('click', function() {
        sidebar.classList.toggle('active');
        overlay.classList.toggle('active');
    });

    // Close sidebar when overlay is clicked
    overlay.addEventListener('click', function() {
        sidebar.classList.remove('active');
        overlay.classList.remove('active');
    });
});