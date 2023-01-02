import React from "react";
import DashboardNavigationItem from "./DashboardNavigationItem";

const DashboardNavigation = (props) => {
    return (
        props.navigationItems && props.navigationItems.length > 0 &&
            <div className="dashboard-navigation">
                {props.navigationItems.map((item, index) => {
                    return (
                        <DashboardNavigationItem url={item.url} title={item.title} icon={item.icon} key={index} />
                    )
                })}
            </div>
    )
}

export default DashboardNavigation;