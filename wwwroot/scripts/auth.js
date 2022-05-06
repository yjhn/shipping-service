/*
 * Call the Authentication controller
 */

export function SignIn(username, role, password, redirect) {
    var url = "login";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    // Catch response
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            if (redirect)
                location.replace(redirect);
        }
    };

    // Data to send
    var data = {
        username: username,
        role: role,
        password: password,
        confirmPassword: password
    };

    // Call API
    xhr.send(JSON.stringify(data));
}

export function Update(newUsername, oldUsername, role, token) {
    var url = "account/manage";
    var xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");
//    xhr.setRequestHeader("__RequestVerificationToken",
        //X - CSRF - TOKEN",
//        token);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            location.reload();
        }
    };

    // Data to send
    var data = {
        newUsername: newUsername,
        oldUsername: oldUsername,
        role: role
    };

    // Call API
    xhr.send(JSON.stringify(data));
}