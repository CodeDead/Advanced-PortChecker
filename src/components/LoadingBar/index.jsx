import React from 'react';
import LinearProgress from '@mui/material/LinearProgress';

const LoadingBar = ({ marginTop }) => (
  <div
    style={{
      display: 'flex',
      justifyContent: 'center',
      marginTop,
    }}
  >
    <LinearProgress variant="indeterminate" />
  </div>
);

export default LoadingBar;
