import React from 'react'
import { inputTypes } from '../types/input'
import ArticleAutocomplete from "../components/elements/article-autocomplete/ArticleAutocomplete";
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { format } from 'date-fns';
import { formatMoney } from './moneyUtils';
import { getLastPriceSale } from '../queries/articleQueries';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { Box, Tooltip } from '@material-ui/core';
import HistoryIcon from '@material-ui/icons/History';
import Input from '../components/elements/input/Input';

export const getBonLivraisonColumns = ({ BLDiscount, hasMultipleSites, suiviModule, multiplyPA }) => ([
    {
        Header: 'Article',
        accessor: 'Article',
        type: inputTypes.text.description,
        editable: true,
        width: '50%',
        Cell: ({
            value,
            row: { index },
            column: { id },
            updateMyData,
            addNewRow,
            owner,
            data,
            site,
        }) => {
            // if(multiplyPA && PA){
            //     const splittedPA = PA.substr(0, PA.indexOf("."));
            //     const split = splittedPA[0].split("");
            //     const prefix = split.substr(0, 1);
            //     const suffix = split[1]
            //     const calculatedPAPart = Number(prefix)*5;
                
            // }
            return (
                <>
                    <ArticleAutocomplete
                        value={value}
                        inTable
                        placeholder="Entrer un article..."
                        onBlur={() => updateMyData(index, id, value)}
                        onChange={(_, selectedValue) => {
                            updateMyData(index, id, selectedValue);
                            updateMyData(index, 'Site', site);
                            if (selectedValue && owner)
                                getLastPriceSale(selectedValue.Id, owner.Id).then(lastPriceSale => {
                                    updateMyData(index, 'Pu', lastPriceSale);
                                });
                            else if (selectedValue)
                                updateMyData(index, 'Pu', selectedValue.PVD);

                            if (data.filter(x => !x.Article).length === 1 || data.length === 1)
                                addNewRow();

                            const qteCell = document.querySelector(`#my-table #Qte-${(index)} input`);
                            if (qteCell) {
                                setTimeout(() => {
                                    qteCell.focus();
                                }, 200)
                            }
                        }}

                    />
                    {value && <Box mb={1} style={{
                        color: value.MinStock > value.QteStock ? 'red' : 'green'
                    }}>
                        <Tooltip title={"CODE:" + Math.floor(1000 + Math.random() * 9000) +
                            "-" + formatMoney(multiplyPA ? value.PA * 5 : value.PA) +
                            "-" + Math.floor(1000 + Math.random() * 9000)}>
                            <span>
                                {value.QteStock} en stock
                            </span>
                        </Tooltip>
                    </Box>}
                </>
            )
        }
    },
    (hasMultipleSites && {
        id: 'Site',
        Header: 'Dépôt/Magasin',
        Cell: ({ row: { original } }) => {
            return (
                <Input disabled tabIndex={-1} inTable value={original.Site?.Name} />
            )
        },
        type: inputTypes.text.description,
        width: 80,
    }),
    {
        Header: 'Qte.',
        accessor: 'Qte',
        type: inputTypes.number.description,
        editable: true,
        align: 'right'
    },
    {
        id: 'Pu',
        Header: 'P.U.',
        accessor: 'Pu',
        editable: true,
        type: inputTypes.number.description,
        align: 'right',
        Cell: ({
            value,
            row: { original, index },
            column: { id },
            updateMyData,
        }) => {
            return (
                <>

                    <Input
                        value={value || ''}
                        inTable
                        align="right"
                        type="number"
                        onFocus={(event) => event.target.select()}
                        onChange={({ target: { value: _value } }) => updateMyData(index, id, _value)}
                        onBlur={() => updateMyData(index, id, value)}
                    />
                    {Boolean(value && original?.Article?.PA >= value) && <Box mb={1}>
                        <div style={{
                            marginLeft: 'auto',
                            color: 'red',
                            width: 'fit-content',
                        }}>
                            Vérifier le prix!
                        </div>
                    </Box>}
                </>
            )
        }
    },
    ((BLDiscount?.Enabled) && {
        Header: 'Remise',
        accessor: 'Discount',
        editable: true,
        type: inputTypes.text.description,
        align: 'right'
    }),
    {
        id: 'TotalHT',
        Header: 'Montant',
        accessor: (props) => {
            return formatMoney(props.Pu * props.Qte);
        },
        type: inputTypes.text.description,
        editable: false,
        align: 'right'
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { index, original }, deleteRow, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {suiviModule?.Enabled && original.Article && <Tooltip title="Historiques des ventes">
                        <IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                            <HistoryIcon />
                        </IconButton>
                    </Tooltip>}
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(index)}>
                        <CloseIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24,
        align: 'right'
    },
].filter(x => x))


