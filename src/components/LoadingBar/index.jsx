import React from 'react';
import CircularProgress from '@mui/material/CircularProgress';

const LoadingBar = ({ marginTop }) => (
  <div style={{
    display: 'flex',
    justifyContent: 'center',
    marginTop,
  }}
  >
    <CircularProgress variant="indeterminate" disableShrink />
  </div>
);

export default LoadingBar;
