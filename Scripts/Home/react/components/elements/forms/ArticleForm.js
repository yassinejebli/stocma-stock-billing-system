import React from 'react';
import { TextField, Button, Box, Avatar } from '@material-ui/core';
import { v4 as uuidv4 } from 'uuid'
import Loader from '../loaders/Loader';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import { saveArticle } from '../../../queries/articleQueries';
import {useSite} from '../../providers/SiteProvider';
import { useSnackBar } from '../../providers/SnackBarProvider';
import TitleIcon from '../misc/TitleIcon';

const initialState = {
    designation: '',
    pvd: '',
    pa: '',
    tva: 20,
    unite: 'U',
    qteStock: 0
}

const ArticleForm = ({data}) => {
    const {siteId} = useSite();
    const {showSnackBar} = useSnackBar();
    const editMode = Boolean(data);
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);

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
            QteStock: 0,
            PVD: formState.pvd,
            TVA: formState.tva,
            PA: formState.pa,
            Unite: formState.unite,
            Ref: 'X'
        }

        setLoading(true);
        const response = await saveArticle(preparedData, formState.qteStock, siteId);
        setLoading(false);
        if (response?.Id) {
            setFormState({ ...initialState });
            showSnackBar();
        }else{
            showSnackBar({
                error: true
            });
        }
    }

    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier l\'article': 'Ajouter un article'} Icon={AddShoppingCartIcon} />
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