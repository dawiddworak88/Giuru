import React from "react";
import PropTypes from "prop-types";
import * as Icon from "@mui/icons-material";
import { Splide, SplideSlide } from "@splidejs/react-splide";

const NotificationBar = (props) => {

    const getIconTag = (iconName) => {
        let IconTag = Icon[iconName];
        const defaultIconName = "PushPin"; 

        if (IconTag === undefined || IconTag === null) {
            IconTag = Icon[defaultIconName];
        }

        return (
            <IconTag />
        )
    };

    return (
        <div className="notification-bar">
            <div className="notification-bar_desktop">
                <div className="notification-bar_list is-flex is-justify-content-center">
                    {props.items.map((item) => (
                        <div className="notification-bar_item is-flex">
                            {getIconTag(item.icon)}
                            <a href={item.link.url} target={item.link.target} className="notification-bar_link">
                                {item.link.text}
                            </a>
                        </div>
                    ))}
                </div>
            </div>
            <div className="notification-bar_mobile">
                <Splide
                    options={{
                        perPage: 1,
                        autoplay: true,
                        pagination: false,
                        arrows: false,
                        type: "loop",
                        speed: 2000,
                        interval: 7000
                    }}
                >
                    {props.items.map((item) => (
                        <SplideSlide>
                            <div className="notification-bar_item is-flex is-justify-content-center">
                                {getIconTag(item.icon)}
                                <a href={item.link.url} target={item.link.target} className="notification-bar_link">
                                    {item.link.text}
                                </a>
                            </div>
                        </SplideSlide>
                    ))}
                </Splide>
            </div>
        </div>
    );
};

NotificationBar.propTypes = {
    items: PropTypes.array.isRequired
}

export default NotificationBar;