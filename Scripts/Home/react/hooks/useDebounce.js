  
import React from 'react';

const useDebounce = (value, delay = 300) => {
    const [debouncedValue, setDebouncedValue] = React.useState(value);

    React.useEffect(
        () => {
            const handler = setTimeout(() => {
                setDebouncedValue(value);
            }, delay);
            return () => {
                clearTimeout(handler);
            };
        },
        [value]
    );
    return debouncedValue;
};

export default useDebounce;