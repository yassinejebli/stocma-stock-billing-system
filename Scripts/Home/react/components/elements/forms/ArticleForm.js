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
import { getRandomInt } from '../../../utils/numberUtils';
import { useSettings } from '../../providers/SettingsProvider';
import ArticleCategoriesAutocomplete from '../article-categories-autocomplete/ArticleCategoriesAutocomplete';

const initialState = {
    Id: uuidv4(),
    Designation: '',
    PVD: '',
    PA: '',
    TVA: 20,
    Unite: 'U',
    QteStock: 0,
    Disabled: false,
    IsBarCodePrintable: true,
    MinStock: 1,
    Image: null,
    BarCode: '',
    Category: null,
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
        fontFamily: "'Libre Barcode 39'",
        fontSize: 30,
        height: 30,
        color: '#000',
    }
}))

const ArticleForm = ({ data, onSuccess }) => {
    const {
        barcodeModule,
        articleImageModule,
    } = useSettings();
    const { canUpdateQteStock } = useAuth();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const editMode = Boolean(data);
    const classes = useStyles();
    const [formState, setFormState] = React.useState({
        ...initialState,
        BarCode: getRandomInt(100000, 999999)+"",
    });
    const [formErrors, setFormErrors] = React.useState({});
    const [base64, setBase64] = React.useState(null);
    const [loading, setLoading] = React.useState(false);

    React.useEffect(() => {
        if (editMode) {
            const { Article, QteStock, Disabled } = data;
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
                IsBarCodePrintable: Article.IsBarCodePrintable,
                Image: Article.Image,
                MinStock: Article.MinStock,
                BarCode: Article.BarCode,
                Category: Article.Categorie,
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
        console.log(formState.PVD, formState.PA)
        if (formState.PVD && Number(formState.PVD) <= Number(formState.PA))
            _errors['PVD'] = 'Vérifier ce prix.'
        if (!formState.PVD)
            _errors['PVD'] = 'Ce champs est obligatoire.'
        if (!formState.PA)
            _errors['PA'] = 'Ce champs est obligatoire.'
        if (!formState.TVA)
            _errors['TVA'] = 'Ce champs est obligatoire.'
        if (formState.BarCode && !/^[a-z0-9]+$/i.test(formState.BarCode))
            _errors['BarCode'] = 'Le code-barres ne doit contenir que des caractères alphanumériques.'
        //

        setFormErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!isFormValid()) return;
        const { Disabled, Category, ...preparedData } = formState;

        setLoading(true);
        if (editMode) {
            const response = await updateArticle({ ...preparedData, Id: formState.Id, IdCategorie: Category?.Id || null }, formState.Id, formState.QteStock, siteId, Disabled);
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
            const response = await saveArticle({ ...preparedData, Id: uuidv4(), IdCategorie: Category?.Id || null }, formState.QteStock, siteId);
            if (response?.Id) {
                setFormState({ ...initialState, Id: uuidv4(), BarCode: getRandomInt(100000, 999999) + "" });
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
            <Box my={1}>
                <ArticleCategoriesAutocomplete
                    value={formState.Category}
                    errorText={formErrors.Category}
                    onChange={(_, value) => setFormState(_formState => ({ ...formState, Category: value }))}
                />
            </Box>
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
            {barcodeModule?.Enabled && <>
                <TextField
                    name="BarCode"
                    label="Code à barre"
                    variant="outlined"
                    size="small"
                    fullWidth
                    margin="normal"
                    inputProps={{
                        maxLength: 18,
                    }}
                    onChange={onFieldChange}
                    value={formState.BarCode}
                    helperText={formErrors.BarCode}
                    error={Boolean(formErrors.BarCode)}
                />
                {formState.BarCode && formState.Designation && <div className={classes.barcodeWrapper}>
                    <div className={classes.barcode}>
                        *{formState.BarCode}*
                </div>
                </div>}
            </>}
            {articleImageModule?.Enabled && <Box my={2}>
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
            </Box>}
            {barcodeModule?.Enabled&&<Box>
            <FormControlLabel
                control={<Switch
                    checked={formState.IsBarCodePrintable}
                    onChange={(_, checked) => setFormState(_formState => ({
                        ...formState,
                        IsBarCodePrintable: checked
                    })
                    )} />}
                label="Imprimer code-barres"
            />
            </Box>}
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