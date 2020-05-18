
const BASE_URL = '/DashboardStatistics/'

export const getProfitAndTurnover = async (idSite) => {
    const parsedParams = new URLSearchParams({
        IdSite: idSite,
    }).toString();
    const URL = BASE_URL+'ProfitAndTurnover?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}