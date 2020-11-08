import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import useDebounce from '../../../hooks/useDebounce';
import { grey } from '@material-ui/core/colors';
import { formatMoney } from '../../../utils/moneyUtils';
import { getData } from '../../../queries/crudBuilder';
import { format } from 'date-fns';

const useStyles = makeStyles({
  root: {

  },
  smallText: {
    fontSize: 12,
    color: grey[700]
  }
});

const BonReceptionAutocomplete = ({ errorText, fournisseurId, withoutMultiple, ...props }) => {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  const [bonReceptions, setBonReceptions] = React.useState([]);
  const [searchText, setSearchText] = React.useState('');
  const debouncedSearchText = useDebounce(searchText);

  React.useEffect(() => {
    setLoading(true);
    getData('BonReceptions', null, {
      NumBon: debouncedSearchText ? {
        contains: debouncedSearchText
      } : undefined,
      IdFournisseur: fournisseurId ? { eq: { type: 'guid', value: fournisseurId } } : undefined,
      IdFactureF: null
    }, ['FactureF', 'BonReceptionItems/Article']).then(response => {
      setLoading(false);
      setBonReceptions(response.data);
    });
  }, [debouncedSearchText, fournisseurId]);

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  return (
    <Autocomplete
      {...props}
      multiple={!Boolean(withoutMultiple)}
      filterSelectedOptions
      popupIcon={null}
      forcePopupIcon={false}
      style={{ minWidth: 240 }}
      options={bonReceptions}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.NumBon}
      renderOption={option => {
      const total = option.BonReceptionItems.reduce((sum, curr) => (
          sum += curr.Pu * curr.Qte
      ), 0);
        return (<div>
          <div>{option?.NumBon}</div>
          <div className={classes.smallText}>Total: {formatMoney(total)}</div>
          <div className={classes.smallText}>Date: {format(new Date(option.Date), 'dd/MM/yyyy')}</div>
        </div>)
      }}
      renderInput={(params) => (
        <TextField
          onChange={onChangeHandler}
          {...params}
          placeholder="Importer un BR..."
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

export default BonReceptionAutocomplete;
