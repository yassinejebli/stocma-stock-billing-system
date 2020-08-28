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

const BonLivraisonAutocomplete = ({ errorText, clientId, withoutMultiple, ...props }) => {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  const [bonLivraisons, setBonLivraisons] = React.useState([]);
  const [searchText, setSearchText] = React.useState('');
  const debouncedSearchText = useDebounce(searchText);

  React.useEffect(() => {
    setLoading(true);
    getData('BonLivraisons', null, {
      NumBon: debouncedSearchText ? {
        contains: debouncedSearchText
      } : undefined,
      IdClient: clientId ? { eq: { type: 'guid', value: clientId } } : undefined,
      IdFacture: null
    }, ['Facture', 'BonLivraisonItems/Article']).then(response => {
      setLoading(false);
      setBonLivraisons(response.data);
    });
  }, [debouncedSearchText, clientId]);

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
      // freeSolo
      style={{ minWidth: 240 }}
      options={bonLivraisons}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.NumBon}
      renderOption={option => {
        const discount = option.BonLivraisonItems.reduce((sum, curr) => {
          const total = curr.Pu * curr.Qte;
          if (curr.Discount) {
              if (!curr.PercentageDiscount)
                  sum += Number(curr.Discount)
              else
                  sum += total * parseFloat(curr.Discount) / 100;
          }
          return sum;
      }, 0);

      const total = option.BonLivraisonItems.reduce((sum, curr) => (
          sum += curr.Pu * curr.Qte
      ), 0);
        return (<div>
          <div>{option?.NumBon}</div>
          <div className={classes.smallText}>Total: {formatMoney(total - discount)}</div>
          <div className={classes.smallText}>Date: {format(new Date(option.Date), 'dd/MM/yyyy')}</div>
        </div>)
      }}
      renderInput={(params) => (
        <TextField
          onChange={onChangeHandler}
          {...params}
          placeholder="Importer un BL..."
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

export default BonLivraisonAutocomplete;
