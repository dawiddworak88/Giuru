import React from "react";

const DownloadForm = (props) => {
    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>

                    </form>
                </div>
            </div>
        </section>
    )
}

export default DownloadForm;