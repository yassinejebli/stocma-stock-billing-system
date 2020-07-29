import React from 'react';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import { useSite } from '../../providers/SiteProvider';
import { FormControl, InputBase, Select, MenuItem, Box } from '@material-ui/core';
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';

const useStyles = makeStyles({
  select: {
    '& .MuiSelect-icon': {
      fill: '#FFF'
    }
  },
  icon: {
    color: '#FFF',
    width: 40,
    height: 40,
    marginRight: 6
  }
});

export const BootstrapInput = withStyles((theme) => ({
  input: {
    minWidth: 150,
    borderRadius: 4,
    position: 'relative',
    backgroundColor: 'transparent',
    border: '1px solid #ced4da',
    fontSize: 16,
    padding: '6px 26px 6px 12px',
    color: '#FFF',
    transition: theme.transitions.create(['border-color', 'box-shadow']),
    // Use the system font instead of the default Roboto font.
    '&:focus': {
      borderRadius: 4
    },
  },
}))(InputBase);

const SiteSelect = () => {
  const { sites, setSite, siteId } = useSite();
  const hasMoreThanOneSite = sites.length > 1;
  const classes = useStyles();

  const handleChange = async ({ target: { value } }) => {
    setSite(sites.find(x => x.Id === value));
  }


  if (!hasMoreThanOneSite) return null;

  return (
    <Box display="flex" alignItems="center">
      <StorefrontOutlinedIcon className={classes.icon} />
      <FormControl className={classes.margin}>
        <Select
          labelId="site"
          value={siteId}
          onChange={handleChange}
          input={<BootstrapInput size="small" />}
          size="small"
          className={classes.select}
        >
          {
            sites.map(x => (
              <MenuItem key={x.Id} value={x.Id}>{x.Name}</MenuItem>
            ))
          }
        </Select>
      </FormControl>
    </Box>
  )
}

export default SiteSelect;
