import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Drawer, Box, Switch, FormControlLabel } from '@material-ui/core';
import TitleIcon from '../misc/TitleIcon';
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { useSettings } from '../../providers/SettingsProvider';
import { BarcodeScan } from 'mdi-material-ui'

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
        setBLPayment,
        factureDiscount,
        setFactureDiscount,
        facturePayment,
        setFacturePayment,
        factureCheque,
        setFactureCheque,
        barcode,
        setBarcode,
        barcodeModule,
    } = useSettings();
    const classes = useStyles();

    return (
        <Drawer anchor="right" {...props}>
            <div className={classes.root}>
                <div className={classes.header}>Paramétrage</div>
                {barcodeModule?.Enabled && <Box flexDirection="column" display="flex" mt={2}>
                    <TitleIcon title="Code à barres" Icon={BarcodeScan} />
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={barcode?.Enabled}
                                onChange={(_, checked) => setBarcode(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Activer le scanner toujours"
                        />
                    </Box>
                </Box>}
                <Box flexDirection="column" display="flex" mt={2}>
                    <TitleIcon title="Devis" Icon={DescriptionOutlinedIcon} />
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisDiscount?.Enabled}
                                onChange={(_, checked) => setDevisDiscount(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Remise"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisValidity?.Enabled}
                                onChange={(_, checked) => setDevisValidity(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Validité de l'offre"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisPayment?.Enabled}
                                onChange={(_, checked) => setDevisPayment(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Mode de paiement"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisTransport?.Enabled}
                                onChange={(_, checked) => setDevisTransport(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Transport / Expédition"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={devisDeliveryTime?.Enabled}
                                onChange={(_, checked) => setDevisDeliveryTime(_setting => ({ ..._setting, Enabled: checked }))} />}
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
                                onChange={(_, checked) => setBLDiscount(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Remise"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={BLPayment?.Enabled}
                                onChange={(_, checked) => setBLPayment(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Mode de paiement"
                        />
                    </Box>
                </Box>
                <Box flexDirection="column" display="flex" mt={4}>
                    <TitleIcon title="Facture" Icon={DescriptionOutlinedIcon} />
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={factureDiscount?.Enabled}
                                onChange={(_, checked) => setFactureDiscount(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Remise"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={facturePayment?.Enabled}
                                onChange={(_, checked) => setFacturePayment(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Mode de paiement"
                        />
                    </Box>
                    <Box mt={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={factureCheque?.Enabled}
                                onChange={(_, checked) => setFactureCheque(_setting => ({ ..._setting, Enabled: checked }))} />}
                            label="Numéro de chèque/effet"
                        />
                    </Box>
                </Box>
            </div>
        </Drawer>
    )
}

export default SettingsDialog