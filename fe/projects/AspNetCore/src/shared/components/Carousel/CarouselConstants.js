export default class CarouselConstants {
    static defaultCarouselResponsive() {
        return {
            superLargeDesktop: {
                breakpoint: { max: 4000, min: 3000 },
                items: 7,
                slidesToSlide: 7
            },
            desktop: {
                breakpoint: { max: 3000, min: 1024 },
                items: 4,
                slidesToSlide: 4
            },
            tablet: {
                breakpoint: { max: 1024, min: 464 },
                items: 2,
                slidesToSlide: 2
            },
            mobile: {
                breakpoint: { max: 464, min: 0 },
                items: 1,
                slidesToSlide: 1
            }
        };
    }
}
