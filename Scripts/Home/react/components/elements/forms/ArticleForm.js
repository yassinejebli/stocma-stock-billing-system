import React from 'react';
import { TextField, Button, Box, Avatar, FormControlLabel, Switch, makeStyles, IconButton } from '@material-ui/core';
import { v4 as uuidv4 } from 'uuid'
import Loader from '../loaders/Loader';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import { saveArticle, updateArticle } from '../../../queries/articleQueries';
import { useSite } from '../../providers/SiteProvider';
import { useSnackBar } from '../../providers/SnackBarProvider';
import TitleIcon from '../misc/TitleIcon';
import FilePicker from '../button/FilePicker';
import { toBase64 } from '../../../utils/imageUtils';
import CancelIcon from '@material-ui/icons/Cancel';
import { uploadArticleImage } from '../../../queries/fileUploader';
import { getImageURL } from '../../../utils/urlBuilder';
import { useAuth } from '../../providers/AuthProvider';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import PrintCodeBarreEtiquette from '../dialogs/PrintCodeBarreEtiquette';
import { useModal } from 'react-modal-hook';
import { getRandomInt } from '../../../utils/numberUtils';

const initialState = {
    Id: uuidv4(),
    Designation: '',
    PVD: '',
    PA: '',
    TVA: 20,
    Unite: 'U',
    QteStock: 0,
    Disabled: false,
    MinStock: 1,
    Image: null,
    BarCode: "A" + getRandomInt(1000, 100000),
}

const useStyles = makeStyles(theme => ({
    image: {
        width: 80,
        height: 80,
        backgroundSize: 'contain',
        backgroundPosition: 'center',
        position: 'relative'
    },
    removeIcon: {
        position: 'absolute',
        top: -10,
        right: -4,
        zIndex: 2,
        height: 16,
        width: 16,
        cursor: 'pointer'
    },
    barcodeWrapper: {
        display: 'flex',
        alignItems: 'center'
    },
    barcode: {
        fontFamily: "'Libre Barcode 128'",
        fontSize: 30,
        height: 30,
        color: '#000',
    }
}))

