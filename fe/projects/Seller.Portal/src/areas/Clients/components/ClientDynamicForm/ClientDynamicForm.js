import React, { useState, useEffect } from 'react';
import { TextField, Select, MenuItem, Button, FormControl, InputLabel } from '@mui/material';

const ClientDynamicForm = (props) => {
    const [formData, setFormData] = useState({});

    useEffect(() => {
        const initialFormData = {};
        
        props.dynamicFields.forEach(field => {
            initialFormData[field.name] = field.type === 'select' ? '' : null;
        });

        setFormData(initialFormData);
    }, [props.dynamicFields]);

    const handleChange = (name, value) => {
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    return (
        props.dynamicFields.map(field => (
            <div key={field.name} className='field'>
                {field.type === 'text' && (
                    <TextField
                        label={field.label}
                        value={formData[field.name] || ''}
                        onChange={(e) => handleChange(field.name, e.target.value)}
                    />
                )}
                {field.type === 'number' && (
                    <TextField
                        label={field.label}
                        type="number"
                        value={formData[field.name] || ''}
                        onChange={(e) => handleChange(field.name, e.target.value)}
                    />
                )}
                {field.type === 'select' && (
                    <FormControl fullWidth>
                    <InputLabel>{field.label}</InputLabel>
                        <Select
                            value={formData[field.name]}
                            label={field.label}
                            onChange={(e) => handleChange(field.name, e.target.value)}
                        >
                            {field.options.map(option => (
                            <MenuItem key={option.value} value={option.value}>
                                {option.label}
                            </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                )}
            </div>
        ))
    );
};

export default ClientDynamicForm;