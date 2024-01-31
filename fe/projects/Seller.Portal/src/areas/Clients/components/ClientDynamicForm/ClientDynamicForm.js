import React, { useState, useEffect, useMemo } from 'react';
import { 
    TextField, Select, MenuItem, NoSsr, Switch,
    FormControl, InputLabel, FormControlLabel
} from '@mui/material';

const ClientDynamicForm = ({ 
    dynamicFields = [], 
    formData: initialFormData = {}, 
    setFormData
}) => {
    const [formData, setLocalFormData] = useState(initialFormData || {});

    useEffect(() => {
        setFormData(formData);
    }, [formData, setFormData]);

    const handleChange = (name, value) => {
        const updatedFormData = { ...formData, [name]: value };
        setLocalFormData(updatedFormData);
    };

    const initialData = useMemo(() => {
        const initialFormData = {};

        dynamicFields.forEach(field => {
            initialFormData[field.id] = field.value ?? (field.type === 'select' ? '' : null);
        });

        return initialFormData;
    }, [dynamicFields]);

    useEffect(() => {
        setLocalFormData(initialData || {});
    }, [initialData]);

    console.log(dynamicFields)

    return (
        dynamicFields.map(field => (
            <div key={field.id} className='field'>
                {(field.type === 'text' || field.type === 'number') && (
                    <TextField
                        id={field.id}
                        label={field.name}
                        fullWidth
                        type={field.type === 'number' ? 'number' : 'text'}
                        variant="standard"
                        value={formData[field.id] || ''}
                        onChange={(e) => handleChange(field.id, e.target.value)}
                    />
                )}
                {/* {field.type === "boolean" && (
                    <NoSsr>
                        <FormControlLabel
                            control={
                                <Switch
                                    onChange={e => handleChange(field.id, e.target.value)}
                                    checked={formData[field.id]}
                                    id="isPublished"
                                    name="isPublished"
                                    color="secondary" />
                                }
                            label={props.isPublishedLabel} />
                    </NoSsr>
                )} */}
                {field.type === 'select' && (
                    <FormControl fullWidth variant="standard">
                        <InputLabel>{field.name}</InputLabel>
                        <Select
                            id={field.id}
                            value={formData[field.id] || ''}
                            label={field.name}
                            onChange={(e) => handleChange(field.id, e.target.value)}
                        >
                            {field.options.map(option => (
                                <MenuItem key={option.value} value={option.value}>{option.name}</MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                )}
            </div>
        ))
    );
};

export default ClientDynamicForm;