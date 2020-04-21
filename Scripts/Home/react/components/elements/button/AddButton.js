import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import Button from '@material-ui/core/Button';
import AddIcon from '@material-ui/icons/Add';


export const useStyles = makeStyles(theme => ({

}));

const AddButton = ({ children, ...props }) => {
    return <Button
        {...props}
        size="small"
        color="primary" startIcon={<AddIcon />}
    >
        {children}
    </Button>
}


export default AddButton;