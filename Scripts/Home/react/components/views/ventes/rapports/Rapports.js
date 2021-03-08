import React from 'react'
import { useTitle } from '../../../providers/TitleProvider';
import VentesBL from './VentesBL';
import VentesFA from './VentesFA';
import Transactions from './Transactions';
import VentesParUtilisateurs from './VentesParUtilisateur';
import VentesBLByClient from './VentesBLByClient';

const Rapports = () => {
    const { setTitle } = useTitle();

    React.useEffect(() => {
        setTitle('Rapports')
    }, []);

    return (
        <>
            <VentesBLByClient />
            <VentesBL />
            <VentesFA />
            <Transactions />
            <VentesParUtilisateurs />
        </>
    )
}

export default Rapports;
