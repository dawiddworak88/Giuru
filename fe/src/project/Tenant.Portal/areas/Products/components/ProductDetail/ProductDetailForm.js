import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';

function ProductDetailForm(props) {

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                <Autocomplete
                    id="select-schema"
                    options={...props.schemas}
                    getOptionLabel={(option) => option.name}
                    renderInput={() => <TextField label={props.selectSchemaLabel} variant="outlined" />} />
                <div className="field">
                    <TextField id="sku" sku="sku" label={props.nameLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={errors.sku && dirty.sku && errors.sku} error={dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={errors.name && dirty.name && errors.name} error={dirty.name} />
                </div>
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ProductDetailForm.propTypes = {
    selectLabel: PropTypes.string.isRequired
};

export default ProductDetailForm;