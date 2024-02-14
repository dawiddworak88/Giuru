import React from "react";
import { Accordion, AccordionDetails, AccordionSummary, Typography } from "@mui/material";
import PropTypes from "prop-types";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { marked } from "marked";

const ContentDetail = (props) => {

    return (
        <section className="section">
            <div className="container">
                <h2 className="title is-3">{props.title}</h2>
                {props.description &&
                    <div dangerouslySetInnerHTML={{__html: marked.parse(props.description)}}></div>
                }
                <div>
                    {props.accordionItems && props.accordionItems.length > 0 &&
                        props.accordionItems.map((accordion, index) => {
                            return (
                                <div key={index} className="mt-5">
                                    <Accordion className="accordion">
                                        <AccordionSummary
                                            expandIcon={<ExpandMoreIcon />}
                                            aria-controls="panel-content"
                                            id={index}
                                        >
                                            <Typography fontWeight={"bold"}>{accordion.title}</Typography>
                                        </AccordionSummary>
                                        <AccordionDetails>
                                            <Typography>{accordion.description}</Typography>
                                        </AccordionDetails>
                                    </Accordion>
                                </div>
                            );
                        })
                    }
                </div>
            </div>
        </section>
    );
};

ContentDetail.propTypes = {
    title: PropTypes.string.isRequired,
    description: PropTypes.string,
    accordionItems: PropTypes.array
};

export default ContentDetail;