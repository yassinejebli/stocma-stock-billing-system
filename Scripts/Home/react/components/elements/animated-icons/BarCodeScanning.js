import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { grey } from '@material-ui/core/colors';
/*
@for $i from 1 through 24 {
li:nth-child(#{$i}){
$w: random(5);
width: #{$w}px;
}
}
*/
const useStyles = makeStyles({
    '@keyframes beam': {
        '50%': {
            opacity: 0
        }
    },
    '@keyframes scanning': {
        '50%': {
            transform: 'translateY(34px)'
        }
    },
    root: {
        position: 'relative',
        maxWidth: 200,
        height: 40,
        textAlign: 'center',
    },
    laser: {
        width: '100%',
        backgroundColor: 'tomato',
        height: 1,
        position: 'absolute',
        top: 0,
        zIndex: 2,
        boxShadow: '0 0 4px red',
        animation: '$scanning 2s infinite',
    },
    diode: {
        animation: '$beam 0.01s infinite',
    },
    barcode: {
        fontFamily: "'Libre Barcode 39'",
        fontSize: 36,
        width: '100%',
        height: '100%',
        color: props=>props.scanning ? '#000' : grey[500],
    }
});

const BarCodeScanning = ({ scanning }) => {
    const classes = useStyles({ scanning });
    return (
        <div className={classes.root}>
            <div className={classes.barcode}>
                *1234*
            </div>
            {scanning && <div className={classes.diode}>
                <div className={classes.laser}></div>
            </div>}
        </div>
    )
}

export default BarCodeScanning;
