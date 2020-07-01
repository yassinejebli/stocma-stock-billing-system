import { Box, Button, TextField } from '@material-ui/core';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import React from 'react';
import { saveData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';
import { updateUtilisateur, updateUserPassword } from '../../../queries/utilisateurQueries';
import LockIcon from '@material-ui/icons/Lock';

const initialState = {
    UserName: '',
}
const TABLE = 'ApplicationUsers';

const UtilisateurForm = ({ data, onSuccess }) => {
    const [password, setPassword] = React.useState('');
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
        if (!formState.UserName)
            _errors['UserName'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;
        const preparedData = formState;

        setLoading(true);
        if (editMode) {
            const response = await updateUtilisateur(preparedData);
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
            const response = await saveData(TABLE, preparedData);
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


    const updatePassword = async () => {
        // if (!isFormValid()) return;

        setLoading(true);
        const response = await updateUserPassword({
            userId: formState.Id,
            newPassword: password
        });
        if (response.ok) {
            showSnackBar();
            if (onSuccess) onSuccess();
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur !'
            });
        }
        setLoading(false);
    }
    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier les infos de l\'utilisateur' : 'Ajouter un nouvel utilisateur'} Icon={DescriptionOutlinedIcon} />
            <TextField
                name="UserName"
                label="Nom/Email de l'utilisateur"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.UserName || ''}
                helperText={formErrors.UserName}
                error={Boolean(formErrors.UserName)}
            />
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>

            <TitleIcon title="Change le mot de passe" Icon={LockIcon} />
            <Box>
                <TextField
                    name="password"
                    label="Mot de passe"
                    variant="outlined"
                    size="small"
                    type="password"
                    fullWidth
                    margin="normal"
                    onChange={({target: {value}})=> setPassword(value)}
                    value={password}
                    helperText={formErrors.password}
                    error={Boolean(formErrors.password)}
                />
            </Box>
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={updatePassword}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default UtilisateurForm