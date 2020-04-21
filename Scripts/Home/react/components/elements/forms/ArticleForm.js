import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { TextField, Button, Box, Avatar } from '@material-ui/core';
import { saveData } from '../../../queries/crudBuilder';
import { v4 as uuidv4 } from 'uuid'
import Loader from '../loaders/Loader';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import SuccessSnackBar from '../snack-bars/SuccessSnackBar';

const border = '1px solid #d8d8d8';

export const useStyles = makeStyles(theme => ({
    item: {
        display: 'flex',
        alignItems: 'center',
        paddingBottom: 16,
        borderBottom: border,
        marginBottom: 16,
        cursor: 'pointer',
        '&:hover .MuiAvatar-root': {
            backgroundColor: '#22496f'
        }
    },
    content: {
        marginLeft: 12,
        maxWidth: 400
    },
    title: {
        fontWeight: 500
    },
    description: {
        marginTop: 6,
        opacity: 0.8
    },
    avatar: {
        height: 50,
        width: 50,
        backgroundColor: '#7290af'
    },
    icon: {
        height: 20,
        width: 20
    }
}));

const initialState = {
    designation: '',
    pvd: '',
    pa: '',
    tva: 20,
    unite: 'U',
    qteStock: 0
}
const TABLE = 'Articles';

const ArticleForm = () => {
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [success, setSuccess] = React.useState(false);
    const classes = useStyles();

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.designation)
            _errors['designation'] = 'Ce champs est obligatoire.'
        // if (!formState.qteStock)
        //     _errors['qteStock'] = 'Ce champs est obligatoire.'
        if (!formState.pvd)
            _errors['pvd'] = 'Ce champs est obligatoire.'
        if (!formState.pa)
            _errors['pa'] = 'Ce champs est obligatoire.'
        if (!formState.tva)
            _errors['tva'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;
        const preparedData = {
            Id: uuidv4(),
            Designation: formState.designation,
            QteStock: formState.qteStock,
            PVD: formState.pvd,
            TVA: formState.tva,
            PA: formState.pa,
            Unite: formState.unite,
            Ref: 'X'
        }

        setLoading(true);
        const response = await saveData(TABLE, preparedData);
        setLoading(false);
        if (response?.Id) {
            setFormState({ ...initialState });
            setSuccess(true);
        }
    }

    return (
        <div>
            <Loader loading={loading} />
            <SuccessSnackBar open={success} onClose={()=>setSuccess(false)}/>
            <div className={classes.item}>
                <Avatar className={classes.avatar}>
                    <AddShoppingCartIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Ajouter un article</div>
                </div>
            </div>
            <TextField
                name="designation"
                label="Désignation"
                variant="outlined"
                size="small"
                fullWidth
                multiline
                margin="normal"
                rows={2}
                onChange={onFieldChange}
                value={formState.designation}
                helperText={formErrors.designation}
                error={Boolean(formErrors.designation)}

            />
            <TextField
                name="qteStock"
                label="Quantité de stock"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.qteStock}
                helperText={formErrors.qteStock}
                error={Boolean(formErrors.qteStock)}

            />
            <TextField
                name="pa"
                label="Prix d'achat"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.pa}
                helperText={formErrors.pa}
                error={Boolean(formErrors.pa)}
            />
            <TextField
                name="pvd"
                label="Prix de vente"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.pvd}
                helperText={formErrors.pvd}
                error={Boolean(formErrors.pvd)}
            />
            <TextField
                name="tva"
                label="T.V.A (%)"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.tva}
                helperText={formErrors.tva}
                error={Boolean(formErrors.tva)}

            />
            <TextField
                name="unite"
                label="Unité"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.unite}
                helperText={formErrors.unite}
                error={Boolean(formErrors.unite)}
            />
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default ArticleForm