const parts = require("./config/webpack.parts");
const path = require("path");

module.exports = {
    ...parts.defaultWebpackConfiguration(),
    entry: {
        registerpage: ["./src/project/Account/areas/Accounts/pages/Register/index.js", "./src/project/Account/areas/Accounts/pages/Register/RegisterPage.scss"],
        resetpasswordpage: ["./src/project/Account/areas/Accounts/pages/ResetPassword/index.js", "./src/project/Account/areas/Accounts/pages/ResetPassword/ResetPasswordPage.scss"],
        signinpage: ["./src/project/Account/areas/Accounts/pages/SignIn/index.js", "./src/project/Account/areas/Accounts/pages/SignIn/SignInPage.scss"],
        setpasswordpage: ["./src/project/Account/areas/Accounts/pages/SetPassword/index.js", "./src/project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage.scss"],
        contentpage: ["./src/project/Account/areas/Home/pages/Content/index.js", "./src/project/Account/areas/Home/pages/Content/ContentPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
        filename: "[name].js"
    }
}