import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import useDebounce from '../../../hooks/useDebounce';
import { grey } from '@material-ui/core/colors';
import { formatMoney } from '../../../utils/moneyUtils';
import { getData } from '../../../queries/crudBuilder';
import { useSite } from '../../providers/SiteProvider';

const useStyles = makeStyles({
  root: {

  },
  total: {
    fontSize: 12,
    color: grey[700]
  }
});

const BonLivraisonAutocomplete = ({ errorText, clientId, ...props }) => {
  const { siteId } = useSite();
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
      IdSite: siteId
      // Disabled: false
    }, ['BonLivraisonItems/Article']).then(response => {
      setLoading(false);
      setBonLivraisons(response.data);
    });
  }, [debouncedSearchText, siteId, clientId]);

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  return (
    <Autocomplete
      {...props}
      multiple
      loading={loading}
      loadingText="Chargement..."
      disableClearable
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
        return (<div>
          <div>{option?.NumBon}</div>
          <div className={classes.total}>Total: {formatMoney(option.BonLivraisonItems.reduce((sum, curr) => (
            sum += curr.Pu * curr.Qte
          ), 0))}</div>
        </div>)
      }}
      renderInput={(params) => (
        <TextField
          onChange={onChangeHandler}
          {...params}
          placeholder="Importer des BL..."
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

export default BonLivraisonAutocomplete;
