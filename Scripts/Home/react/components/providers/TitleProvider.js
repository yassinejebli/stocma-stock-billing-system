import React from 'react';

const TitleContext = React.createContext({
    title: '',
    //...
});

export const useTitle = () => {
    const context = React.useContext(TitleContext);
    if (!context) {
        throw new Error('useTitle must be used within a TitleProvider')
    }
    return context
}


const TitleProvider = ({children}) => {
    const [title, setTitle] = React.useState('');

    const _setTitle = (_title) => {
        setTitle(_title)
        document.title = _title;
    }

    return (
        <TitleContext.Provider value={{
            title,
            setTitle: _setTitle
        }}>
            {children}
        </TitleContext.Provider>)
};

export default TitleProvider
