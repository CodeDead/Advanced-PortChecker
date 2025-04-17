import React from 'react';
import TextField from '@mui/material/TextField';

const PortInput = ({ label, port, disabled, onKeyDown, onChange }) => (
  <TextField
    id="start-port-basic"
    label={label}
    variant="outlined"
    value={port}
    disabled={disabled}
    type="number"
    min={0}
    max={65535}
    pattern="[0-9]"
    fullWidth
    onChange={onChange}
    onKeyDown={onKeyDown}
  />
);

export default PortInput;
