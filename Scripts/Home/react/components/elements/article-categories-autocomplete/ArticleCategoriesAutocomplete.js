import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getAllData } from '../../../queries/crudBuilder';
import useDebounce from '../../../hooks/useDebounce';
import Input from '../input/Input';
import { useSite } from '../../providers/SiteProvider';

const useStyles = makeStyles({
  root: {

  },
});

const ArticleCategoriesAutocomplete = ({ errorText, inTable, withArticles, ...props }) => {
  const {siteId} = useSite();
  const classes = useStyles();
  const [searchText, setSearchText] = React.useState('');
  const [categories, setCategories] = React.useState([]);
  const debouncedSearchText = useDebounce(searchText);
  React.useEffect(() => {
    getAllData('Categories', null, withArticles ? ['Articles($expand=ArticleSites)'] : null).then(response => {
      setCategories(response.map(x => {
        console.log({xxx: x.Articles})
        return ({
          ...x,
          Articles: x.Articles ? x.Articles.map(y => ({
            ...y,
            QteStock: y.ArticleSites?.find(z=>z.IdSite === siteId)?.QteStock,
          })) : [],
        })
      }));
    });
  }, [debouncedSearchText]);
  console.log({test: categories})
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
