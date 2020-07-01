import React from 'react';
import { Snackbar } from '@material-ui/core';
import MuiAlert from '@material-ui/lab/Alert';

const SuccessSnackBar = ({ error = false, text = "Enregistré avec succès", ...props }) => {
    return (
        <Snackbar anchorOrigin={{ vertical: 'top', horizontal: 'center' }} autoHideDuration={3000}  {...props}>
            <Alert severity={error ? "error" : "success"}>
                <div style={{
                    fontSize: 14
                }}>
                    {text}
                </div>
            </Alert>
        </Snackbar>
    )
}

const Alert = (props) => {
    return <MuiAlert elevation={6} variant="filled" {...props} />
}

export default SuccessSnackBar