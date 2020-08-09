import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Avatar } from '@material-ui/core';

const border = '1px solid #d8d8d8';

const useStyles = makeStyles(theme => ({
    item: {
        display: 'flex',
        alignItems: 'center',
        paddingBottom: props=>props.noBorder?0:16,
        borderBottom: props=>props.noBorder?'none':border,
        cursor: 'pointer',
        '&:hover .MuiAvatar-root': {
            backgroundColor: '#22496f'
        }
    },
    content: {
        marginLeft: 12,
        maxWidth: 400
    },
    title: {
        fontWeight: 500,
        fontSize: 16
    },
    description: {
        marginTop: 6,
        opacity: 0.8
    },
    avatar: {
        height: 50,
        width: 50,
        backgroundColor: '#7290af'
    },
    icon: {
        height: 24,
        width: 24
    }
}));


const TitleIcon = ({ title, description, Icon, noBorder }) => {
    const classes = useStyles({noBorder: noBorder});
    return (
        <div className={classes.item}>
            <Avatar className={classes.avatar}>
                <Icon className={classes.icon} />
            </Avatar>
            <div className={classes.content}>
                <div className={classes.title}>{title}</div>
                {description&&<div className={classes.description} style={{marginTop: 0}}>{description}</div>}
            </div>
        </div>
    )
}

export default TitleIcon