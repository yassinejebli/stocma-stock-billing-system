
const BASE_URL = '/DashboardStatistics/'

export const getMonthlyProfitAndTurnover = async (idSite) => {
    const parsedParams = new URLSearchParams({
        IdSite: idSite,
    }).toString();
    const URL = BASE_URL+'MonthlyProfitAndTurnover?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}

export const getDailyProfitAndTurnover = async (idSite) => {
    const parsedParams = new URLSearchParams({
        IdSite: idSite,
    }).toString();
    const URL = BASE_URL+'DailyProfitAndTurnover?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}