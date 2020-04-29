import React from 'react';
import { useTitle } from '../../providers/TitleProvider';

const Dashboard = () => {
    const { setTitle } = useTitle();
    React.useEffect(() => {
        setTitle('Tableau du bord')
    }, []);

    return (
        <>
            Dashboard
        </>
    )
}

export default Dashboard;
