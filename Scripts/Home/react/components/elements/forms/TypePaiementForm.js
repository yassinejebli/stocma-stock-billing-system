import { Box, Button, TextField } from '@material-ui/core';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import React from 'react';
import { saveData, partialUpdateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';

const initialState = {
    Name: '',
    IsDebit: false,
}
const TABLE = 'TypePaiements';

const TypePaiementForm = ({ data, onSuccess }) => {
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
                IsDebit: formState.IsDebit,
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
            <TitleIcon title={editMode ? 'Modifier les infos de la méthode de paiement' : 'Ajouter une méthode de paiement'} Icon={DescriptionOutlinedIcon} />
            <TextField
                name="Name"
                label="Nom du méthode de paiement"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Name || ''}
                helperText={formErrors.Name}
                error={Boolean(formErrors.Name)}
            />

            <RadioGroup value={formState.IsDebit.toString()} onChange={({ target: { value } }) => {
                console.log({ value })
                setFormState(_formState => ({
                    ..._formState,
                    IsDebit: value === 'true'
                }))
            }}>
                <Box display="flex">
                    <FormControlLabel value="false" control={<Radio />} label="Credit" />
                    <FormControlLabel value="true" control={<Radio />} label="Debit" />
                </Box>
            </RadioGroup>
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default TypePaiementForm