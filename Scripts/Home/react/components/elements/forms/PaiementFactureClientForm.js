import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Box, Button, TextField, FormControlLabel, Switch } from '@material-ui/core';
import DatePicker from '../date-picker/DatePicker';
import TitleIcon from '../misc/TitleIcon';
import AccountBalanceWalletOutlinedIcon from '@material-ui/icons/AccountBalanceWalletOutlined';
import ClientAutocomplete from '../client-autocomplete/ClientAutocomplete';
import { saveData, updateData } from '../../../queries/crudBuilder';
import { useSnackBar } from '../../providers/SnackBarProvider';
import TypePaiementAutocomplete from '../type-paiement-autocomplete/TypePaiementAutocomplete';
import { useAuth } from '../../providers/AuthProvider';
import { useLoader } from '../../providers/LoaderProvider';

export const useStyles = makeStyles(theme => ({
    root: {

    }
}));

const TABLE = 'PaiementFactures'

//TODO: should get these data from backend
export const paiementMethods = [
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb2',
        name: 'Espéce',
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb3',
        name: 'Chéque',
        IsBankRelated: true,
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeecb4',
        name: 'Effet',
        IsBankRelated: true,
    },
    {
        id: '399d159e-9ce0-4fcc-957a-08a65bbeece1',
        name: 'Impayé',
        IsBankRelated: true,
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

const PaiementClientForm = ({ document, amount, typePaiement, paiement, onSuccess, isAvoir }) => {
    const { showLoader } = useLoader();
    const { showSnackBar } = useSnackBar();
    const { canManagePaiementsClients } = useAuth();
    if (!canManagePaiementsClients) return null;
    const [formState, setFormState] = React.useState({
        ...initialState,
        date: new Date()
    });
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
                type: typePaiement,
                IdBonLivraison: document.Id
            }));
        }
        if (isEditMode) {
            setFormState(_formState => ({
                ..._formState,
                amount: paiement.Credit || paiement.Debit, //TODO: change this
                client: paiement.Client,
                type: paiement.TypePaiement,
                comment: paiement.Comment,
                date: paiement.Date,
                dueDate: paiement.DateEcheance,
                isCashed: paiement.EnCaisse
            }));
        }
    }, []);

    const save = async () => {
        if (!isFormValid()) return;

        showLoader(true);
        const preparedData = {
            IdTypePaiement: formState.type.Id,
            IdClient: formState.client.Id,
            Credit: formState.type.IsDebit ? 0 : formState.amount,
            Debit: formState.type.IsDebit ? formState.amount : 0,
            Date: formState.date,
            DateEcheance: formState.dueDate,
            Comment: formState.comment,
            EnCaisse: formState.isCashed
        }

        if (isEditMode) {
            const response = await updateData(TABLE, { ...preparedData, Id: paiement.Id, ModificationDate: new Date() }, paiement.Id);
            if (response.ok) {
                setFormState({ ...initialState });
                showSnackBar({
                    text: 'Le paiement est bien enregistré'
                });
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
                showSnackBar({
                    text: 'Le paiement est bien enregistré'
                });
                setFormState({ ...initialState });
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: 'Erreur!'
                })
            }
        }

        showLoader(false);
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
        if (!formState.dueDate && formState.type?.IsBankRelated)
            _errors['dueDate'] = 'Ce champs est obligatoire.'
        if (!formState.comment && formState.type?.IsBankRelated)
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
                    disabled={isEditMode}
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
                <Box mt={1}>
                    <TypePaiementAutocomplete
                        showAllPaymentMethods
                        value={formState.type}
                        errorText={formErrors.type}
                        onChange={(_, value) => setFormState(_formState => ({ ...formState, type: value }))}
                    />
                </Box>
                <DatePicker
                    value={formState.date}
                    onChange={(_date) => setFormState(_formState => ({ ...formState, date: _date }))}
                    margin="normal"
                    label="Date de paiement"
                    error={Boolean(formErrors.date)}
                    helperText={formErrors.date}
                />
                {formState.type?.IsBankRelated && <DatePicker
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
                {formState.type?.IsBankRelated && !formState.type?.IsDebit && <FormControlLabel
                    control={<Switch
                        checked={formState.isCashed}
                        onChange={(_, checked) => setFormState(_formState => ({
                            ...formState,
                            isCashed: checked
                        })
                        )} />}
                    label="Encaissé"
                />}
                <Box mt={1} display="flex" justifyContent="flex-end" onClick={save}>
                    <Button variant="contained" color="primary">
                        Enregistrer
                    </Button>
                </Box>

            </Box>
        </div>
    )
}

export default PaiementClientForm