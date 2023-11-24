import React from "react";
import PropTypes from "prop-types";
import SalesAnalytics from "../../../shared/components/AnalyticsDashboard/SalesAnalytics/SalesAnalytics";

const OrdersAnalyticsDetail = (props) => {
    return (
        <div className="section orders-analytics pt-6">
            <div className="orders-analytics__content">
                <h1 className="title is-4">{props.title}</h1>
                <div className="orders-analytics__container mt-4">
                    <SalesAnalytics {...props.salesAnalytics} />
                </div>
            </div>
        </div>
    )
}

OrdersAnalyticsDetail.propTypes = {
    title: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired
}

export default OrdersAnalyticsDetail;