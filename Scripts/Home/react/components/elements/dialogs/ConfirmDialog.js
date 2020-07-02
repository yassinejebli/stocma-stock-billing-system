import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import Button from "@material-ui/core/Button";
import Box from "@material-ui/core/Box";
import CancelIcon from '@material-ui/icons/Cancel';
import Typography from "@material-ui/core/Typography";
import grey from "@material-ui/core/colors/grey";
import { confirmable, createConfirmation } from 'react-confirm';
const useStyles = makeStyles({
    closeIcon: {
        width: 32,
        height: 32,
        cursor: 'pointer'
    },
    button: {
        minWidth: 90,
    },
    smallText: {
        fontSize: 10,
        color: grey[700]
    },
    text: {
        fontSize: 15
    }
});

function ConfirmModal({ show, proceed, text, smallText }) {
    const classes = useStyles();

    return (
        <Dialog open={show}>
            <Box pb={2}>
                <Box mt={1} mr={2} display="flex" justifyContent="flex-end">
                    <CancelIcon className={classes.closeIcon} color="primary" onClick={() => proceed(false)} />
                </Box>
                <Box px={2}>
                    <DialogContent>
                        <Box>
                            <div className={classes.text}>
                                {text}
                            </div>
                            {smallText && <div className={classes.smallText}
                                dangerouslySetInnerHTML={{ __html: smallText }} />}
                        </Box>
                        <Box mt={4} mb={1} display="flex" justifyContent="center">
                            <Button className={classes.button} variant="contained"
                                onClick={() => proceed(true)} color="primary">
                                Ok
                            </Button>
                            <Button  style={{ marginLeft: 20 }} className={classes.button} variant="contained" onClick={() => proceed(false)} color="primary">
                                Annuler
                            </Button>

                        </Box>
                    </DialogContent>
                </Box>
            </Box>
        </Dialog>
    );
}

//export default confirmable(ConfirmModal);
export const confirmDialog = createConfirmation(confirmable(ConfirmModal));