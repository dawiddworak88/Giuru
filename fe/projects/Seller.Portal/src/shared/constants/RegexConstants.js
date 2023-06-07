export default class RegexConstants {
    static onlyLetters() {
        return /^[A-Za-z]+$/;
    }

    static nonBlankAndNonEmptyString(){
        return /(.|\s)*\S(.|\s)*/;
    }
}