export const getClientColumns = ({ useVAT, showCodeClient }) => ([
    {
        Header: 'Client',
        accessor: 'Name',
        type: inputTypes.text.description,
        width: '30%'
    },
    (showCodeClient && {
        Header: 'Code',
        accessor: 'CodeClient',
        type: inputTypes.text.description,
        width: 80
    }),
    {
        Header: 'I.C.E',
        accessor: 'ICE',
        type: inputTypes.text.description,
        width: 140
    },
    // {
    //     Header: 'Adresse',
    //     accessor: 'Adresse',
    //     type: inputTypes.text.description,
    //     width: 120
    // },
    {
        Header: 'Tel',
        accessor: 'Tel',
        type: inputTypes.text.description,
        width: 80
    },
    {
        id: 'Solde',
        Header: 'Solde',
        accessor: (props) => {
            const solde = useVAT ? props.SoldeFacture : props.Solde;
            return formatMoney(solde || 0)
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'Plafond',
        Header: 'Plafond',
        accessor: (props) => {
            return props.Plafond ? formatMoney(props.Plafond) : ''
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'update',
        Header: '',
        Cell: ({ row: { original }, updateRow }) => {
            return (
                <div style={{ textAlign: 'center' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { original: { Id } }, deleteRow }) => {
            return (
                <div style={{ textAlign: 'right' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
].filter(x => x))


export const getFournisseurColumns = () => ([
    {
        Header: 'Fournisseur',
        accessor: 'Name',
        type: inputTypes.text.description,
        width: '30%'
    },
    {
        Header: 'I.C.E',
        accessor: 'ICE',
        type: inputTypes.text.description,
        width: 140
    },
    {
        Header: 'Adresse',
        accessor: 'Adresse',
        type: inputTypes.text.description,
        width: 120
    },
    {
        Header: 'Tel',
        accessor: 'Tel',
        type: inputTypes.text.description,
        width: 80
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { original: { Id } }, deleteRow }) => {
            return (
                <div style={{ textAlign: 'right' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
    {
        id: 'update',
        Header: '',
        Cell: ({ row: { original }, updateRow }) => {
            return (
                <div style={{ textAlign: 'center' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
])


export const getBonLivraisonListColumns = ({ canUpdateBonLivraisons, canDeleteBonLivraisons, isAdmin }) => ([
    {
        Header: 'Client',
        accessor: 'Client.Name',
        type: inputTypes.text.description,
        width: 100
    },
    {
        Header: 'N#',
        accessor: 'NumBon',
        type: inputTypes.text.description,
        width: 60
    },
    {
        id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
        width: 60
    },
    {
        Header: 'Opérateur',
        accessor: 'User',
        type: inputTypes.text.description,
    },
    {
        Header: 'Mode de paiement',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 50,
        accessor: (props) => {
            const discount = props.BonLivraisonItems.reduce((sum, curr) => {
                const total = curr.Pu * curr.Qte;
                if (curr.Discount) {
                    if (!curr.PercentageDiscount)
                        sum += Number(curr.Discount)
                    else
                        sum += total * parseFloat(curr.Discount) / 100;
                }
                return sum;
            }, 0);

            const total = props.BonLivraisonItems.reduce((sum, curr) => (
                sum += curr.Pu * curr.Qte
            ), 0);

            return formatMoney(total - discount);
        },
        align: 'right'
    },
    (isAdmin && {
        id: 'Marge',
        Header: 'Marge',
        type: inputTypes.text.description,
        width: 30,
        accessor: (props) => {
            const discount = props.BonLivraisonItems.reduce((sum, curr) => {
                const total = curr.Pu * curr.Qte;
                if (curr.Discount) {
                    if (!curr.PercentageDiscount)
                        sum += Number(curr.Discount)
                    else
                        sum += total * parseFloat(curr.Discount) / 100;
                }
                return sum;
            }, 0);
            const marge = props.BonLivraisonItems.reduce((sum, curr) => (
                sum += (curr.Pu - curr.PA) * curr.Qte
            ), 0);
            return formatMoney(marge - discount);
        },
        align: 'right'
    }),
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, print }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                    {canUpdateBonLivraisons && <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original.Id)}>
                        <EditOutlinedIcon />
                    </IconButton>}
                    {canDeleteBonLivraisons && <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>}
                </Box>
            )
        },
        width: 24
    },
].filter(x => x))