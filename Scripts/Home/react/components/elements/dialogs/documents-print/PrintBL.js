import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintBonLivraisonURL } from '../../../../utils/urlBuilder';
import PaiementClientForm from '../../forms/PaiementClientForm';
import { useSite } from '../../../providers/SiteProvider';
import { useAuth } from '../../../providers/AuthProvider';
import { useSettings } from '../../../providers/SettingsProvider';

const DOCUMENT_ITEMS = 'BonLivraisonItems'

const PrintBL = ({document, typePaiement, onClose, onExited, open}) => {
    const { canManagePaiementsClients } = useAuth();

    const isClientDivers = Boolean(document?.Client?.IsClientDivers);
    const {useVAT} = useSite();
    const {paiementModule} = useSettings();
    const [showForm, setShowForm] = React.useState(typePaiement && !isClientDivers && canManagePaiementsClients);
    const [bigFormat, setBigFormat] = React.useState(document?.WithDiscount);
    const [showBalance, setShowBalance] = React.useState(false);
    const [hidePrices, setHidePrices] = React.useState(false);
    const [showStamp, setShowStamp] = React.useState(false);

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
                    showStamp: showStamp
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
                                {!useVAT&&paiementModule?.Enabled&&<FormControlLabel
                                    control={<Switch
                                        checked={showBalance}
                                        onChange={(_, checked) => setShowBalance(checked)} />}
                                    label="Afficher le solde"
                                />}
                                <FormControlLabel
                                    control={<Switch
                                        checked={showStamp}
                                        onChange={(_, checked) => setShowStamp(checked)} />}
                                    label="Afficher le cachet"
                                />
                            </Box>
                            {!isClientDivers&&!useVAT&&canManagePaiementsClients&&paiementModule?.Enabled&&<div>
                                <FormControlLabel
                                    control={<Switch
                                        checked={showForm}
                                        onChange={(_, checked) => setShowForm(checked)}
                                    />}
                                    label="Paiement reçu"
                                />
                            </div>}
                            {showForm && !useVAT && !isClientDivers &&
                                <Box mt={2}>
                                    <PaiementClientForm
                                        amount={document[DOCUMENT_ITEMS]?.reduce((sum, curr) => (
                                            sum += curr.Pu * curr.Qte
                                        ), 0)}
                                        typePaiement={typePaiement}
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