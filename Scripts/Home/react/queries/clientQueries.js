import buildQuery from 'odata-query'
const TABLE = 'Clients';

export const getClients = async (filters) => {
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

export const getAllClients = async () => {
    const URL = '/Odata/' + TABLE;
    try {
        const res = await ( await fetch(URL) ).json();
        return res?.value
      } catch(e) { 
          console.log(e); 
          return [];
    }
}

export const getClientsProfit = async (skip, filters) => {
    const parsedParams = new URLSearchParams({
        Skip: skip,
        SearchText: filters.searchText,
        From: filters.dateFrom?.toISOString(),
        To: filters.dateTo?.toISOString(),
    }).toString();
    const URL = '/Statistics/ClientsProfit?' + parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}

export const getClientsNotBuying = async (skip, filters) => {
    const parsedParams = new URLSearchParams({
        Skip: skip,
        SearchText: filters.searchText,
        Months: filters.months,
    }).toString();
    const URL = '/Statistics/ClientsNotBuying?' + parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}