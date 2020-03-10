export default class PasswordValidator {

    static validateFormat(password) {

        var format = new RegExp("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

        if (format.test(password)) {

            return true;
        }

        return false;
    }
}