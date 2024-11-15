const menuToggle = document.getElementById('menu-toggle');
const menu = document.getElementById('mobile-menu');
let isMenuOpen = false;

menuToggle.addEventListener('click', () => {
    isMenuOpen = !isMenuOpen;
    menu.classList.toggle('is-open');
});
window.addEventListener('click', (event) => {
    if (!menu.contains(event.target) && !menuToggle.contains(event.target) && isMenuOpen) {
        isMenuOpen = false;
        menu.classList.remove('is-open');
    }
});
window.addEventListener('resize', () => {
    if (window.innerWidth > 685 && isMenuOpen) {
        isMenuOpen = false;
        menu.classList.remove('is-open');
    }
});