import React from 'react';
import Loader, { LinearLoader } from '../elements/loaders/Loader';

const LoaderContext = React.createContext({
});

export const useLoader = () => {
    const context = React.useContext(LoaderContext);
    if (!context) {
        throw new Error('useLoader must be used within a LoaderProvider')
    }
    return context
}


const LoaderProvider = ({ children }) => {
    const [loading, setLoading] = React.useState(false);
    const [isLinear, setLinear] = React.useState(false);
    return (
        <LoaderContext.Provider value={{
            showLoader: (loading, linear) => setLoading(()=>{
                setLinear(linear)
                return loading;
            })
        }}>
            {
                isLinear ?
                    <LinearLoader loading={loading} />
                    :
                    <Loader
                        loading={loading}
                    />
            }
            {children}
        </LoaderContext.Provider>)
};

export default LoaderProvider
