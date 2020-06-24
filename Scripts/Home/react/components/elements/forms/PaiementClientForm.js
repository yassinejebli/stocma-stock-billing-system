import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Box, Button, TextField, FormControlLabel, Switch } from '@material-ui/core';
import DatePicker from '../date-picker/DatePicker';
import TitleIcon from '../misc/TitleIcon';
import AccountBalanceWalletOutlinedIcon from '@material-ui/icons/AccountBalanceWalletOutlined';
import Autocomplete from '@material-ui/lab/Autocomplete';
import ClientAutocomplete from '../client-autocomplete/ClientAutocomplete';
import { saveData, updateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';

export const useStyles = makeStyles(theme => ({
    root: {

    }
}));

const TABLE = 'Paiements'

//TODO: should get these data from backend
export const paiementMethods = [
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb2',
        name: 'Espéce',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb3',
        name: 'Chéque',
        isBankRelatedItem: true,
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb4',
        name: 'Effet',
        isBankRelatedItem: true,
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeece1',
        name: 'Impayé',
        isBankRelatedItem: true,
        isDebit: true
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecc1',
        name: 'Versement',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb5',
        name: 'Remise',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb8',
        name: 'Avoir',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb7',
        name: 'Vente',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeeca4',
        name: 'Remboursement',
        isDebit: true
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecc9',
        name: 'Ancien solde',
        isDebit: true
    }
]

const initialState = {
    type: null,
    amount: '',
    date: new Date(),
    dueDate: null,
    comment: '',
    client: null,
    isCashed: false
}

const PaiementClientForm = ({ document, amount, paiement, onSuccess, isAvoir }) => {
    const { showSnackBar } = useSnackBar();
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const isFromDocument = Boolean(document);
    const isEditMode = Boolean(paiement);

    React.useEffect(() => {
        if (isFromDocument) {
            setFormState(_formState => ({
                ..._formState,
                amount,
                client: document.Client,
                comment: (isAvoir ? 'Avoir ' : 'BL ') + document.NumBon,
                IdBonLivraison: document.Id
            }));
        }
        if (isEditMode) {
            setFormState(_formState => ({
                ..._formState,
                amount: paiement.Credit || paiement.Debit, //TODO: change this
                client: paiement.Client,
                type: paiementMethods.find(x => x.id === paiement.IdTypePaiement),
                comment: paiement.Comment,
                date: paiement.Date,
                dueDate: paiement.DateEcheance,
                isCashed: paiement.EnCaisse
            }));
        }
    }, []);

    const save = async () => {
        if (!isFormValid()) return;

        const preparedData = {
            IdTypePaiement: formState.type.id,
            IdClient: formState.client.Id,
            Credit: formState.type.isDebit ? 0 : formState.amount,
            Debit: formState.type.isDebit ? formState.amount : 0,
            Date: formState.date,
            DateEcheance: formState.dueDate,
            Comment: formState.comment,
            EnCaisse: formState.isCashed
        }

        if (isEditMode) {
            const response = await updateData(TABLE, { ...preparedData, Id: paiement.Id }, paiement.Id);
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
                showSnackBar();
                setFormState({ ...initialState });
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur!'
                })
            }
        }

        console.log({ preparedData });
    }

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.client)
            _errors['client'] = 'Ce champs est obligatoire.'
        if (!formState.amount || formState.amount <= 0)
            _errors['amount'] = 'Ce champs est obligatoire.'
        if (!formState.type)
            _errors['type'] = 'Ce champs est obligatoire.'
        if (!formState.date)
            _errors['date'] = 'Ce champs est obligatoire.'
        if (!formState.dueDate && formState.type?.isBankRelatedItem)
            _errors['dueDate'] = 'Ce champs est obligatoire.'
        if (!formState.comment && formState.type?.isBankRelatedItem)
            _errors['comment'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    console.log({ formState });
    return (
        <div>
            <TitleIcon title="Paiement" Icon={AccountBalanceWalletOutlinedIcon} />
            <Box flexDirection="column" display="flex" mt={2}>
                {!isFromDocument && <ClientAutocomplete
                    label="Client"
                    value={formState.client}
                    onChange={(_, value) => setFormState(_formState => ({ ...formState, client: value }))}
                    errorText={formErrors.client}
                />}
                <TextField
                    name="amount"
                    variant="outlined"
                    type="number"
                    size="small"
                    value={formState.amount}
                    onChange={onFieldChange}
                    label="Montant"
                    margin="normal"
                    error={Boolean(formErrors.amount)}
                    helperText={formErrors.amount}
                />
                <Autocomplete
                    options={paiementMethods}
                    disableClearable
                    autoHighlight
                    value={formState.type}
                    onChange={(_, value) => setFormState(_formState => ({ ...formState, type: value }))}
                    size="small"
                    getOptionLabel={(option) => option?.name}
                    renderInput={(params) => (
                        <TextField
                            onChange={() => null}
                            {...params}
                            margin="normal"
                            label="Mode de paiement"
                            variant="outlined"
                            inputProps={{
                                ...params.inputProps,
                                autoComplete: 'new-password',
                                type: 'search',
                                margin: 'normal'
                            }}
                            error={Boolean(formErrors.type)}
                            helperText={formErrors.type}
                        />
                    )}
                />
                <DatePicker
                    value={formState.date}
                    onChange={(_date) => setFormState(_formState => ({ ...formState, date: _date }))}
                    margin="normal"
                    label="Date de paiement"
                    error={Boolean(formErrors.date)}
                    helperText={formErrors.date}
                />
                {formState.type?.isBankRelatedItem && <DatePicker
                    value={formState.dueDate}
                    onChange={(_date) => setFormState(_formState => ({ ...formState, dueDate: _date }))}
                    margin="normal"
                    label="Date d'échéance"
                    error={Boolean(formErrors.dueDate)}
                    helperText={formErrors.dueDate}
                />}
                <TextField
                    variant="outlined"
                    name="comment"
                    size="small"
                    value={formState.comment}
                    onChange={onFieldChange}
                    label="Description (numéro de chèque, effet ...)"
                    margin="normal"
                    error={Boolean(formErrors.comment)}
                    helperText={formErrors.comment}
                />
                {formState.type?.isBankRelatedItem&&!formState.type?.isDebit&&<FormControlLabel
                    control={<Switch
                        checked={formState.isCashed}
                        onChange={(_, checked) => setFormState(_formState => ({
                            ...formState,
                            isCashed: checked
                        })
                        )} />}
                    label="Encaissé"
                />}
                <Box mt={2} display="flex" justifyContent="flex-end" onClick={save}>
                    <Button variant="contained" color="primary">
                        Enregistrer
                    </Button>
                </Box>

            </Box>
        </div>
    )
}

export default PaiementClientForm