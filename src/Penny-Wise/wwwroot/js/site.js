function confirmAndDelete() {
    var result = confirm("Are you sure you want to delete your account?");
    if (result) {
        window.location = "/Profile/DeleteAccount";
    }
}

function setCurrentDate() {
    var monthNames = ["January", "February", "March", "April", "May", "June",
                     "July", "August", "September", "October", "November", "December"
                     ];
    var element = document.getElementById("current-month");
    var d = new Date();
    element.innerHTML = monthNames[d.getMonth()] + " " + d.getFullYear();
}