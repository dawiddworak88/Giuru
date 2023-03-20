import React from "react";
import PropTypes from "prop-types";
import CountrySalesAnalytics from "../../../../shared/components/AnalyticsDashboard/CountrySalesAnalytics/CountrySalesAnalytics";
import SalesAnalytics from "../../../../shared/components/AnalyticsDashboard/SalesAnalytics/SalesAnalytics";
import ProductsSalesAnalytics from "../../../../shared/components/AnalyticsDashboard/ProductsSalesAnalytics/ProductsSalesAnalytics";
import ClientsSalesAnalytics from "../../../../shared/components/AnalyticsDashboard/ClientSalesAnalytics/ClientsSalesAnalytics";

const DashboardDetail = (props) => {
    return (
        <div className="section dashboard">
            <h1 className="title is-3">{props.title}</h1>
            <div className="mt-4">
                <div className="columns is-multiline">
                    <div className="column is-full dashboard__container">
                        {props.dailySalesAnalytics &&
                            <SalesAnalytics {...props.dailySalesAnalytics} />
                        }
                    </div>
                    <div className="column is-half dashboard__container">
                        {props.productsSalesAnalytics &&
                            <ProductsSalesAnalytics {...props.productsSalesAnalytics} />
                        }
                    </div>
                    <div className="column is-half dashboard__container">
                        {props.clientsSalesAnalytics &&
                            <ClientsSalesAnalytics {...props.clientsSalesAnalytics} />
                        }
                    </div>
                    <div className="column is-full dashboard__container">
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
    countrySalesAnalytics: PropTypes.object.isRequired,
    productsSalesAnalytics: PropTypes.object.isRequired,
    clientsSalesAnalytics: PropTypes.object.isRequired
}

export default DashboardDetail;
