import Box from "@material-ui/core/Box";
import React from "react";
import Paper from "../../elements/misc/Paper";
import Table from "../../elements/table/Table";
import { useTitle } from "../../providers/TitleProvider";
import { getData } from "../../../queries/crudBuilder";
import { deleteArticle } from "../../../queries/articleQueries";
import { useSnackBar } from "../../providers/SnackBarProvider";
import { useModal } from "react-modal-hook";
import { SideDialogWrapper } from "../../elements/dialogs/SideWrapperDialog";
import { MicrosoftExcel } from "mdi-material-ui";
import ArticleForm from "../../elements/forms/ArticleForm";
import TitleIcon from "../../elements/misc/TitleIcon";
import LocalMallOutlinedIcon from "@material-ui/icons/LocalMallOutlined";
import {
  TextField,
  Dialog,
  FormControlLabel,
  Checkbox,
  Button,
} from "@material-ui/core";
import useDebounce from "../../../hooks/useDebounce";
import PrintIcon from "@material-ui/icons/Print";
import { articleColumns } from "../../elements/table/columns/articleColumns";
import {
  getImageURL,
  getPrintInventaireTousLesArticlesURL,
  getExportStockURL,
} from "../../../utils/urlBuilder";
import ArticlesStatistics from "../../elements/statistics/ArticlesStatistics";
import { useSite } from "../../providers/SiteProvider";
import LocalAtmIcon from "@material-ui/icons/LocalAtm";
import { useHistory } from "react-router-dom";
import AddIcon from "@material-ui/icons/Add";
import { useAuth } from "../../providers/AuthProvider";
import { useLoader } from "../../providers/LoaderProvider";
import { useSettings } from "../../providers/SettingsProvider";
import TrendingDownIcon from "@material-ui/icons/TrendingDown";

const TABLE = "ArticleSites";
const EXPAND = ["Article/Categorie"];

