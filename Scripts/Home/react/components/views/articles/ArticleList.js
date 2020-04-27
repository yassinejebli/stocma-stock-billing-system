import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData } from '../../../queries/crudBuilder'
import { deleteArticle } from '../../../queries/articleQueries'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import ArticleForm from '../../elements/forms/ArticleForm'
import TitleIcon from '../../elements/misc/TitleIcon'
import LocalMallOutlinedIcon from '@material-ui/icons/LocalMallOutlined'
import { TextField, Dialog, FormControlLabel, Checkbox } from '@material-ui/core'
import useDebounce from '../../../hooks/useDebounce'
import { articleColumns } from '../../elements/table/columns/articleColumns'
import { getImageURL } from '../../../utils/urlBuilder'
import ArticlesStatistics from '../../elements/statistics/ArticlesStatistics'
import { useSite } from '../../providers/SiteProvider'

const TABLE = 'ArticleSites';
const EXPAND = ['Article'];

const ArticleList = () => {
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const [showDisabledArticles, setShowDisabledArticles] = React.useState(false);
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            IdSite: siteId,
            or: {
                'Article/Designation': {
                    contains: debouncedSearchText
                }
            },
            Disabled: !showDisabledArticles ? false : undefined
        }
    }, [debouncedSearchText, showDisabledArticles, siteId]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [selectedRow, setSelectedRow] = React.useState();
    const [pageCount, setTotalCount] = React.useState(0);
    const [selectedImage, setSelectedImage] = React.useState(null);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => articleColumns(),
        []
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
            <ArticleForm onSuccess={() => {
                refetchData();
                hideModal();
            }} data={selectedRow} />
        </SideDialogWrapper>
    ), [selectedRow]);
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

    React.useEffect(() => {
        refetchData();
    }, [siteId])

    const refetchData = () => {
        getData(TABLE, {}, filters, EXPAND).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const deleteRow = React.useCallback(async (row) => {
        setLoading(true);
        const response = await deleteArticle(row.IdSite, row.IdArticle);
        console.log({ response });
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
            }, filters, EXPAND).then((response) => {
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
            <Box my={2} display="flex" justifyContent="center">
                <ArticlesStatistics />
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
                        control={<Checkbox checked={showDisabledArticles} color="primary" onChange={event=>setShowDisabledArticles(event.target.checked)} />}
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

export default ArticleList;
