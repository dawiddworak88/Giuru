export default class NavigationHelper {

    static redirect(url) {

        if (typeof window !== "undefined") {
        
            window.location.href = url;
        }
    }

    static openInNewTab(url) {

        if (typeof window !== "undefined") {
            window.open(url, "_blank");
        }
    }
}
