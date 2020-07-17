const BASE_URL = '/DashboardStatistics/'

export const getMonthlyProfitAndTurnover = async (idSite, year) => {
    const parsedParams = new URLSearchParams({
        year: year,
        IdSite: idSite,
    }).toString();
    const URL = BASE_URL+'MonthlyProfitAndTurnover?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const getMonthlyProfitAndCash = async (idSite, year) => {
    const parsedParams = new URLSearchParams({
        year: year,
    }).toString();
    const URL = BASE_URL+'MonthlyProfitAndCash?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const getDailyProfitAndTurnover = async (idSite, year) => {
    const parsedParams = new URLSearchParams({
        year: year,
        IdSite: idSite,
    }).toString();
    const URL = BASE_URL+'DailyProfitAndTurnover?'+parsedParams;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}