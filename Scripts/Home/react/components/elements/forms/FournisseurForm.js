import { Box, Button, FormControlLabel, Switch, TextField } from '@material-ui/core';
import GroupAddOutlinedIcon from '@material-ui/icons/GroupAddOutlined';
import React from 'react';
import { saveData, updateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';

const initialState = {
    Name: '',
    Tel: '',
    Fax: '',
    Email: '',
    Adresse: '',
    ICE: '',
    Disabled: false
}
const TABLE = 'Fournisseurs';

const FournisseurForm = ({data, onSuccess}) => {
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    
    React.useEffect(()=>{
        if(editMode)
            setFormState({...data})
    }, [])

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.Name)
            _errors['Name'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;
        const {Solde, SoldeFacture, ...preparedData} = formState;
        setLoading(true);
        if(editMode){
            const response = await updateData(TABLE, preparedData, preparedData.Id);
            if (response.ok) {
                setFormState({ ...initialState });
                showSnackBar();
                if(onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur !'
                });
            }
        }else{
            const response = await saveData(TABLE, preparedData);
            if (response?.Id) {
                setFormState({ ...initialState });
                showSnackBar();
                if(onSuccess) onSuccess();
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
            <TitleIcon title={editMode ? 'Modifier les infos du fournisseur' : 'Ajouter un fournisseur'} Icon={GroupAddOutlinedIcon} />
            <TextField
                name="Name"
                label="Nom du fournisseur"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Name||''}
                helperText={formErrors.Name}
                error={Boolean(formErrors.Name)}
            />
            <TextField
                name="ICE"
                label="I.C.E"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.ICE||''}
            />
            <TextField
                name="Tel"
                label="Tel"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Tel||''}
            />
            <TextField
                name="Fax"
                label="Fax"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Fax||''}
            />
            <TextField
                name="Adresse"
                label="Adresse"
                variant="outlined"
                size="small"
                fullWidth
                multiline
                margin="normal"
                rows={3}
                onChange={onFieldChange}
                value={formState.Adresse||''}
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

export default FournisseurForm