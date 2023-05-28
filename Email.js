// Get the email link element by its id
const emailLink = document.getElementById('emailLink');

// Add an event listener for the click event
emailLink.addEventListener('click', function (event) {
    event.preventDefault(); // Prevent the default action (opening the mail client)
    window.open('https://mail.google.com', '_blank'); // Open Gmail in a window
});

