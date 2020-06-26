import React from 'react';
import { useTitle } from '../../providers/TitleProvider';
import ProfitAndExpenses from './ProfitAndExpenses';
import MonthlyProfitAndExpenses from './MonthlyProfitAndExpenses';

const Dashboard = () => {
    const { setTitle } = useTitle();
    React.useEffect(() => {
        setTitle('Tableau du bord')
    }, []);

    return (
        <div>
            <div>
                <ProfitAndExpenses />
                <MonthlyProfitAndExpenses />
            </div>
        </div>
    )
}

export default Dashboard;
