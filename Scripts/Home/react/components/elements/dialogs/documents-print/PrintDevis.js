import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintDevisURL } from '../../../../utils/urlBuilder';

const PrintDevis = ({document, onClose, onExited, open}) => {
    const [showStamp, setShowStamp] = React.useState(false);

    if(!document) return null;
    return (
        <IframeDialog
                onExited={onExited}
                open={open}
                onClose={onClose}
                src={getPrintDevisURL({
                    IdDevis: document.Id,
                    ShowStamp: showStamp,
                    Cachet: showStamp
                })}>
                <Box p={1}>
                    {document &&
                        <>
                            <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                                <FormControlLabel
                                    control={<Switch
                                        checked={showStamp}
                                        onChange={(_, checked) => setShowStamp(checked)} />}
                                    label="Afficher le cachet"
                                />
                            </Box>
                        </>
                    }
                </Box>
            </IframeDialog>
    )
}

export default PrintDevis;