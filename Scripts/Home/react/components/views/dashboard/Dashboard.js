import React from 'react';
import { useTitle } from '../../providers/TitleProvider';
import ProfitAndExpenses from './ProfitAndExpenses';

const Dashboard = () => {
    const { setTitle } = useTitle();
    React.useEffect(() => {
        setTitle('Tableau du bord')
    }, []);

    return (
        <div>
            <div>
                <ProfitAndExpenses />
            </div>
        </div>
    )
}

export default Dashboard;
