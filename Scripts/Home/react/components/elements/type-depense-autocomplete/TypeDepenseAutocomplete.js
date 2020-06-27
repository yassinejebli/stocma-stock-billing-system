import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Autocomplete from '@material-ui/lab/Autocomplete';
import useDebounce from '../../../hooks/useDebounce';
import { grey } from '@material-ui/core/colors';
import { getAllData } from '../../../queries/crudBuilder';
import Input from '../input/Input';

const useStyles = makeStyles({
  root: {

  },
  solde: {
    fontSize: 12,
    color: grey[700]
  }
});


const DepenseAutocomplete = ({ errorText, inTable, placeholder, ...props }) => {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  const [depenses, setDepenses] = React.useState([]);

  React.useEffect(() => {
    setLoading(true);
    getAllData('TypeDepenses').then(response => {
      setLoading(false);
      setDepenses(response);
    });
  }, []);

  return (
    <Autocomplete
      {...props}
      loading={loading}
      loadingText="Chargement..."
      disableClearable
      popupIcon={null}
      forcePopupIcon={false}
      // freeSolo
      style={{ minWidth: 240 }}
      options={depenses}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.Name}
      renderInput={(params) => (
        <Input
          {...params}
          placeholder={placeholder}
          inTable={inTable}
        />
      )}
    />
  )
}

export default DepenseAutocomplete;
