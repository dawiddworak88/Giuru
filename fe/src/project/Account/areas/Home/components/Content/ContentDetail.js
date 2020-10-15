import React from "react";
import PropTypes from "prop-types";
import parse from 'html-react-parser';

function ContentDetail(props) {

    return (

        <section className="content-detail section">
            <h1 className="title is-3">{props.title}</h1>
            <div>
                {parse(props.content)}
            </div>
        </section>
    );
}

ContentDetail.propTypes = {
    title: PropTypes.string.isRequired,
    content: PropTypes.string.isRequired
};

export default ContentDetail;
