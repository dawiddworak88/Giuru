import React, { useContext } from "react"
import { toast } from "react-toastify";
import { Context } from "../../../../../../shared/stores/Store";
import PropTypes from "prop-types";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { 
    TextField, Button, InputLabel, CircularProgress 
} from "@mui/material";

const ClientManagerForm = (props) => {
    const [state, dispatch] = useContext(Context);

    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form">
                       
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ClientManagerForm.propTypes = {
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
}

export default ClientManagerForm;