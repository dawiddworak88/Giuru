import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";

function SettingsForm(props) {

    const [state, dispatch] = useContext(Context);

    return (
        <section className="section section-small-padding settings">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    {/* Place for form */}
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

SettingsForm.propTypes = {
    title: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired
};

export default SettingsForm;
