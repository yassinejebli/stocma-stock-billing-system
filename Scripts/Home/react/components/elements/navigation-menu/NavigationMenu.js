import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import BottomNavigation from '@material-ui/core/BottomNavigation';
import BottomNavigationAction from '@material-ui/core/BottomNavigationAction';
import PostAddIcon from '@material-ui/icons/PostAdd';
import { useHistory } from 'react-router-dom';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';

const useStyles = makeStyles({
  root: {
    position: 'fixed',
    bottom: 0,
    left: 0,
    right: 0,
    height: 64,
    borderTop: '1px solid rgba(0,0,0,0.08)',
    '& .MuiBottomNavigationAction-label':{
      fontSize: 14
    }
  },
  icon: {
    width: 28,
    height: 28
  }
});

const NavigationMenu = () => {
  const classes = useStyles();
  const [selectedMenuItem, setSelectedMenuItem] = React.useState(0);
  const history = useHistory();
  const menuItems = [
    {
      label: 'C.C',
      value: 5,
      icon: <AccountBalanceIcon className={classes.icon} style={{ color: 'rgb(14, 230, 158)' }} />,
      route: '/'
    },
    {
      label: 'B.L',
      value: 0,
      icon: <PostAddIcon className={classes.icon} style={{ color: 'rgb(14, 230, 158)' }} />,
      route: '/BonLivraison'
    },
    {
      label: 'FA',
      value: 2,
      icon: <PostAddIcon className={classes.icon} style={{ color: 'rgb(111, 14, 230)' }} />,
      route: '/Facture'
    },
    {
      label: 'B.R',
      value: 1,
      icon: <PostAddIcon className={classes.icon} style={{ color: 'rgb(14, 140, 230)' }} />,
      route: '/BonReception'
    },
    {
      label: 'C.F',
      value: 3,
      icon: <AccountBalanceIcon className={classes.icon} style={{ color: 'rgb(227, 66, 66)' }} />,
      route: '/'
    },
  ];
  return (
    <BottomNavigation className={classes.root} value={selectedMenuItem} onChange={(_, value) => setSelectedMenuItem(value)}>
      {
        menuItems.map(mi => {
          return <BottomNavigationAction
            key={mi.value}
            value={mi.value} 
            label={mi.label}
            showLabel 
            icon={mi.icon}
            onClick={() => history.replace(mi.route)}
          />
        })
      }
    </BottomNavigation>
  )
}

export default NavigationMenu;
