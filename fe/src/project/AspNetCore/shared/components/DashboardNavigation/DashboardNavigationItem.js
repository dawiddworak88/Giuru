import React from "react";
import * as Icon from "@mui/icons-material";

const DashboardNavigationItem = (props) => {

    const NavigationIcon = Icon[props.icon]

    return (
        <a href={props.url} className="dashboard-navigation-item">
            <span className="dashboard-navigation-item__icon"><NavigationIcon /></span>
            <span className="dashboard-navigation-item__title">{props.title}</span>
        </a>
    )
}

export default DashboardNavigationItem;