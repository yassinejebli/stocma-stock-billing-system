import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintBonLivraisonURL } from '../../../../utils/urlBuilder';
import PaiementClientForm from '../../forms/PaiementClientForm';

const DOCUMENT_ITEMS = 'BonLivraisonItems'

const PrintBL = ({document, onClose, onExited, open}) => {
    const [showForm, setShowForm] = React.useState(false);
    const [bigFormat, setBigFormat] = React.useState(false);
    const [showBalance, setShowBalance] = React.useState(false);
    const [hidePrices, setHidePrices] = React.useState(false);

    if(!document) return null;
    return (
        <IframeDialog
                onExited={onExited}
                open={open}
                onClose={onClose}
                src={getPrintBonLivraisonURL({
                    IdBonLivraison: document.Id,
                    showBalance,
                    showPrices: !hidePrices,
                    bigFormat,
                })}>
                <Box p={1}>
                    {document &&
                        <>
                            <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                                <FormControlLabel
                                    control={<Switch
                                        checked={bigFormat}
                                        onChange={(_, checked) => setBigFormat(checked)} />}
                                    label="Grand format"
                                />
                                <FormControlLabel
                                    control={<Switch
                                        checked={hidePrices}
                                        onChange={(_, checked) => setHidePrices(checked)} />}
                                    label="Cacher les prix"
                                />
                                <FormControlLabel
                                    control={<Switch
                                        checked={showBalance}
                                        onChange={(_, checked) => setShowBalance(checked)} />}
                                    label="Afficher solde"
                                />
                            </Box>
                            <div>
                                <FormControlLabel
                                    control={<Switch
                                        checked={showForm}
                                        onChange={(_, checked) => setShowForm(checked)}
                                    />}
                                    label="Paiement reçu"
                                />
                            </div>
                            {showForm &&
                                <Box mt={2}>
                                    <PaiementClientForm
                                        amount={document[DOCUMENT_ITEMS]?.reduce((sum, curr) => (
                                            sum += curr.Pu * curr.Qte
                                        ), 0)}
                                        document={document}
                                        onSuccess={() => {
                                            //a workaround to refresh document
                                            const iframe = window.document.getElementById('iframe-dialog');
                                            iframe.src = iframe.src;
                                            setShowForm(false);
                                        }}
                                    />
                                </Box>}
                        </>
                    }
                </Box>
            </IframeDialog>
    )
}

export default PrintBL;