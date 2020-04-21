const TABLE = 'Clients';

export const getClients = async (field, text) => {
    if(!text)
        return [];

    const filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1';
  
    const URL = '/Odata/' + TABLE + '?&$top=20' + filterText;

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