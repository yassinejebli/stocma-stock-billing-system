import React from 'react';
import { createMuiTheme, ThemeProvider as Provider } from '@material-ui/core/styles';

const theme = createMuiTheme({
    palette: {
        primary: {
            main: '#3478c9'
        },
        secondary: {
            main: '#7290af',
            light: '#FFFFFF'
        },
    },
    status: {
        danger: '#F1F1F1',
    },
    // overrides: {
    //     MuiMenuItem: {
    //         root:{
    //             fontSize: 10
    //         },
    //         dense:{
    //             fontSize: 10
    //         }
    //     }
    //   },
    overrides: {
        MuiListItemText:{
            root:{
                '& .MuiTypography-body1':{
                    fontSize: '1rem'
                }
            }
        }
    }
});

const ThemeProvider = ({ children }) => (
    <Provider theme={theme}>
        {children}
    </Provider>
);

export default ThemeProvider;
