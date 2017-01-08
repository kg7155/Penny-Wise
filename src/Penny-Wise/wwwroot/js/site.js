function confirmAndDelete() {
    var result = confirm("Are you sure you want to delete your account?");
    if (result) {
        window.location = "/Profile/DeleteAccount";
    }
}