import Box from "@material-ui/core/Box";
import React from "react";
import DescriptionOutlinedIcon from "@material-ui/icons/DescriptionOutlined";
import { TextField, Button } from "@material-ui/core";
import { useModal } from "react-modal-hook";
import Paper from "../../elements/misc/Paper";
import { useTitle } from "../../providers/TitleProvider";
import useDebounce from "../../../hooks/useDebounce";
import TitleIcon from "../../elements/misc/TitleIcon";
import { getPaiementClientListColumns } from "../../elements/table/columns/paiementClientFactureColumns";
import DatePicker from "../../elements/date-picker/DatePicker";
import { MicrosoftExcel } from "mdi-material-ui";
import Table from "../../elements/table/Table";
import { getData, deleteData, saveData } from "../../../queries/crudBuilder";
import ClientAutocomplete from "../../elements/client-autocomplete/ClientAutocomplete";
import PaiementClientForm from "../../elements/forms/PaiementClientForm";
import { partialUpdateData } from "../../../queries/crudBuilder";
import { SideDialogWrapper } from "../../elements/dialogs/SideWrapperDialog";
import PrintIcon from "@material-ui/icons/Print";
import AddIcon from "@material-ui/icons/Add";
import { useSnackBar } from "../../providers/SnackBarProvider";
import PrintClientAccountSummary from "../../elements/dialogs/documents-print/PrintClientAccountSummary";
import SoldeText from "../../elements/texts/SoldeText";
import { useLoader } from "../../providers/LoaderProvider";
import {
  getExportClientAccountSummaryURL,
  getPrintSituationGlobaleClientsURL,
} from "../../../utils/urlBuilder";
import { useAuth } from "../../providers/AuthProvider";
import ListAltIcon from "@material-ui/icons/ListAlt";
import IframeDialog from "../../elements/dialogs/IframeDialog";
import PrintFacture from "../../elements/dialogs/documents-print/PrintFacture";
import PaiementFactureClientForm from "../../elements/forms/PaiementFactureClientForm";
const TABLE = "PaiementFactures";

const EXPAND = [
  "TypePaiement",
  "Client($select=Id,Name)",
  "Facture/Client($select=Id,Name)",
  "BonLivraisons/BonLivraisonItems",
];

