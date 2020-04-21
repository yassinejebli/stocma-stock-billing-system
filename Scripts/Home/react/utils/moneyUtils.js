export const formatMoney = (amount=0) => {
    return amount.toLocaleString('fr', { minimumFractionDigits: 2 }).replace(',', '.')
}