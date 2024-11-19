document.addEventListener("DOMContentLoaded", function () {
    const searchToggle = document.getElementById("searchToggle");
    const searchSidebar = document.getElementById("searchSidebar");
    const overlay = document.getElementById("overlay");

    searchToggle.addEventListener("click", function () {
        searchSidebar.classList.toggle("active");
        overlay.classList.toggle("active");
    });

    overlay.addEventListener("click", function () {
        searchSidebar.classList.remove("active");
        overlay.classList.remove("active");
    });
});
