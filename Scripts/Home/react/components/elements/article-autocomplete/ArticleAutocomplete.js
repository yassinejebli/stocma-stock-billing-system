import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getArticles } from '../../../queries/articleQueries';
import { grey } from '@material-ui/core/colors';
import Input from '../input/Input';
import { useSite } from '../../providers/SiteProvider';

const useStyles = makeStyles({
  qte: {
    fontSize: 12,
    color: grey[700]
  }
});

const ArticleAutocomplete = ({ inTable, placeholder, ...props }) => {
  const { siteId } = useSite(); 
  const classes = useStyles();
  const [articles, setArticles] = React.useState([]);

  React.useEffect(()=>{
    setArticles([]);
  }, [siteId])
  const onChangeHandler = async ({ target: { value } }) => {
    const data = await getArticles({
      'Article/Designation': value ? {
        contains: value
      }:undefined,
      IdSite: siteId,
      Disabled: false
    });
    setArticles(data.map(x=>({
      ...x.Article,
      QteStock: x.QteStock
    })));
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
        return option?.Designation}}
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

export default ArticleAutocomplete;
