const currentURL = window.location.href;

const navLinks = document.querySelectorAll('.navbar li a');
navLinks.forEach(link => {

    if (link.href === currentURL) {
        link.classList.add('active');
    } else {
        link.classList.remove('active');
    }
});