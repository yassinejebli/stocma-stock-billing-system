import Box from '@material-ui/core/Box'
import React from 'react'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { getSuiviAchatsColumns } from '../../../elements/table/columns/suiviAchatsColumns'
import Paper from '../../../elements/misc/Paper'
import TitleIcon from '../../../elements/misc/TitleIcon'
import { useTitle } from '../../../providers/TitleProvider';
import { getData } from '../../../../queries/crudBuilder';
import useDebounce from '../../../../hooks/useDebounce';
import Table from '../../../elements/table/Table';
import PrintBR from '../../../elements/dialogs/documents-print/PrintBR';
import { useModal } from 'react-modal-hook';
import DatePicker from '../../../elements/date-picker/DatePicker';

const TABLE = 'BonReceptionItems';

const EXPAND = ['Article', 'BonReception/Fournisseur($select=Id,Name)'];

const SuiviAchats = ({ fournisseur, article }) => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear() - 1, today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const { setTitle } = useTitle();
    const [fournisseurSearchText, setFournisseurSearchText] = React.useState(fournisseur?.Name || '');
    const [articleSearchText, setArticleSearchText] = React.useState(article?.Designation || '');
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const debouncedFournisseurSearchText = useDebounce(fournisseurSearchText);
    const debouncedArticleSearchText = useDebounce(articleSearchText);
    const filters = React.useMemo(() => {
        return {
            'BonReception/Date': { ge: dateFrom, le: dateTo },
            and: [
                {
                    'BonReception/Fournisseur/Name': {
                        contains: debouncedFournisseurSearchText
                    }
                },
                {
                    'Article/Designation': {
                        contains: debouncedArticleSearchText
                    }
                },
            ]
        }
    }, [debouncedFournisseurSearchText, debouncedArticleSearchText, dateFrom, dateTo]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getSuiviAchatsColumns(),
        []
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBR
                onExited={onExited}
                open={open}
                document={documentToPrint}
                onClose={() => {
                    setDocumentToPrint(null);
                    hideModal();
                }}
            />
        )
    }, [documentToPrint]);

    React.useEffect(() => {
        setTitle('Suivi des achats')
    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
                $skip: startRow
            }, filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            });
        }
    }, [])

    const print = React.useCallback((document) => {
        setDocumentToPrint(document);
        showModal();
    }, [])

    return (
        <>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Suivi des achats" Icon={DescriptionOutlinedIcon} />
                </Box>
                <Box mt={3}>
                    <DatePicker
                        value={dateFrom}
                        label="Date de début"
                        onChange={(date) => {
                            date && date.setHours(0, 0, 0, 0);
                            setDateFrom(date)
                        }}
                    />
                    <DatePicker
                        style={{
                            marginLeft: 12
                        }}
                        value={dateTo}
                        label="Date de fin"
                        onChange={(date) => {
                            date && date.setHours(23, 59, 59, 999);
                            setDateTo(date)
                        }}
                    />
                </Box>
                <Box mt={2} display="flex">
                    <Box width={240}>
                        <TextField
                            fullWidth
                            value={fournisseurSearchText}
                            onChange={({ target: { value } }) => {
                                setFournisseurSearchText(value);
                            }}
                            placeholder="Choisir un fournisseur..."
                            variant="outlined"
                            size="small"
                        />
                    </Box>
                    <Box ml={1.5} width={240}>
                        <TextField
                            fullWidth
                            value={articleSearchText}
                            onChange={({ target: { value } }) => {
                                setArticleSearchText(value);
                            }}
                            placeholder="Choisir un article..."
                            variant="outlined"
                            size="small"
                        />
                    </Box>
                </Box>
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                        serverPagination
                        print={print}
                        totalItems={totalItems}
                        pageCount={pageCount}
                        fetchData={fetchData}
                        filters={filters}
                    />
                </Box>
                <Box mt={2}>
                    Total Qte: {data.reduce((sum, curr) => (
                    sum += curr.Qte
                ), 0)}
                </Box>
            </Paper>
        </>
    )
}

export default SuiviAchats;
