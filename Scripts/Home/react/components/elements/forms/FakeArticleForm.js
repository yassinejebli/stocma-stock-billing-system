import React from 'react';
import { TextField, Button, Box, Switch, FormControlLabel } from '@material-ui/core';
import { v4 as uuidv4 } from 'uuid'
import Loader from '../loaders/Loader';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import { useSnackBar } from '../../providers/SnackBarProvider';
import TitleIcon from '../misc/TitleIcon';
import { saveData, updateData } from '../../../queries/crudBuilder';

const initialState = {
    Ref: '',
    Id: uuidv4(),
    Designation: '',
    PVD: '',
    PA: '',
    TVA: 20,
    Unite: 'U',
    QteStock: 0,
}

const FakeArticleForm = ({ data, onSuccess }) => {
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);

    React.useEffect(() => {
        if (editMode) {
            setFormState({
                ...data
            });
        }
    }, [])

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.Designation)
            _errors['Designation'] = 'Ce champs est obligatoire.'
        if (!formState.Ref)
            _errors['Ref'] = 'Ce champs est obligatoire.'
        if (!formState.PVD)
            _errors['PVD'] = 'Ce champs est obligatoire.'
        if (!formState.PA)
            _errors['PA'] = 'Ce champs est obligatoire.'
        if (!formState.TVA)
            _errors['TVA'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;

        setLoading(true);
        if (editMode) {
            const response = await updateData('ArticleFactures', formState, formState.Id);
            if (response?.ok) {
                setFormState({ ...initialState });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur!'
                });
            }
        } else {
            const response = await saveData('ArticleFactures', {...formState, Id: uuidv4()});
            if (response?.Id) {
                setFormState({ ...initialState, Id: uuidv4() });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur!'
                });
            }
        }
        setLoading(false);
    }

    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier l\'article' : 'Ajouter un article'} Icon={AddShoppingCartIcon} />
            <TextField
                name="Ref"
                label="Référence/Code"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Ref}
                helperText={formErrors.Ref}
                error={Boolean(formErrors.Ref)}

            />
            <TextField
                name="Designation"
                label="Désignation"
                variant="outlined"
                size="small"
                fullWidth
                multiline
                margin="normal"
                rows={2}
                onChange={onFieldChange}
                value={formState.Designation}
                helperText={formErrors.Designation}
                error={Boolean(formErrors.Designation)}

            />
            <TextField
                name="QteStock"
                label="Quantité en stock"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.QteStock}
                helperText={formErrors.QteStock}
                error={Boolean(formErrors.QteStock)}

            />
            <TextField
                name="PA"
                label="Prix d'achat"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.PA}
                helperText={formErrors.PA}
                error={Boolean(formErrors.PA)}
            />
            <TextField
                name="PVD"
                label="Prix de vente"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.PVD}
                helperText={formErrors.PVD}
                error={Boolean(formErrors.PVD)}
            />
            <TextField
                name="TVA"
                label="T.V.A applicable (%)"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.TVA}
                helperText={formErrors.TVA}
                error={Boolean(formErrors.TVA)}

            />
            <TextField
                name="Unite"
                label="Unité"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Unite}
                helperText={formErrors.Unite}
                error={Boolean(formErrors.Unite)}
            />
            <FormControlLabel
                control={<Switch
                    checked={!formState.Disabled}
                    onChange={(_, checked) => setFormState(_formState => ({
                        ...formState,
                        Disabled: !checked
                    })
                    )} />}
                label="Actif"
            />
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default FakeArticleForm