import React from 'react';

const Dashboard = () => {
    React.useEffect(() => {
        document.title = 'Tableau du bord'
    }, []);

    return (
        <>
            Dashboard
        </>
        )
}

export default Dashboard;
