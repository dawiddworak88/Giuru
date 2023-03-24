export default class ClipboardHelper {

    static copyToClipboard(text) {

        if (typeof window !== "undefined") {

            var dummy = document.createElement("textarea");
            document.body.appendChild(dummy);
            dummy.value = text;
            dummy.select();
            document.execCommand("copy");
            document.body.removeChild(dummy);
        }
    }
}
