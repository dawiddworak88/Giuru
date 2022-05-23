import React, { useContext, useState, Fragment } from "react";
import { Context } from "../../../../../../shared/stores/Store";
import { Button, InputAdornment, TextField } from "@mui/material";

const SettingsForm = (props) => {
    const [state, dispatch] = useContext(Context);

    return (
        <div className="settings-form pl-5">
            <h1 className="subtitle is-size-3 mb-6">Ustawienia konta</h1>
            <div className="form-group">
                <div className="settings-form-section">
                    <h2 className="settings-form-section__title">Dane konta</h2>
                </div>
                <TextField 
                    id="client"
                    label="Nazwa"
                    value="sebastian"
                    variant="standard"
                    fullWidth={true}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">
                                <Button>ZMIEŃ</Button>
                            </InputAdornment>
                        )
                    }}
                />
            </div>

            <div className="form-group">
                <div className="settings-form-section">
                    <h2 className="settings-form-section__title">Twój identyfikator API</h2>
                    <p className="settings-form-section__description">Dzięki temu identyfikatorowi możesz generować kod dostępu do naszego api.</p>
                </div>
                <TextField 
                    id="client"
                    label="Nazwa"
                    value="sebastian"
                    variant="standard"
                    fullWidth={true}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">
                                <Button>ZMIEŃ</Button>
                            </InputAdornment>
                        )
                    }}
                />
            </div>
        </div>
    )
}

export default SettingsForm;