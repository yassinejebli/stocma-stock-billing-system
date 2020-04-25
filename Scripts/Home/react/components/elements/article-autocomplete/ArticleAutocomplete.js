import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { getArticles } from '../../../queries/articleQueries';
import { grey } from '@material-ui/core/colors';
import Input from '../input/Input';

const useStyles = makeStyles({
  qte: {
    fontSize: 12,
    color: grey[700]
  }

});


const ArticleAutocomplete = ({ inTable, placeholder, ...props }) => {
  const classes = useStyles();
  const [articles, setArticles] = React.useState([]);

  const onChangeHandler = async ({ target: { value } }) => {
    const data = await getArticles('Designation', value);
    setArticles(data);
  }

  return (
    <Autocomplete
      {...props}
      selectOnFocus
      noOptionsText=""
      forcePopupIcon={false}
      disableClearable
      // freeSolo
      style={{ width: '100%' }}
      options={articles}
      classes={{
        option: classes.option,
      }}
      autoHighlight
      size="small"
      getOptionLabel={(option) => option?.Designation}
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
