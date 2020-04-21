import React from 'react';
import SuccessSnackBar from '../elements/snack-bars/SuccessSnackBar';

const SnackBarContext = React.createContext({
});

export const useSnackBar = () => {
    const context = React.useContext(SnackBarContext);
    if (!context) {
        throw new Error('useSnackBar must be used within a SnackBarProvider')
    }
    return context
}


const SnackBarProvider = ({ children }) => {
    const [open, setOpen] = React.useState(false);
    const [error, setError] = React.useState(false);
    const [text, setText] = React.useState('');
    const openSnackBar = ({
        error = false,
        text
    } = {}) => {
        setError(error);
        setText(text);
        setOpen(true);
    }
    return (
        <SnackBarContext.Provider value={{
            showSnackBar: openSnackBar
        }}>
            <SuccessSnackBar
                text={text}
                error={error} open={open}
                onClose={() => setOpen(false)}
            />
            {children}
        </SnackBarContext.Provider>)
};

export default SnackBarProvider
