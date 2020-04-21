import 'date-fns';
import React from 'react';
import frLocale from "date-fns/locale/fr";
import DateFnsUtils from '@date-io/date-fns';
import {
  MuiPickersUtilsProvider,
  DatePicker as MuiDatePicker,
} from '@material-ui/pickers';

export default function DatePicker(props) {
  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils} locale={frLocale}>
        <MuiDatePicker
         {...props}
         size="small"
         autoOk
         inputVariant="outlined"
          format="dd/MM/yyyy"
          cancelLabel="Annuler"
        />
    </MuiPickersUtilsProvider>
  )
}