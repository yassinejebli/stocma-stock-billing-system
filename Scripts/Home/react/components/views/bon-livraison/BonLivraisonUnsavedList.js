import Box from '@material-ui/core/Box'
import React from 'react'
import Table from '../../elements/table/Table'
import { Button, IconButton, Tooltip } from '@material-ui/core'
import { formatMoney } from '../../../utils/moneyUtils'
import { inputTypes } from '../../../types/input'
import { format } from 'date-fns';
import CheckIcon from '@material-ui/icons/Check';
import DeleteForeverIcon from '@material-ui/icons/DeleteForever';

export const AUTO_SAVED_BL = "auto-saved-bl";
const BonLivraisonUnsavedList = ({onImport}) => {
    const [data, setData] = React.useState([]);
    const columns = React.useMemo(
        () => bonLivraisonUnsavedListColumns(),
        []
    );

    React.useEffect(() => {
        const autoSavedBonLivraisons = localStorage.getItem(AUTO_SAVED_BL);
        if (autoSavedBonLivraisons) {
            const savedDocumentsStorage = JSON.parse(autoSavedBonLivraisons);
            setData(savedDocumentsStorage?.sort((a,b)=>b.date-a.date))
        }
    }, [])

    return (
        <Box p={4}>
            <Box display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="secondary"
                    startIcon={<DeleteForeverIcon />}
                    onClick={() => {
                        localStorage.removeItem(AUTO_SAVED_BL);
                        setData([]);
                    }}
                >
                    Supprimer tous
                </Button>
            </Box>
            <Box mt={2}>
                <Table
                    columns={columns}
                    data={data}
                    customAction={onImport}
                />
            </Box>
        </Box>
    )
}



export const bonLivraisonUnsavedListColumns = () => ([
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.date), 'dd/MM/yyyy HH:mm')
        },
        width: 100
    },
    {
        Header: 'Client',
        accessor: 'client.Name',
        type: inputTypes.text.description,
        width: 180,
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 50,
        accessor: (props) => {
            const total = props.data.reduce((sum, curr) => (
                sum += curr.Pu * curr.Qte
            ), 0);
            return formatMoney(total);
        },
        align: 'right'
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                        <Tooltip title="Récupérer ce document">
                            <CheckIcon color="primary" />
                        </Tooltip>
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
])

export default BonLivraisonUnsavedList;
