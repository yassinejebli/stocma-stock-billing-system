import { Box, Button, TextField } from '@material-ui/core';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import React from 'react';
import { saveData, updateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import Loader from '../loaders/Loader';
import TitleIcon from '../misc/TitleIcon';

const initialState = {
    Name: '',
}
const TABLE = 'TypeDepenses';

const TypeDepenseForm = ({data, onSuccess}) => {
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
        const preparedData = formState;

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
            <TitleIcon title={editMode ? 'Modifier les infos du type de dépense' : 'Ajouter un type de dépense'} Icon={DescriptionOutlinedIcon} />
            <TextField
                name="Name"
                label="Nom du type de dépense"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Name||''}
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

export default TypeDepenseForm