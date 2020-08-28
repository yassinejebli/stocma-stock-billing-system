import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getAllData } from '../../../queries/crudBuilder';
import useDebounce from '../../../hooks/useDebounce';
import Input from '../input/Input';

const useStyles = makeStyles({
  root: {

  },
});

const ArticleCategoriesAutocomplete = ({ errorText, inTable, ...props }) => {
  const classes = useStyles();
  const [searchText, setSearchText] = React.useState('');
  const [categories, setCategories] = React.useState([]);
  const debouncedSearchText = useDebounce(searchText);
  React.useEffect(() => {
    getAllData('Categories').then(response => {
      setCategories(response);
    });
  }, [debouncedSearchText]);

  const onChangeHandler = async ({ target: { value } }) => {
    setSearchText(value);
  }

  console.log({value: props.value})
  return (
    <Autocomplete
      {...props}
      popupIcon={null}
      forcePopupIcon={false}
      style={{ width: '100%' }}
      options={categories}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.Name}
      renderInput={(params) => (
        <Input
          {...params}
          placeholder="Famille..."
          onChange={onChangeHandler}
          inTable={inTable}
        />
      )}
    />
  )
}

export default ArticleCategoriesAutocomplete;
