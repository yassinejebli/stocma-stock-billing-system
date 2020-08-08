import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getAllData } from '../../../queries/crudBuilder';
import useDebounce from '../../../hooks/useDebounce';

const useStyles = makeStyles({
  root: {

  },
});

const TypePaiementAutocomplete = ({ errorText, showAllPaymentMethods, ...props }) => {
  const classes = useStyles();
  const [searchText, setSearchText] = React.useState('');
  const [paymentMethods, setPaymentMethods] = React.useState([]);
  const debouncedSearchText = useDebounce(searchText);
  const filters = showAllPaymentMethods ? {} : {
    IsAvoir: false,
    IsAchat: false,
    IsVente: false,
    IsImpaye: false,
    IsAncien: false,
    IsRemise: false,
    IsRemboursement: false,
  };
  React.useEffect(() => {

    getAllData('TypePaiements', filters).then(response => {
      setPaymentMethods(response);
    });
  }, [debouncedSearchText]);

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  return (
    <Autocomplete
      {...props}
      popupIcon={null}
      forcePopupIcon={false}
      style={{ minWidth: 240 }}
      options={paymentMethods}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.Name}
      renderInput={(params) => (
        <TextField
          onChange={onChangeHandler}
          {...params}
          placeholder="Méthode de paiement..."
          error={Boolean(errorText)}
          helperText={errorText}
          variant="outlined"
          fullWidth
          inputProps={{
            ...params.inputProps,
            autoComplete: 'new-password', // disable autocomplete and autofill
            margin: 'normal'
          }}
        />
      )}
    />
  )
}

export default TypePaiementAutocomplete;
