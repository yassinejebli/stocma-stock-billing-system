import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintClientAccountSummaryURL } from '../../../../utils/urlBuilder';
import { useSite } from '../../../providers/SiteProvider';

const PrintClientAccountSummary = ({ clientId, dateFrom, dateTo, onClose, onExited, open }) => {
    const { useVAT } = useSite();
    const [showStamp, setShowStamp] = React.useState(false);

    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintClientAccountSummaryURL({
                id: clientId,
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString(),
                showStamp: showStamp
            })}>
            <Box p={1}>
                {document &&
                    <>
                        <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                            {/* {!useVAT && <FormControlLabel
                                control={<Switch
                                    checked={showBalance}
                                    onChange={(_, checked) => setShowBalance(checked)} />}
                                label="Afficher le solde"
                            />
                            } */}
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

export default PrintClientAccountSummary;