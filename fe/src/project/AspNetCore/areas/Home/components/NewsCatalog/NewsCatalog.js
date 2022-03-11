import React from "react";
import PropTypes from "prop-types";

const NewsCatalog = (props) => {
    return (
        <section className="section news-catalog">
            <div className="news-catalog__container">
                <div className="columns is-tablet is-multiline">
                    <div className="column is-half">
                        test_1
                    </div>
                    <div className="column is-half">
                        test_2
                    </div>
                    <div className="column is-half">
                        test_3
                    </div>
                    <div className="column is-half">
                        test_4
                    </div>
                </div>
            </div>
        </section>
    )
}

NewsCatalog.propTypes = {

}

export default NewsCatalog;