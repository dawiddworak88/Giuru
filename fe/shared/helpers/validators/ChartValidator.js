export default class ChartValidator {

    static validate(fromDate, toDate) {
        if (fromDate > toDate || fromDate > new Date()) {
            return false
        }

        return true;
    }
}