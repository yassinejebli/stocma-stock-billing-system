import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { useSite } from '../../providers/SiteProvider';

const useStyles = makeStyles({
  root: {

  },
});

const SiteAutocomplete = ({ errorText, ...props }) => {
  const classes = useStyles();
  const {sites} = useSite();

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  return (
    <Autocomplete
      {...props}
      disableClearable
      popupIcon={null}
      forcePopupIcon={false}
      style={{ minWidth: 240 }}
      options={sites}
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
          placeholder="Choisir un dépôt/magasin..."
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

export default SiteAutocomplete;
