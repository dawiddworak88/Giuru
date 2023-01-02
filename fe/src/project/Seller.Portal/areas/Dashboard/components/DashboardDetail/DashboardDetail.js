import React from "react";
import PropTypes from "prop-types";
import CountrySalesAnalytics from "../../../../../../shared/components/AnalyticsDashboard/CountrySalesAnalytics/CountrySalesAnalytics";
import SalesAnalytics from "../../../../../../shared/components/AnalyticsDashboard/SalesAnalytics/SalesAnalytics";

const DashboardDetail = (props) => {
    return (
        <div className="section dashboard">
            <h1 className="title is-3">{props.title}</h1>
            <div className="mt-6">
                <div className="columns">
                    <div className="column is-two-thirds">
                        {props.salesAnalytics &&
                            <SalesAnalytics {...props.salesAnalytics} />
                        }
                    </div>
                    <div className="column is-one-third">
                        {props.countrySales &&
                            <CountrySalesAnalytics {...props.countrySales} />
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}

DashboardDetail.propTypes = {
    title: PropTypes.string.isRequired,
    salesAnalytics: PropTypes.object.isRequired,
    countrySales: PropTypes.object.isRequired
}

export default DashboardDetail;