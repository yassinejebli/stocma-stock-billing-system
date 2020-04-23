import React from 'react';
import { useSite } from './SiteProvider';

const ArticleDataContext = React.createContext({
    articles: []
});

export const useArticleData = () => {
    const context = React.useContext(SiteContext);
    if (!context) {
        throw new Error('useArticleData must be used within a ArticleDataProvider')
    }
    return context
}


const ArticleDataProvider = ({children}) => {
    const [articles, setArticles] = React.useState([]);

    React.useEffect(()=>{
        
    },[]);

    return (
        <ArticleDataContext.Provider value={{
            articles
        }}>
            {children}
        </ArticleDataContext.Provider>)
};

export default ArticleDataProvider
