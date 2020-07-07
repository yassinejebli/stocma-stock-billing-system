import 'date-fns';
import React from 'react';
import frLocale from "date-fns/locale/fr";
import DateFnsUtils from '@date-io/date-fns';
import {
  MuiPickersUtilsProvider,
  DatePicker as MuiDatePicker,
} from '@material-ui/pickers';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles(theme=>({
  root: {
    minWidth: 240,
    // margin: '16px 0 0 0'
  }
}))

export default function DatePicker(props) {
  const classes = useStyles();
  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils} locale={frLocale}>
      <MuiDatePicker
        {...props}
        size="small"
        autoOk
        inputVariant="outlined"
        format="dd/MM/yyyy"
        cancelLabel="Annuler"
        clearLabel="Effacer"
        className={classes.root}
      />
    </MuiPickersUtilsProvider>
  )
}