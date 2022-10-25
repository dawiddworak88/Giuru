import React from "react";

export default class ToastHelper {

    static withLink(message, url, urlLabel) {
        return (
            <div>
                {message} <a href={`${url}`}>{urlLabel}</a>
            </div>
        )
    }
}
