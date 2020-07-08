import { Box, Button, TextField, FormControlLabel, Switch } from '@material-ui/core';
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';
import React from 'react';
import { saveData, updateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';
import { useSite } from '../../providers/SiteProvider';

const initialState = {
    Code: '',
    Name: '',
    Address: '',
    Disabled: false
}
const TABLE = 'Sites';

const SiteForm = ({ data, onSuccess }) => {
    const { fetchSites } = useSite();
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);

    React.useEffect(() => {
        if (editMode)
            setFormState({ ...data })
    }, [])

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        if (Number(localStorage.getItem('site')) === formState.Id && formState.Disabled === true) {
            showSnackBar({
                error: true,
                text: 'Vous devez changer le dépôt/magasin actuel pour pouvoir l\'archiver'
            });
            return;
        }

        const _errors = [];
        if (!formState.Code)
            _errors['Code'] = 'Ce champs est obligatoire.'
        if (!formState.Name)
            _errors['Name'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;

        setLoading(true);
        if (editMode) {
            const response = await updateData(TABLE, formState, formState.Id);
            if (response.ok) {
                setFormState({ ...initialState });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur !'
                });
            }
        } else {
            const response = await saveData(TABLE, formState);
            if (response?.Id) {
                setFormState({ ...initialState });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur !'
                });
            }
        }
        fetchSites();
        setLoading(false);
    }

    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier les infos du dépôt/magasin' : 'Ajouter un dépôt/magasin'} Icon={StorefrontOutlinedIcon} />
            <TextField
                name="Code"
                label="Code"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Code || ''}
                helperText={formErrors.Code}
                error={Boolean(formErrors.Code)}
            />
            <TextField
                name="Name"
                label="Nom du dépôt/magasin"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Name || ''}
                helperText={formErrors.Name}
                error={Boolean(formErrors.Name)}
            />
            <TextField
                name="Address"
                label="Adresse"
                variant="outlined"
                size="small"
                fullWidth
                multiline
                margin="normal"
                rows={3}
                onChange={onFieldChange}
                value={formState.Address || ''}
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

export default SiteForm