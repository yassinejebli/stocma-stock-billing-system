import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintBonCommandeURL } from '../../../../utils/urlBuilder';
import { FormControlLabel, Switch, Box } from '@material-ui/core';

const PrinBonCommande = ({ document, onClose, onExited, open }) => {
    const [hidePrices, setHidePrices] = React.useState(true);
    const [showStamp, setShowStamp] = React.useState(false);
    if (!document) return null;
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintBonCommandeURL({
                id: document.Id,
                showStamp,
                hidePrices,
            })}>
            <Box p={1}>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <FormControlLabel
                        control={<Switch
                            checked={hidePrices}
                            onChange={(_, checked) => setHidePrices(checked)} />}
                        label="Cacher les prix"
                    />
                    <FormControlLabel
                        control={<Switch
                            checked={showStamp}
                            onChange={(_, checked) => setShowStamp(checked)} />}
                        label="Afficher le cachet"
                    />
                </Box>
            </Box>
        </IframeDialog>
    )
}

export default PrinBonCommande;