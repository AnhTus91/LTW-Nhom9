function maxLengthText(text, maxLength) {
    if (text.length > maxLength) { //Kiểm tra nếu độ dài của tên lớn hơn maxLength
        return text.substring(0, maxLength) + '...';   //Thì rút gọn tên và thêm 3 chấm
    } else {
        return text; //Còn không thì để nguyên
    }
}

// Lấy tất cả các phần tử có class là "product-name"
const productNames = document.querySelectorAll('.product-name');

// Áp dụng hàm truncateText cho từng phần tử
productNames.forEach(productName => { //Xài hàm forEach để lướt hết các items trong productName
    const truncatedText = maxLengthText(productName.textContent, 28); //Gọi lại hàm maxLengthText để rút gọn tên xuống 28 kí tự (nếu nó dài hơn 28 kí tự)
    productName.textContent = truncatedText;
});

const header = document.getElementById('Header');

window.addEventListener('scroll', () => {
    const scrollPosition = window.scrollY;
    const headerHeight = header.offsetHeight;

    if (scrollPosition >= headerHeight) {
        header.classList.add('fixed');

    } else {
        header.classList.remove('fixed');
    }
});