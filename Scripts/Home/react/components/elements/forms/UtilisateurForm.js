import { Box, Button, TextField, FormControlLabel, Switch } from '@material-ui/core';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import React from 'react';
import { saveData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';
import { updateUtilisateur, updateUserPassword, setClaim, hasClaim, createUser } from '../../../queries/utilisateurQueries';
import LockIcon from '@material-ui/icons/Lock';
import VpnKeyIcon from '@material-ui/icons/VpnKey';

const initialState = {
    UserName: '',
}
const TABLE = 'ApplicationUsers';

export const CLAIMS = [
    {
        id: 'CanUpdateQteStock',
        displayName: 'Peut changer la quantité de stock'
    },
    {
        id: 'CanViewDashboard',
        displayName: 'Peut consulter le tableau du bord'
    },
    {
        id: 'CanManageClients',
        displayName: 'Peut gérer les clients'
    },
    {
        id: 'CanManageFournisseurs',
        displayName: 'Peut gérer les fournisseurs'
    },
    {
        id: 'CanAddPaiements',
        displayName: 'Peut ajouter les paiements'
    },
    {
        id: 'CanRemovePaiements',
        displayName: 'Peut supprimer les paiements'
    },
    {
        id: 'CanViewPaiements',
        displayName: 'Peut consulter des paiements'
    }
]

const UtilisateurForm = ({ data, onSuccess }) => {
    const isAdmin = data?.Id === '00000000-0000-0000-0000-000000000000';
    const [password, setPassword] = React.useState('');
    const [claims, setClaims] = React.useState([])
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);

    React.useEffect(() => {
        if (editMode) {
            setFormState({ ...data })
        }
        const promises = CLAIMS.map(async (x) => ({
            ...x,
            enabled: editMode ? await hasClaim({
                userId: data.Id,
                claim: x.id
            }) : false
        }))
        Promise.all(promises).then(result => setClaims(result));
    }, [])

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.UserName)
            _errors['UserName'] = 'Ce champs est obligatoire.'

        if (formState.UserName && formState.UserName.length < 3)
            _errors['UserName'] = 'Le nom d\'utilisateur doit comporter au moins 3 caractères';

        if (formState.UserName && /\s/.test(formState.UserName))
            _errors['UserName'] = 'Le nom d\'utilisateur ne doit pas comporter des espaces';

        if (password && password.length < 3)
            _errors['password'] = 'Le mot de passe doit comporter au moins 3 caractères';

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
                if (password)
                    await updatePassword()
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur !'
                });
            }
        } else {
            const response = await createUser({
                username: formState.UserName,
                password
            });
            if (response?.user?.Id) {
                setFormState(_formState => ({
                    ..._formState,
                    Id: response?.user?.Id
                }))
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
        if (password?.length < 3)
            return showSnackBar({
                error: true,
                text: 'Le mot de passe doit comporter au moins 3 caractères!'
            })
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

    const updateClaim = async (claim, enabled) => {
        const response = await (await setClaim({
            userId: formState.Id,
            claim,
            enabled,
        })).json();
        setClaims(_claims => {
            return _claims.map(x => x.id === claim ? { ...x, enabled: response?.userHasClaim } : { ...x });
        })
        showSnackBar()
    }
    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier les infos de l\'utilisateur' : 'Ajouter un nouvel utilisateur'} Icon={DescriptionOutlinedIcon} />
            <TextField
                name="UserName"
                label="Nom de l'utilisateur"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.UserName || ''}
                helperText={formErrors.UserName}
                error={Boolean(formErrors.UserName)}
            />
            <TextField
                name="password"
                label="Mot de passe"
                variant="outlined"
                size="small"
                type="password"
                fullWidth
                margin="normal"
                onChange={({ target: { value } }) => setPassword(value)}
                value={password}
                helperText={formErrors.password}
                error={Boolean(formErrors.password)}
            />
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>

            {!isAdmin && <Box>
                <TitleIcon title="Autorisations" Icon={VpnKeyIcon} />
                {
                    claims.map(x => (
                        <FormControlLabel
                            key={x.id}
                            control={<Switch
                                checked={x.enabled}
                                onChange={(_, checked) => {
                                    updateClaim(x.id, checked)
                                }} />}
                            label={x.displayName}
                        />
                    ))
                }
            </Box>}
        </div>
    )
}

export default UtilisateurForm