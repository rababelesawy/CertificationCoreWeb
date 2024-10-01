





function facebookLogin() {

    var provider = new firebase.auth.FacebookAuthProvider();
    provider.addScope('user_birthday');


    firebase.auth().signInWithPopup(provider).then(function (result) {
        // This gives you a Facebook Access Token. You can use it to access the Facebook API.
        var token = result.credential.accessToken;
        // The signed-in user info.
        var user = result.user;

        var email = user.providerData[0].email;
        var pass = user.providerData[0].uid;
        var fullName = user.providerData[0].displayName;
        var phone = (user.providerData[0].phoneNumber == null) ? "" : user.providerData[0].phoneNumber;
        var logindata = { "Email": email, "Password": pass };

        var registerdata = {
            "Email": email, "Password": pass, "Address": "addresss",
            "Phone": phone, "FullName": fullName, "ActivationCode": "",
            "ConfirmPassword": pass, "IsActive": true,
            "TypeId": "1"
        };

        //login
        loginregister(logindata, registerdata);
    });
}



function googleLognin() {

    var provider = new firebase.auth.GoogleAuthProvider();
    firebase.auth().signInWithPopup(provider).then(function (result) {

        // This gives you a Google Access Token. You can use it to access the Google API.
        var token = result.credential.accessToken;
        // The signed-in user info.
        var user = result.user;

        var email = user.providerData[0].email;
        var pass = user.providerData[0].uid;
        var fullName = user.providerData[0].displayName;
        var phone = (user.providerData[0].phoneNumber == null) ? "123" : user.providerData[0].phoneNumber;
        var logindata = { "Email": email, "Password": pass };

        var registerdata = {
            "Email": email, "Password": pass, "Address": "addresss",
            "Phone": phone, "Name": fullName, "ActivationCode": "",
            "ConfirmPassword": pass, "IsActive": true,
            "TypeId": "1"
        };


        loginregister(logindata, registerdata);

    });
}



function loginregister(logindata, registerdata) {

    $.post("Account/Login", logindata, function (res) {

        if (res === "2") { // not found
            $.post("Account/Register", registerdata, function (ress) {
                if (ress === "1") {
                    // do login
                    $.post("Account/Login", logindata, function (result) {
                        if (result === "1") {
                            window.location.href = "/";
                        }
                    });
                } 
            });

        } else {
            window.location.href = "/";
        }

    });
}







