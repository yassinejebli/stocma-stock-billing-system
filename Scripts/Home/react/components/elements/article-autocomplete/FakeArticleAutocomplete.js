import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getFakeArticles } from '../../../queries/articleQueries';
import { grey } from '@material-ui/core/colors';
import Input from '../input/Input';

const useStyles = makeStyles({
  qte: {
    fontSize: 12,
    color: grey[700]
  }
});


const FakeArticleAutocomplete = ({ inTable, placeholder, ...props }) => {
  const classes = useStyles();
  const [articles, setArticles] = React.useState([]);

  const onChangeHandler = async ({ target: { value } }) => {
    const data = await getFakeArticles({
      'Designation': value ? {
        contains: value
      } : undefined,
      Disabled: false
    });
    setArticles(data);
  }

  return (
    <Autocomplete
      {...props}
      selectOnFocus
      noOptionsText=""
      forcePopupIcon={false}
      disableClearable
      style={{ width: '100%' }}
      options={articles}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => {
        return option?.Designation
      }}
      renderOption={option => (
        <div>
          <div>{option.Designation}</div>
          <div className={classes.qte}>quantité en stock: {option.QteStock}</div>
        </div>
      )}
      renderInput={(params) => (
        <Input
          {...params}
          placeholder={placeholder}
          onChange={onChangeHandler}
          inTable={inTable}
        />
      )}
    />
  )
}

export default FakeArticleAutocomplete;
