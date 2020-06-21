export const formatMoney = (amount=0) => {
    console.log({amount})
    return amount.toLocaleString('fr', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).replace(',', '.')
}