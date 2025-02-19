import React from "react"
import BlockRendererClient from "../BlockRendererClient/BlockRendererClient"
import CarouselGrid from "../CarouselGrid/CarouselGrid";
import { Video } from "../Video/Video";

/** Component to transform data from CMS to UI component */
export const StrapiContent = (props) => {

    const generateWidget = (widget) => {
        switch (widget.typename) {
            case "ComponentSharedContent": 
                return <BlockRendererClient content={JSON.parse(widget.content)} />;
            case "ComponentSharedSlider":
                return <CarouselGrid {...widget.slider} />;
            case "ComponentBlocksVideo":
                return <Video {...widget} />
            default:
                return null
        }
    }

    return (
        <div className="section">
            <div className="container">
                <div className="columns is-centered">
                    <div className="column is-8">
                        {props.widgets && props.widgets.length > 0 && props.widgets.map((widget) => (
                            generateWidget(widget)
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}