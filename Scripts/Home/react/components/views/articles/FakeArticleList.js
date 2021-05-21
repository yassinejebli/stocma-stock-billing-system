import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import TitleIcon from '../../elements/misc/TitleIcon'
import LocalMallOutlinedIcon from '@material-ui/icons/LocalMallOutlined'
import { TextField, Dialog, FormControlLabel, Checkbox, Button } from '@material-ui/core'
import useDebounce from '../../../hooks/useDebounce'
import { fakeArticleColumns } from '../../elements/table/columns/articleColumns'
import { getImageURL, getPrintArticleFactureReport } from '../../../utils/urlBuilder'
import {ArticlesFactureStatistics} from '../../elements/statistics/ArticlesStatistics'
import AddIcon from '@material-ui/icons/Add';
import FakeArticleForm from '../../elements/forms/FakeArticleForm'
import { useSettings } from '../../providers/SettingsProvider'
import PrintIcon from '@material-ui/icons/Print';

const TABLE = 'ArticleFactures';

const FakeArticleList = () => {
    const {
        articlesStatisticsModule,
    } = useSettings();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const [showDisabledArticles, setShowDisabledArticles] = React.useState(false);
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            Designation: {
                contains: debouncedSearchText
            },
            Disabled: !showDisabledArticles ? false : undefined
        }
    }, [debouncedSearchText, showDisabledArticles]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [selectedRow, setSelectedRow] = React.useState();
    const [pageCount, setTotalCount] = React.useState(0);
    const [selectedImage, setSelectedImage] = React.useState(null);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => fakeArticleColumns(),
        []
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
            <FakeArticleForm onSuccess={() => {
                refetchData();
                hideModal();
            }} data={selectedRow} />
        </SideDialogWrapper>
    ), [selectedRow]);
    const [showArticleModal, hideArticleModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideArticleModal}>
            <FakeArticleForm onSuccess={() => {
                refetchData();
            }} />
        </SideDialogWrapper>
    ), [filters]);
    const [showModalImage, hideModalImage] = useModal(({ in: open, onExited }) => {
        return (
            <Dialog
                onExited={onExited}
                open={open}
                maxWidth="md"
                onClose={() => {
                    setSelectedImage(null);
                    hideModalImage();
                }}
            >
                <img style={{
                    width: '100%',
                    height: 'auto',
                }} src={getImageURL(selectedImage)} />
            </Dialog>)
    }, [selectedImage]);


    React.useEffect(() => {
        setTitle('Articles')
    }, []);

    const refetchData = () => {
        getData(TABLE, {}, filters).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const deleteRow = React.useCallback(async (row) => {
        setLoading(true);
        const response = await deleteData(TABLE, row.Id);
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer l\'article sélectionné !'
            });
        }
        setLoading(false);
    }, []);

    const updateRow = React.useCallback(async (row) => {
        setSelectedRow(row);
        showModal();
    }, []);

    const showImage = React.useCallback(async (image) => {
        setSelectedImage(image);
        showModalImage();
    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
                $skip: startRow
            }, filters).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            });
        }
    }, [])

    return (
        <>
            <Loader loading={loading} />
            {articlesStatisticsModule?.Enabled&&<Box my={2} display="flex" justifyContent="center">
                <ArticlesFactureStatistics />
            </Box>}
            <Box mt={1} mb={2} display="flex" justifyContent="space-between">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<PrintIcon />}
                    onClick={()=>window.open(getPrintArticleFactureReport(),"_blank")}
                >
                    Imprimer
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={showArticleModal}
                >
                    Nouvel article
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des articles" Icon={LocalMallOutlinedIcon} />
                    <TextField
                        value={searchText}
                        onChange={({ target: { value } }) => {
                            setSearchText(value);
                        }}
                        placeholder="Rechercher..."
                        variant="outlined"
                        size="small"
                    />
                </Box>
                <Box mt={2}>
                    <FormControlLabel
                        control={<Checkbox checked={showDisabledArticles} color="primary" onChange={event => setShowDisabledArticles(event.target.checked)} />}
                        label="Afficher les articles désactivés"
                    />
                </Box>
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                        deleteRow={deleteRow}
                        updateRow={updateRow}
                        showImage={showImage}
                        serverPagination
                        totalItems={totalItems}
                        pageCount={pageCount}
                        fetchData={fetchData}
                        filters={filters}
                    />
                </Box>
            </Paper>
        </>
    )
}

export default FakeArticleList;
