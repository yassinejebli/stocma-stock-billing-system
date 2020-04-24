import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import PhotoCameraOutlinedIcon from '@material-ui/icons/PhotoCameraOutlined';
import { Avatar, Box } from '@material-ui/core';
import { toBase64 } from '../../../utils/imageUtils';

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
        width: 32,
        height: 32
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
            {...props}
        />
        <label htmlFor="contained-button-file">
            <div className={classes.wrapper}>
                <PhotoCameraOutlinedIcon className={classes.icon} />
            </div>
        </label>
    </>
}


export default FilePicker;