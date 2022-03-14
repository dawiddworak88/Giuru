import React, {useState} from "react";
import { CalendarToday } from "@material-ui/icons";
import PropTypes from "prop-types";
import { Hash } from "react-feather"
import moment from "moment";

const NewsCatalog = (props) => {
    const [items] = useState(props.pagedResults.data ? props.pagedResults.data : null);

    const handleNewsCollapse = (e) => {
        const collapseTarget = e.target.dataset.collapseTarget;

        const target = document.querySelector(collapseTarget);
        target.classList.toggle("active");
    }

    return (
        items.length > 0 && items &&
            <section className="section news-catalog">
                <div className="news-catalog__container">
                    <p className="title is-4">{props.title}</p>
                    <div className="columns is-tablet is-multiline">
                        {items.map((item, index) => {
                            return (
                                <div className="column is-half news-catalog__item">
                                    <div className="card">
                                        <div className="media-content">
                                            <h2 className="title is-5">{item.title}</h2>
                                            <div className="media-data">
                                                <div className="data">
                                                    <Hash />
                                                    <span className="data-text">{item.categoryName}</span>
                                                </div>
                                                <div className="data">
                                                    <CalendarToday /> 
                                                    <span className="data-text">{moment.utc(item.createdDate).local().format("L")}</span>
                                                </div>
                                            </div>
                                            <p className="is-6 media-description">{item.description}</p>
                                            {item.content &&
                                                <span data-collapse-target={`#content-${index+1}`} onClick={(e) => handleNewsCollapse(e)} className="media-read">{props.readMoreLabel}</span>
                                            }
                                        </div>
                                        {item.content &&
                                            <div id={`content-${index+1}`} class="media-collapse">{item.content}</div>
                                        }
                                    </div>
                                </div>
                            )
                        })}
                    </div>
                </div>
            </section>
    )
}

NewsCatalog.propTypes = {
    title: PropTypes.string.isRequired,
    readMoreLabel: PropTypes.string.isRequired
}

export default NewsCatalog;