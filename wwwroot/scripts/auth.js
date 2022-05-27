/*
 * Call the Authentication controller.
 * This is a horrible hack in order to sign in the user.
 * Since the correct HttpContext is only available in controllers,
 * we need to call the controller from here to be able to sign in.
 */

export function SignIn(username, role, password, redirect) {
    const url = "login";
    const xhr = new XMLHttpRequest();

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
    const data = {
        username: username,
        role: role,
        password: password,
        // confirmPassword is needed because we use the same user model
        // for registration and login
        confirmPassword: password
    };

    // Call API
    xhr.send(JSON.stringify(data));
}

export function Update(newUsername, oldUsername, role, token) {
    const url = "account/manage";
    const xhr = new XMLHttpRequest();

    // Initialization
    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE 
        {
            location.reload();
        }
    };

    // Data to send
    const data = {
        newUsername: newUsername,
        oldUsername: oldUsername,
        role: role
    };

    // Call API
    xhr.send(JSON.stringify(data));
}
