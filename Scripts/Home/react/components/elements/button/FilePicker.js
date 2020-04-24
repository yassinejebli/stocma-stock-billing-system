import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import AddAPhotoIcon from '@material-ui/icons/AddAPhoto';
export const useStyles = makeStyles(theme => ({
    input: {
        display: 'none'
    },
    wrapper: {
        backgroundColor: theme.palette.primary.main,
        padding: '16px 0',
        display: 'flex',
        justifyContent: 'center',
        width: '100%',
        cursor: 'pointer'
    },
    icon: {
        color: '#FFF',
        width: 26,
        height: 26
    }
}));

const FilePicker = (props) => {
    const classes = useStyles();
    return <>
        <input
            accept="image/*"
            className={classes.input}
            id="contained-button-file"
            multiple
            type="file"
            onClick={event=> { 
                event.target.value = null
           }}
            {...props}
        />
        <label htmlFor="contained-button-file">
            <div className={classes.wrapper}>
                <AddAPhotoIcon className={classes.icon} />
            </div>
        </label>
    </>
}


export default FilePicker;