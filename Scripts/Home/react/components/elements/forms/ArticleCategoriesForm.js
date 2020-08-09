import { Box, Button, TextField } from '@material-ui/core';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import React from 'react';
import { saveData, partialUpdateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';

const initialState = {
    Name: '',
}
const TABLE = 'Categories';

const ArticleCategoriesForm = ({ data, onSuccess }) => {
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
        const _errors = [];
        if (!formState.Name)
            _errors['Name'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }
    console.log({formState})
    const save = async () => {
        if (!isFormValid()) return;

        setLoading(true);
        if (editMode) {
            const response = await partialUpdateData(TABLE, {
                Id: formState.Id,
                Name: formState.Name,
            }, formState.Id);
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
        setLoading(false);
    }

    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier les infos de la famille' : 'Ajouter une nouvelle famille'} Icon={DescriptionOutlinedIcon} />
            <TextField
                name="Name"
                label="Nom de la famille"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Name || ''}
                helperText={formErrors.Name}
                error={Boolean(formErrors.Name)}
            />

            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default ArticleCategoriesForm