const ArticleForm = ({ data, onSuccess }) => {
    const { canUpdateQteStock } = useAuth();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const classes = useStyles();
    const [formState, setFormState] = React.useState(initialState);
    const [formErrors, setFormErrors] = React.useState({});
    const [base64, setBase64] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [showPrintBarcodeLabelModal, hidePrintBarcodeLabelModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintCodeBarreEtiquette
                onExited={onExited}
                open={open}
                designation={formState.Designation}
                barCode={formState.BarCode}
                onClose={() => {
                    hidePrintBarcodeLabelModal(null);
                }}
            />
        )
    }, [formState.Designation, formState.BarCode]);

    React.useEffect(() => {
        console.log({ data })
        if (editMode) {
            const { Article, QteStock, Disabled } = data;
            console.log('Article.MinStock', Article.MinStock)
            setFormState({
                Id: Article.Id,
                QteStock,
                Designation: Article.Designation,
                Ref: Article.Ref,
                PVD: Article.PVD,
                PA: Article.PA,
                TVA: Article.TVA,
                Unite: Article.Unite,
                Disabled: Disabled,
                Image: Article.Image,
                MinStock: Article.MinStock,
                BarCode: Article.BarCode,
            });
        }
    }, [])

    const onFieldChange = ({ target }) => setFormState(_formState => ({ ..._formState, [target.name]: target.value }));

    const isFormValid = () => {
        const _errors = [];
        if (!formState.Designation)
            _errors['Designation'] = 'Ce champs est obligatoire.'
        if (!formState.Ref)
            _errors['Ref'] = 'Ce champs est obligatoire.'
        if (!formState.PVD)
            _errors['PVD'] = 'Ce champs est obligatoire.'
        if (!formState.PA)
            _errors['PA'] = 'Ce champs est obligatoire.'
        if (!formState.TVA)
            _errors['TVA'] = 'Ce champs est obligatoire.'

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;
        const { Disabled, ...preparedData } = formState;

        setLoading(true);
        if (editMode) {
            const response = await updateArticle({ ...preparedData, Id: formState.Id }, formState.Id, formState.QteStock, siteId, Disabled);
            if (response?.ok) {
                setFormState({ ...initialState });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: "Vous n'êtes pas autorisé à modifier l'article!"
                });
            }
        } else {
            const response = await saveArticle({ ...preparedData, Id: uuidv4() }, formState.QteStock, siteId);
            if (response?.Id) {
                setFormState({ ...initialState, Id: uuidv4(), BarCode: "A"+getRandomInt(1000, 100000) });
                showSnackBar();
                if (onSuccess) onSuccess();
            } else {
                showSnackBar({
                    error: true,
                    text: "Erreur!",
                });
            }
        }
        setLoading(false);
    }

    return (
        <div>
            <Loader loading={loading} />
            <TitleIcon title={editMode ? 'Modifier l\'article' : 'Ajouter un article'} Icon={AddShoppingCartIcon} />
            <TextField
                name="Ref"
                label="Référence/Code"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Ref}
                helperText={formErrors.Ref}
                error={Boolean(formErrors.Ref)}

            />
            <TextField
                name="Designation"
                label="Désignation"
                variant="outlined"
                size="small"
                fullWidth
                multiline
                margin="normal"
                rows={2}
                onChange={onFieldChange}
                value={formState.Designation}
                helperText={formErrors.Designation}
                error={Boolean(formErrors.Designation)}

            />
            <TextField
                name="QteStock"
                label="Quantité en stock"
                variant="outlined"
                size="small"
                fullWidth
                disabled={!canUpdateQteStock && editMode}
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.QteStock}
                helperText={formErrors.QteStock}
                error={Boolean(formErrors.QteStock)}

            />
            <TextField
                name="MinStock"
                label="Qte minimum en stock"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.MinStock}

            />
            <TextField
                name="PA"
                label="Prix d'achat"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.PA}
                helperText={formErrors.PA}
                error={Boolean(formErrors.PA)}
            />
            <TextField
                name="PVD"
                label="Prix de vente"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.PVD}
                helperText={formErrors.PVD}
                error={Boolean(formErrors.PVD)}
            />
            <TextField
                name="TVA"
                label="T.V.A applicable (%)"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                type="number"
                onChange={onFieldChange}
                value={formState.TVA}
                helperText={formErrors.TVA}
                error={Boolean(formErrors.TVA)}

            />
            <TextField
                name="Unite"
                label="Unité"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                onChange={onFieldChange}
                value={formState.Unite}
                helperText={formErrors.Unite}
                error={Boolean(formErrors.Unite)}
            />
            <TextField
                name="BarCode"
                label="Code à barre"
                variant="outlined"
                size="small"
                fullWidth
                margin="normal"
                inputProps={{ maxLength: 14 }}
                onChange={onFieldChange}
                value={formState.BarCode}
                helperText={formErrors.BarCode}
                error={Boolean(formErrors.BarCode)}
            />
            {formState.BarCode && formState.Designation && <div className={classes.barcodeWrapper}>
                <div className={classes.barcode}>
                    {formState.BarCode}
                </div>
                <Box ml={0.5}>
                    <IconButton onClick={showPrintBarcodeLabelModal}>
                        <PrintOutlinedIcon />
                    </IconButton>
                </Box>
            </div>}
            <Box my={2}>
                <Box my={2}>
                    {formState.Image && <div className={classes.image} style={{
                        backgroundImage: `url(${getImageURL(formState.Image)})`
                    }}>
                        <div className={classes.removeIcon}
                            onClick={() => {
                                setFormState(_formState => ({ ..._formState, Image: null }));
                                setBase64(null);
                            }}>
                            <CancelIcon color="primary" />
                        </div>
                    </div>}
                </Box>
                <FilePicker
                    onChange={async ({ target }) => {
                        const file = target.files?.[0];
                        if (file) {
                            const res = uploadArticleImage(formState.Id, file);
                            console.log({ res });
                            const _base64 = await toBase64(target.files?.[0]);
                            setBase64(_base64);
                            setFormState(_formState => ({
                                ..._formState, Image: formState.Id + '.' + file.name?.split('.').pop()
                            }));
                        }
                    }}
                />
            </Box>
            <FormControlLabel
                control={<Switch
                    checked={!formState.Disabled}
                    onChange={(_, checked) => setFormState(_formState => ({
                        ...formState,
                        Disabled: !checked
                    })
                    )} />}
                label="Actif"
            />
            <Box my={4} display="flex" justifyContent="flex-end">
                <Button variant="contained" color="primary" onClick={save}>
                    Enregistrer
                </Button>
            </Box>
        </div>
    )
}

export default ArticleForm