export default class CssSsrRemovalHelper {

    static Remove() {

        const jssStyles = document.querySelector("#jss-server-side");

        if (jssStyles) {
            jssStyles.parentElement.removeChild(jssStyles);
        }
    }
}