import React from "react";
import * as Icon from "@mui/icons-material";

const DashboardNavigationItem = (props) => {

    const NavigationIcon = Icon[props.icon]

    return (
        <a href={props.url} className="dashboard-navigation__item">
            <span><NavigationIcon /></span>
            <span>{props.title}</span>
        </a>
    )
}

export default DashboardNavigationItem;