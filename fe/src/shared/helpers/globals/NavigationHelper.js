export default class NavigationHelper {

    static redirect(url) {

        if (typeof window !== "undefined") {
        
            window.location.href = url;
        }
    }
}
