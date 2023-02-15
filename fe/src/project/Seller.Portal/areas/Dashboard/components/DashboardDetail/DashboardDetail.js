import React from "react";
import PropTypes from "prop-types";
import CountrySalesAnalytics from "../../../../../../shared/components/AnalyticsDashboard/CountrySalesAnalytics/CountrySalesAnalytics";
import SalesAnalytics from "../../../../../../shared/components/AnalyticsDashboard/SalesAnalytics/SalesAnalytics";

const DashboardDetail = (props) => {
    return (
        <div className="section dashboard">
            <h1 className="title is-3">{props.title}</h1>
            <div className="mt-4">
                <div className="columns">
                    <div className="column is-half dashboard__container">
                        {props.dailySalesAnalytics &&
                            <SalesAnalytics {...props.dailySalesAnalytics} />
                        }
                    </div>
                    <div className="column is-half dashboard__container">
                        {props.countrySalesAnalytics &&
                            <CountrySalesAnalytics {...props.countrySalesAnalytics} />
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}

DashboardDetail.propTypes = {
    title: PropTypes.string.isRequired,
    dailySalesAnalytics: PropTypes.object.isRequired,
    countrySalesAnalytics: PropTypes.object.isRequired
}

export default DashboardDetail;