import React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';

const AlertDialog = ({
  open,
  title,
  content,
  onOk,
  onCancel,
  onClose,
  agreeLabel,
  cancelLabel,
}) => {
  /**
   * Cancel action
   */
  const cancel = () => {
    if (onCancel) {
      onCancel();
      onClose();
    }
  };

  /**
   * Agree action
   */
  const agree = () => {
    if (onOk) {
      onOk();
      onClose();
    }
  };

  /**
   * Close the dialog
   */
  const close = () => {
    if (onClose) {
      onClose();
    }
  };

  const splitContent = content.split('\n').map((item, idx) => {
    if (idx === 0) {
      return (
        <span key={idx}>
          {item}
          {content.split('\n').length > 1 ? <br /> : null}
        </span>
      );
    }
    return (
      <span key={idx}>
        {item.charAt(0).toUpperCase() + item.slice(1)}
        <br />
      </span>
    );
  });

  return (
    <Dialog
      open={open}
      onClose={close}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">{title}</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          {splitContent}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        {onCancel ? <Button onClick={cancel}>{cancelLabel}</Button> : null}
        {onOk ? (
          <Button onClick={agree} autoFocus>
            {agreeLabel}
          </Button>
        ) : null}
      </DialogActions>
    </Dialog>
  );
};

export default AlertDialog;
