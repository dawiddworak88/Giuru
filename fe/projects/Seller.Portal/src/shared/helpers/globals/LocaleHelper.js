import moment from "moment";
import "moment/locale/de";
import "moment/locale/pl";
import "moment/locale/de";

export default class LocaleHelper {

    static setMomentLocale(locale) {

        moment.locale(locale);
    }
}