const PaiementClientFactureList = () => {
  const refreshCount = React.useRef(0);
  const { showLoader } = useLoader();
  const today = new Date();
  const firstDayCurrentMonth = new Date(
    today.getFullYear(),
    today.getMonth(),
    1
  );
  const lastDayCurrentMonth = new Date();
  firstDayCurrentMonth.setHours(0, 0, 0, 0);
  lastDayCurrentMonth.setHours(23, 59, 59, 999);
  const { setTitle } = useTitle();
  const [client, setClient] = React.useState(null);
  const [searchText, setSearchText] = React.useState("");
  const [selectedRow, setSelectedRow] = React.useState(null);
  const [summaryClientIdToPrint, setSummaryClientIdToPrint] =
    React.useState(null);
  const [documentToPrint, setDocumentToPrint] = React.useState(null);
  const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
  const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
  const debouncedSearchText = useDebounce(searchText);
  const [errors, setErrors] = React.useState({});
  const { showSnackBar } = useSnackBar();
  const filters = React.useMemo(() => {
    return {
      Date: { ge: dateFrom, le: dateTo },
      IdClient: client ? { eq: { type: "guid", value: client.Id } } : undefined,
      or: [
        {
          Comment: {
            contains: debouncedSearchText,
          },
        },
        {
          "TypePaiement/Name": {
            contains: debouncedSearchText,
          },
        },
        {
          "Facture/NumBon": {
            contains: debouncedSearchText,
          },
        },
        {
          Credit: !isNaN(debouncedSearchText)
            ? Number(debouncedSearchText)
            : undefined,
        },
      ],
    };
  }, [debouncedSearchText, client, dateFrom, dateTo]);
  const [showBLModal, hideBLModal] = useModal(
    ({ in: open, onExited }) => {
      return (
        <PrintFacture
          onExited={onExited}
          open={open}
          document={documentToPrint}
          onClose={() => {
            setDocumentToPrint(null);
            hideBLModal();
          }}
        />
      );
    },
    [documentToPrint]
  );
  const [showAccountSummaryModal, hideAccountSummaryModal] = useModal(
    ({ in: open, onExited }) => {
      return (
        <PrintClientAccountSummary
          onExited={onExited}
          open={open}
          clientId={summaryClientIdToPrint}
          dateFrom={dateFrom}
          dateTo={dateTo}
          onClose={() => {
            setSummaryClientIdToPrint(null);
            hideAccountSummaryModal();
          }}
        />
      );
    },
    [summaryClientIdToPrint, dateFrom, dateTo]
  );
  const [showNewPaiementModal, hideNewPaiementModal] = useModal(
    ({ in: open, onExited }) => (
      <SideDialogWrapper
        open={open}
        onExited={onExited}
        onClose={hideNewPaiementModal}
      >
        <PaiementFactureClientForm
          onSuccess={() => {
            refetchData();
            // hideNewPaiementModal();
          }}
        />
      </SideDialogWrapper>
    ),
    [filters, client]
  );
  const [showPaiementModal, hidePaiementModal] = useModal(
    ({ in: open, onExited }) => (
      <SideDialogWrapper
        open={open}
        onExited={onExited}
        onClose={hidePaiementModal}
      >
        {selectedRow && (
          <PaiementFactureClientForm
            paiement={selectedRow}
            onSuccess={() => {
              refetchData();
              hidePaiementModal();
            }}
          />
        )}
      </SideDialogWrapper>
    ),
    [selectedRow]
  );
  const [data, setData] = React.useState([]);
  const [totalItems, setTotalItems] = React.useState(0);
  const [pageCount, setTotalCount] = React.useState(0);
  const fetchIdRef = React.useRef(0);
  const columns = React.useMemo(
    () => getPaiementClientListColumns({ isFiltered: Boolean(!client) }),
    [client]
  );
  const [showPrintSituationGlobale, hidePrintSituationGlobale] = useModal(
    ({ in: open, onExited }) => {
      return (
        <IframeDialog
          onExited={onExited}
          open={open}
          onClose={hidePrintSituationGlobale}
          src={getPrintSituationGlobaleClientsURL()}
        ></IframeDialog>
      );
    },
    []
  );

  React.useEffect(() => {
    setTitle("Liste des paiements");
  }, []);

  const refetchData = React.useCallback(() => {
    console.log({ filters });
    showLoader(true, true);
    getData(TABLE, {}, filters, EXPAND)
      .then((response) => {
        setData(response.data);
        setTotalItems(response.totalItems);
      })
      .catch((err) => {
        console.log({ err });
      })
      .finally(() => {
        refreshCount.current += 1;
        showLoader();
      });
  }, [filters]);

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

  const print = React.useCallback((document) => {
    setDocumentToPrint(document);
    showBLModal();
  }, []);

  const disableRow = React.useCallback(
    async (document) => {
      const response = await partialUpdateData(
        TABLE,
        {
          Hide: !document.Hide,
        },
        document.Id
      );
      if (response.ok) showSnackBar();
      else
        showSnackBar({
          error: true,
          text: "Erreur !",
        });
      refetchData();
    },
    [filters]
  );

  const updateRow = React.useCallback(async (row) => {
    setSelectedRow(row);
    showPaiementModal();
  }, []);

  const printAccountSummary = () => {
    setSummaryClientIdToPrint(client?.Id);
    showAccountSummaryModal();
  };

  //Impaye
  const customAction = React.useCallback(async (row) => {
    const preparedData = {
      IdTypePaiement: "399d159e-9ce0-4fcc-957a-08a65bbeece1",
      IdClient: row.IdClient,
      Debit: row.Credit,
      Date: new Date(),
      DateEcheance: row.DateEcheance,
      Comment: row.Comment,
    };
    const response = await saveData(TABLE, preparedData);
    if (response?.Id) showSnackBar();
    else
      showSnackBar({
        error: true,
        text: "Erreur !",
      });
    refetchData();
  }, []);

  const editDocument = React.useCallback((id) => {
    window.open(`/Administration#/Facture?FactureId=${id}`, "_blank");
  }, []);

  const deleteRow = React.useCallback(
    async (id) => {
      const response = await deleteData(TABLE, id);
      if (response.ok) {
        showSnackBar();
      } else {
        showSnackBar({
          error: true,
          text: "Impossible de supprimer la ligne sélectionnée !",
        });
      }
      refetchData();
    },
    [filters]
  );

  return (
    <>
      <Box mt={1} mb={2} display="flex" justifyContent="space-between">
        {client && dateFrom && dateTo && (
          <Box display="flex">
            <Button
              variant="contained"
              color="primary"
              startIcon={<PrintIcon />}
              onClick={printAccountSummary}
            >
              Imprimer la situation
            </Button>
            {/* <Box ml={2}>
                        <Button
                            style={{
                                backgroundColor: '#026f39'
                            }}
                            variant="contained"
                            color="primary"
                            startIcon={<MicrosoftExcel />}
                            href={getExportClientAccountSummaryURL({
                                id: client.Id,
                                dateFrom: dateFrom.toISOString(),
                                dateTo: dateTo.toISOString(),
                            })}
                        >
                            Exporter Excel
                        </Button>
                    </Box> */}
          </Box>
        )}
        <Box ml="auto">
          {/* <Button
                    variant="contained"
                    color="secondary"
                    style={{
                        marginRight: 8
                    }}
                    startIcon={<ListAltIcon />}
                    onClick={showPrintSituationGlobale}
                >
                    Situation Globale
                </Button> */}
          <Button
            variant="contained"
            color="primary"
            startIcon={<AddIcon />}
            onClick={showNewPaiementModal}
          >
            Nouveau paiement
          </Button>
        </Box>
      </Box>
      <Paper>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <TitleIcon
            noBorder
            title="Liste des paiements"
            Icon={DescriptionOutlinedIcon}
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
        <Box width={240} mt={3}>
          <ClientAutocomplete
            disableClearable={false}
            value={client}
            onChange={(_, value) => setClient(value)}
            errorText={errors.client}
          />
        </Box>
        <Box mt={3}>
          <DatePicker
            value={dateFrom}
            label="Date de début"
            onChange={(date) => {
              date && date.setHours(0, 0, 0, 0);
              setDateFrom(date);
            }}
          />
          <DatePicker
            style={{
              marginLeft: 12,
            }}
            value={dateTo}
            label="Date de fin"
            onChange={(date) => {
              date && date.setHours(23, 59, 59, 999);
              setDateTo(date);
            }}
          />
        </Box>
        <Box mt={4}>
          <Table
            columns={columns}
            data={data}
            serverPagination
            updateRow={updateRow}
            updateRow2={editDocument}
            deleteRow={deleteRow}
            disableRow={disableRow}
            customAction={customAction}
            print={print}
            totalItems={totalItems}
            pageCount={pageCount}
            fetchData={fetchData}
            filters={filters}
          />
          {client && (
            <Box mt={2}>
              <SoldeText
                refresh={refreshCount.current}
                clientId={client.Id}
                date={dateFrom}
              />
            </Box>
          )}
        </Box>
      </Paper>
    </>
  );
};

export default PaiementClientFactureList;
