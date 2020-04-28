import buildQuery from 'odata-query'
const TABLE = 'Fournisseurs';

export const getFournisseurs = async (filters) => {
    if(!filters['Name']) return [];
    const allParams = buildQuery({ 
        filter: filters, 
        top: 20, 
        skip: 0,
    });
  
    const URL = '/Odata/' + TABLE + allParams;

    try {
        const res = await ( await fetch(URL) ).json();
        return res?.value
      } catch(e) { 
          console.log(e); 
          return [];
    }
}

export const getAllFournisseurs = async () => {
    const URL = '/Odata/' + TABLE;
    try {
        const res = await ( await fetch(URL) ).json();
        return res?.value
      } catch(e) { 
          console.log(e); 
          return [];
    }
}