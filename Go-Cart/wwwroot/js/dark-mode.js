const darkModeToggle = document.querySelector('#moon');
const lightModeToggle = document.querySelector('#sun');
const body = document.querySelector('body');

darkModeToggle.addEventListener('click', () => {
  body.classList.add('dark-mode');
  darkModeToggle.classList.add('d-none');
    lightModeToggle.classList.remove('d-none');
    localStorage.setItem('darkMode', 'off');
});

lightModeToggle.addEventListener('click', () => {
    body.classList.remove('dark-mode');
    lightModeToggle.classList.add('d-none');
    darkModeToggle.classList.remove('d-none');
    localStorage.setItem('darkMode', 'on');
});

$(document).ready(function () {
    if (localStorage.getItem('darkMode') === 'off') {
        body.classList.add('dark-mode');
        darkModeToggle.classList.add('d-none');
        lightModeToggle.classList.remove('d-none');
    }
    else{
        body.classList.remove('dark-mode');
        lightModeToggle.classList.add('d-none');
        darkModeToggle.classList.remove('d-none');
    }
});