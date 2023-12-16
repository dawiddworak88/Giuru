import React, { useState, useEffect } from 'react';
import { TextField, Select, MenuItem, Button, FormControl, InputLabel } from '@mui/material';

const ClientDynamicForm = (props) => {
    const [formData, setFormData] = useState(props.formData ? props.formData : {});

    useEffect(() => {
        const initialFormData = {};
        
        props.dynamicFields.forEach(field => {
            initialFormData[field.id] = field.type === 'select' ? '' : null;
        });

        props.setFormData(initialFormData);
    }, [props.dynamicFields]);

    const handleChange = (name, value) => {
        // setFormData(prev => ({ ...prev, [name]: value }));
        // props.onChange({name: "test", value: "abcd"})
        // props.onChange(prev => ({ ...prev, [name]: value }))
        props.setFormData(prev => ({ ...prev, [name]: value }))
    };

    return (
        props.dynamicFields.map(field => (
            <div key={field.id} className='field'>
                {field.type === 'text' && (
                    <TextField
                        id={field.id}
                        label={field.name}
                        fullWidth
                        variant="standard"
                        value={formData[field.id] || ''}
                        onChange={(e) => handleChange(field.id, e.target.value)}
                    />
                )}
                {field.type === 'number' && (
                    <TextField
                        id={field.id}
                        label={field.name}
                        fullWidth
                        type="number"
                        variant="standard"
                        value={formData[field.id] || ''}
                        onChange={(e) => handleChange(field.id, e.target.value)}
                    />
                )}
                {field.type === 'select' && (
                    <FormControl fullWidth variant="standard">
                        <InputLabel>{field.name}</InputLabel>
                        <Select
                            id={field.id}
                            value={formData[field.id]}
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