export default class CssSsrRemovalHelper {

    static remove() {

        const jssStyles = document.querySelector("#jss-server-side");

        if (jssStyles) {
            jssStyles.parentElement.removeChild(jssStyles);
        }
    }
}