const ArticleList = () => {
  const {
    articleMarginModule,
    articleImageModule,
    articlesStatisticsModule,
    articlesNotSellingModule,
  } = useSettings();
  const { showLoader } = useLoader();
  const { isAdmin } = useAuth();
  const { siteId } = useSite();
  const { showSnackBar } = useSnackBar();
  const { setTitle } = useTitle();
  const [lowStockSelected, setLowStockSelected] = React.useState(false);
  const [searchText, setSearchText] = React.useState("");
  const [showDisabledArticles, setShowDisabledArticles] = React.useState(false);
  const debouncedSearchText = useDebounce(searchText);
  const history = useHistory();
  const filters = React.useMemo(() => {
    return {
      and: [
        { IdSite: siteId },
        {
          QteStock: lowStockSelected
            ? {
                le: {
                  type: "raw",
                  value: "Article/MinStock",
                },
              }
            : undefined,
        },
        { Disabled: !showDisabledArticles ? false : undefined },
        {
          or: [
            {
              and: debouncedSearchText.split(" ").map((word) => ({
                "Article/Designation": {
                  contains: word,
                },
              })),
            },
            {
              "Article/Ref": {
                contains: debouncedSearchText,
              },
            },
            {
              "Article/BarCode": {
                contains: debouncedSearchText,
              },
            },
          ],
        },
      ],
    };
  }, [debouncedSearchText, showDisabledArticles, siteId, lowStockSelected]);

  console.log({ filters });
  const [data, setData] = React.useState([]);
  const [totalItems, setTotalItems] = React.useState(0);
  const [selectedRow, setSelectedRow] = React.useState();
  const [pageCount, setTotalCount] = React.useState(0);
  const [selectedImage, setSelectedImage] = React.useState(null);
  const fetchIdRef = React.useRef(0);
  const columns = React.useMemo(
    () => articleColumns({ articleImageModule }),
    []
  );
  const [showModal, hideModal] = useModal(
    ({ in: open, onExited }) => (
      <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
        <ArticleForm
          onSuccess={() => {
            refetchData();
            hideModal();
          }}
          data={selectedRow}
        />
      </SideDialogWrapper>
    ),
    [selectedRow]
  );
  const [showArticleModal, hideArticleModal] = useModal(
    ({ in: open, onExited }) => (
      <SideDialogWrapper
        open={open}
        onExited={onExited}
        onClose={hideArticleModal}
      >
        <ArticleForm
          onSuccess={() => {
            refetchData();
          }}
        />
      </SideDialogWrapper>
    ),
    [filters]
  );
  const [showModalImage, hideModalImage] = useModal(
    ({ in: open, onExited }) => {
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
          <img
            style={{
              width: "100%",
              height: "auto",
            }}
            src={getImageURL(selectedImage)}
          />
        </Dialog>
      );
    },
    [selectedImage]
  );

  React.useEffect(() => {
    setTitle("Articles");
  }, []);

  React.useEffect(() => {
    refetchData();
  }, [siteId]);

  const refetchData = () => {
    showLoader(true);
    getData(TABLE, {}, filters, EXPAND)
      .then((response) => {
        setData(response.data);
        setTotalItems(response.totalItems);
      })
      .catch((err) => {
        console.log({ err });
      })
      .finally(() => {
        showLoader();
      });
  };

  const deleteRow = React.useCallback(async (row) => {
    const response = await deleteArticle(row.IdSite, row.IdArticle);
    console.log({ response });
    if (response.ok) {
      showSnackBar();
      refetchData();
    } else {
      showSnackBar({
        error: true,
        text: "Impossible de supprimer l'article sélectionné !",
      });
    }
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
      showLoader(true, true);
      getData(
        TABLE,
        {
          $skip: startRow,
        },
        filters,
        EXPAND
      )
        .then((response) => {
          setData(response.data);
          setTotalItems(response.totalItems);
          setTotalCount(Math.ceil(response.totalItems / pageSize));
        })
        .catch((err) => {
          console.log({ err });
        })
        .finally(() => {
          showLoader();
        });
    }
  }, []);

  return (
    <>
      {articlesStatisticsModule?.Enabled && (
        <Box my={2} display="flex" justifyContent="center">
          <ArticlesStatistics
            lowStockSelected={lowStockSelected}
            onLowStockCountClick={() => {
              setLowStockSelected((_lowStockSelected) => !_lowStockSelected);
            }}
          />
        </Box>
      )}
      <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
        {articlesNotSellingModule?.Enabled && (
          <Box mr={2}>
            <Button
              variant="contained"
              color="secondary"
              startIcon={<TrendingDownIcon />}
              onClick={() => history.push("/articles-non-vendus")}
            >
              Les articles non-vendus
            </Button>
          </Box>
        )}
        {isAdmin && articleMarginModule?.Enabled && (
          <Box mr={2}>
            <Button
              variant="contained"
              color="primary"
              startIcon={<LocalAtmIcon />}
              onClick={() => history.push("/marge-articles")}
            >
              Marge bénéficiaire par article
            </Button>
          </Box>
        )}
        <Box mr={2}>
          <Button
            variant="contained"
            color="primary"
            startIcon={<PrintIcon />}
            onClick={() =>
              window.open(
                getPrintInventaireTousLesArticlesURL({ idSite: siteId }),
                "_blank"
              )
            }
          >
            Impression
          </Button>
        </Box>
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
          <TitleIcon
            noBorder
            title="Liste des articles"
            Icon={LocalMallOutlinedIcon}
          />
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
            control={
              <Checkbox
                checked={showDisabledArticles}
                color="primary"
                onChange={(event) =>
                  setShowDisabledArticles(event.target.checked)
                }
              />
            }
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
        <Box mt={2}>
          <Button
            style={{
              backgroundColor: "#026f39",
            }}
            variant="contained"
            color="primary"
            startIcon={<MicrosoftExcel />}
            href={getExportStockURL({
              idSite: siteId,
            })}
          >
            Exporter Excel
          </Button>
        </Box>
      </Paper>
    </>
  );
};

export default ArticleList;
