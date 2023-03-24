export default class NavigationHelper {

    static redirect(url, target = null) {

        if (typeof window !== "undefined") {
        
            if (target != null) {
                return window.open(url, target);
            }

            return window.location.href = url;
        }
    }
}
