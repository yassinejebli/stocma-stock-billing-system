import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Drawer, Box, Switch, FormControlLabel } from '@material-ui/core';
import TitleIcon from '../misc/TitleIcon';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { useSettings } from '../../providers/SettingsProvider';

export const useStyles = makeStyles(theme => ({
    root: {
        padding: '26px',
        display: 'flex',
        flexDirection: 'column',
        width: 400
    },
    header: {
        fontSize: 28,
        marginBottom: 12
    },

}));

export const SettingsDialog = ({ children, onExited, ...props }) => {
    const {
        devisDiscount,
        setDevisDiscount,
        devisValidity,
        setDevisValidity,
        devisPayment,
        setDevisPayment,
        devisTransport,
        setDevisTransport,
        devisDeliveryTime,
        setDevisDeliveryTime,
        BLDiscount,
        setBLDiscount,
        BLPayment,
        setBLPayment
    } = useSettings();
    const classes = useStyles();

    return (
        <Drawer anchor="right" {...props}>
            <div className={classes.root}>
                <div className={classes.header}>Paramétrage</div>
                <Box flexDirection="column" display="flex" mt={2}>
                    <TitleIcon title="Devis" Icon={DescriptionOutlinedIcon} />
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisDiscount?.Enabled}
                                onChange={(_, checked) => setDevisDiscount(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Remise"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisValidity?.Enabled}
                                onChange={(_, checked) => setDevisValidity(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Validité de l'offre"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisPayment?.Enabled}
                                onChange={(_, checked) => setDevisPayment(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Mode de paiement"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisTransport?.Enabled}
                                onChange={(_, checked) => setDevisTransport(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Transport / Expédition"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisDeliveryTime?.Enabled}
                                onChange={(_, checked) => setDevisDeliveryTime(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Délai de livraision"
                        />
                    </Box>
                </Box>
                <Box flexDirection="column" display="flex" mt={4}>
                    <TitleIcon title="Bon de livraison" Icon={DescriptionOutlinedIcon} />
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={BLDiscount?.Enabled}
                                onChange={(_, checked) => setBLDiscount(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Remise"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={BLPayment?.Enabled}
                                onChange={(_, checked) => setBLPayment(_devisSetting => ({ ..._devisSetting, Enabled: checked }))} />}
                            label="Mode de paiement"
                        />
                    </Box>
                </Box>
            </div>
        </Drawer>
    )
}

export default SettingsDialog