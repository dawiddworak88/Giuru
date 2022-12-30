import React from "react";
import PropTypes from "prop-types";
import { marked } from "marked";

function ContentDetail(props) {

    return (

        <section className="section">
            <div className="container">
                <div className="content has-text-justified"></div>
                <h1 className="title is-3">{props.title}</h1>
                {props.content &&
                    <div dangerouslySetInnerHTML={{__html: marked.parse(props.content)}}></div>
                }
            </div>
        </section>
    );
}

ContentDetail.propTypes = {
    title: PropTypes.string.isRequired,
    content: PropTypes.string
};

export default ContentDetail;
