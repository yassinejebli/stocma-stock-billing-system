import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import TextField from '@material-ui/core/TextField';

export const useInputStyles = makeStyles(theme => ({
    input: {
        backgroundColor: 'transparent',
        border: 'none',
        width: '100%',
        fontWeight: 500,
        paddingTop: 6,
        paddingBottom: 6,
        '&:focus': {
            outline: 'none',
            // backgroundColor:'gray'
        },
        '&& .MuiInput-root:hover::before': {
            display: 'none'
        },
        '&& .MuiInput-underline:before': {
            display: 'none'
        },
        '&& .MuiInput-underline:after': {
            display: 'none'
        },
        '&& textarea, input': {
            textAlign: props => props.align
        }
    }
}));

const Input = ({ inTable,
    tabIndex,
    readOnly,
    type = 'text',
    align,
    style,
    ...props }) => {
    const classes = useInputStyles({ align });

    return (
        <TextField
            {...(inTable && { className: classes.input })}
            {...props}
            variant={inTable ? 'standard' : 'outlined'}
            multiline={type !== 'number'}
            inputProps={{
                ...props.inputProps,
                autoComplete: 'new-password',
                type,
                margin: 'normal',
                readOnly,
                tabIndex,
                style
            }}

        />
    )
}

export default Input;