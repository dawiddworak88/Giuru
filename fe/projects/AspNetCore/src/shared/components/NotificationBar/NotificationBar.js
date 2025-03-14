import React from "react";
import PropTypes from "prop-types";
import * as Icon from "@mui/icons-material";
import { Splide, SplideSlide } from "@splidejs/react-splide";

const NotificationBar = (props) => {

    const getIconTag = (iconName) => {

        if (iconName && Icon[iconName]) {

            const IconTag = Icon[iconName];

            if (IconTag) {
                return <IconTag />;
            }
        }
        
        return;
    };

    return (
        <div className="notification-bar">
            <div className="notification-bar_desktop">
                <div className="notification-bar_list is-flex is-justify-content-center">
                    {props.items.map((item, index) => (
                        <div className="notification-bar_item is-flex" key={index}>
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
                        type: "loop"
                    }}
                >
                    {props.items.map((item, index) => (
                        <SplideSlide key={index}>
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