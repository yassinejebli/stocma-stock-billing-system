import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getClients } from '../../../queries/clientQueries';
import useDebounce from '../../../hooks/useDebounce';
import { grey } from '@material-ui/core/colors';
import { formatMoney } from '../../../utils/moneyUtils';

const useStyles = makeStyles({
  root: {

  },
  solde: {
    fontSize: 12,
    color: grey[700]
  }
});


const ClientAutocomplete = ({ errorText, ...props }) => {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  const [clients, setClients] = React.useState([]);
  const [searchText, setSearchText] = React.useState('');
  const debouncedSearchText = useDebounce(searchText);

  React.useEffect(() => {
    setLoading(true);
    getClients('Name', debouncedSearchText?.toUpperCase()).then(response => {
      setLoading(false);
      setClients(response);
    });
  }, [debouncedSearchText]);

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  return (
    <Autocomplete
      {...props}
      loading={loading}
      loadingText="Chargement..."
      disableClearable
      popupIcon={null}
      // freeSolo
      style={{ minWidth: 240 }}
      options={clients}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.Name}
      renderOption={option => {
        const soldeColor = (option.Solde > option?.Plafond && option?.Plafond !== 0) ? 'red' :grey[700];
        return (<div>
          <div>{option?.Name}</div>
          <div style={{color: soldeColor}} className={classes.solde}>Solde: {formatMoney(option?.Solde || 0)}</div>
        </div>)
      }}
      renderInput={(params) => (
        <TextField
          onChange={onChangeHandler}
          {...params}
          placeholder="Choisir un client..."
          error={Boolean(errorText)}
          helperText={errorText}
          variant="outlined"
          fullWidth
          inputProps={{
            ...params.inputProps,
            autoComplete: 'new-password', // disable autocomplete and autofill
            type: 'search',
            margin: 'normal'
          }}
        />
      )}
    />
  )
}

export default ClientAutocomplete;
