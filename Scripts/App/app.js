var app = angular.module('AdminModule', ['ui.bootstrap', 'ngAnimate', 'ui.grid.moveColumns', 'ui.grid.rowEdit',
    'ui.grid.autoResize', 'ui.grid.resizeColumns', 'ui.grid.exporter', 'ngResource',
    'ngTouch', 'ui.grid', 'ui.grid.edit', 'ui.grid.pagination', 'ui.grid.cellNav'
    , 'ui.grid.selection', 'angularFileUpload']);
var checkboxCellTemplate = '<div class="ngSelectionCell"><input class="ngSelectionCheckbox" type="checkbox" ng-checked="{{COL_FIELD}}==true" disabled /></div>';
var checkboxCellEditTemplate = '<div class="checkbox" style=" margin:0px !important ;padding:0px!important;"><label style="display:block !important"> <input class="ngSelectionCheckbox"   type="checkbox" ng-model="COL_FIELD" ng-input="COL_FIELD"  /></label></div>';
checkboxCellTemplate = checkboxCellEditTemplate;

app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});
app.factory('myService', function ($http) {

    var getData = function (IdClient,IdArticle) {

        return $http({ method: "GET", url: "/Statistique/getPriceLastSell/", params: { IdClient: IdClient, IdArticle: IdArticle } }).then(function (result) {
            return result.data;
        });
    };
    return { getData: getData };
});
var checkboxCellEditTemplateDisb = '<div class="ngSelectionCell"><input class="ngSelectionCheckbox"   type="checkbox" ng-model="COL_FIELD" disabled  /></div>';

var timespanCellEditTemplate = '<div class="ngSelectionCell"><input  type="input" ng-model="COL_FIELD" timespaninput  /></div>';

var lookupProductCellEditTemplate = '<input type="text" ng-model="row.entity.IdProduct" placeholder="Choisissez un produit" ' +
'typeahead="Product as Product.Name for Product in lookupFactory.get(\'Products\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Product,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdProduct","Product", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//lookUp WorkUnit
var lookupWorkUnitCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD"  ng-model="row.entity.IdWorkUnit" placeholder="Choisissez un établissement" ' +
'typeahead="WorkUnit as WorkUnit.Name for WorkUnit in lookupFactory.get(\'WorkUnits\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.WorkUnit,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdWorkUnit","WorkUnit", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//JournalType
var lookupArticleCellEditTemplate = '<div class="ui-grid-cell-contents"><input type="text" autocomplete="off" ng-input="COL_FIELD"  ng-model="row.entity.IdArticle" placeholder="Choisissez un article" ' +
'typeahead="Article as Article.Ref for Article in grid.appScope.lookupFactory.get(\'Articles\', \'Ref\', $viewValue)" ' +
'typeahead-input-formatter="grid.appScope.lookupFactory.format(row.entity.Article,\'Ref\')" ' +
'typeahead-on-select=\'grid.appScope.lookupFactory.set(row.entity,"IdArticle","Article", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control"></div>'

var lookupTypeDepenceCellEditTemplate = '<div class="ui-grid-cell-contents"><input ui-grid-editor type="text" autocomplete="off" ng-input="COL_FIELD"  style="border: none;"  autocomplete="off" ng-model="row.entity.IdTypeDepence" placeholder="Choisissez un type dépence" ' +
'typeahead="TypeDepence as TypeDepence.Name for TypeDepence in grid.appScope.lookupFactory.get(\'TypeDepences\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="grid.appScope.lookupFactory.format(row.entity.TypeDepence,\'Name\')" ' +
'typeahead-on-select=\'grid.appScope.lookupFactory.set(row.entity,"IdTypeDepence","TypeDepence", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control"></div>'

//                        <input type="text" autocomplete="off" ng-model="myRef" placeholder="choisissez un Article"
//typeahead="v as v.Ref for v  in lookupFactory.get('Articles', 'Ref', $viewValue)"

//typeahead-wait-ms=" 100"
//                               class="" />


//JournalType
var lookupJournalCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountJournal" placeholder="Choisissez un type" ' +
'typeahead="AccountJournal as AccountJournal.Name for AccountJournal in lookupFactory.get(\'AccountJournals\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountJournal,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountJournal","AccountJournal", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'


var lookupFamilleCellEditTemplate = '<div><input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdFamille" ' +
'typeahead="Famille as Famille.Name for Famille in grid.appScope.lookupFactory.get(\'Familles\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="grid.appScope.lookupFactory.format(row.entity.Famille,\'Name\')" ' +
'typeahead-on-select=\'grid.appScope.lookupFactory.set(row.entity,"IdFamille","Famille", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control"/></div>'

var lookupClientCellEditTemplate = '<div><input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdClient" ' +
    'typeahead="Client as Client.Name for Client in grid.appScope.lookupFactory.get(\'Clients\', \'Name\', $viewValue)" ' +
    'typeahead-input-formatter="grid.appScope.lookupFactory.format(row.entity.Client,\'Name\')" ' +
    'typeahead-on-select=\'grid.appScope.lookupFactory.set(row.entity,"IdClient","Client", $item,"Id")\' ' +
    'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
    'class="form-control"/></div>'

var lookupRevendeurCellEditTemplate = '<div><input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdRevendeur" ' +
'typeahead="Revendeur as Revendeur.Name for Revendeur in grid.appScope.lookupFactory.get(\'Revendeurs\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="grid.appScope.lookupFactory.format(row.entity.Revendeur,\'Name\')" ' +
'typeahead-on-select=\'grid.appScope.lookupFactory.set(row.entity,"IdRevendeur","Revendeur", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control"/></div>'

//Type Dossier
var lookupFolderTypeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdFolderType" placeholder="Choisissez un type" ' +
'typeahead="FolderType as FolderType.Name for FolderType in lookupFactory.get(\'FolderTypes\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.FolderType,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdFolderType","FolderType", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
////////////////////////////////////////////
var lookupTaxeRateCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdTaxeRate" placeholder="Choisissez un taux de TVA" ' +
'typeahead="r as r.Name for r in lookupFactory.getExpand(\'TaxRates\', \'Name\', $viewValue,\'AccountGeneral\',\'non\',\'oui\')" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.TaxeRate,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdTaxeRate","TaxeRate", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
////// compte tva
var lookupTaxeRateCptCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdTaxeRate" placeholder="Choisissez un taux de TVA" ' +
'typeahead="r as r.AccountGeneral.Code+ \' \' + r.Name for r in lookupFactory.getExpand(\'TaxRates\', \'AccountGeneral/Code\', $viewValue,\'AccountGeneral\',\'non\',\'oui\')" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.TaxeRate,\'AccountGeneral.Code\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdTaxeRate","TaxeRate", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//Compte general
var lookupGeneralCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.get(\'AccountGenerals\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneral,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneral", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//Compte general code
var lookupGeneralCodeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Code for AccountGeneral in lookupFactory.get(\'AccountGenerals\', \'Code\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneral,\'Code\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneral", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

var lookupGeneralCellTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.Name"/>';

//Compte Analytic
var lookupAnalyticCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountAnalytic" placeholder="Choisissez un compte" ' +
'typeahead="AccountAnalytic as AccountAnalytic.Name for AccountAnalytic in lookupFactory.get(\'AccountAnalytics\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountAnalytic,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountAnalytic","AccountAnalytic", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//Code analytic code
var lookupAnalyticCodeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountAnalytic" placeholder="Choisissez un compte" ' +
'typeahead="AccountAnalytic as AccountAnalytic.Code for AccountAnalytic in lookupFactory.get(\'AccountAnalytics\', \'Code\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountAnalytic,\'Code\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountAnalytic","AccountAnalytic", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

//<span ng-if="row.entity.IsTiers == true">' +
//'<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
//'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.getTiers(\'Name\', $viewValue,row.entity.Id,false)" ' +
//'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneralParent,\'Name\')" ' +
//'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneralParent", $item,"Id")\' ' +
//'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
//'class="form-control">' +
//'</span>

//var lookupJournalTypeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountJournalType" placeholder="Choisissez un type" ' +
//'typeahead="AccountJournalType as AccountJournalType.Name for AccountJournalType in lookupFactory.get(\'AccountJournalTypes\', \'Name\', $viewValue)" ' +
//'typeahead-input-formatter="lookupFactory.format(row.entity.AccountJournalType,\'Name\')" ' +
//'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountJournalType","AccountJournalType", $item,"Id")\' ' +
//'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
//'class="form-control">'

//compte compte tier name
var lookupCompteTierCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountTier" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.getTiers(\'Name\', $viewValue,null,true,false)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountTier,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountTier","AccountTier", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//compte compte tier code
var lookupCompteTierCodeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountTier" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Code for AccountGeneral in lookupFactory.getTiers(\'Code\', $viewValue,null,true,false)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountTier,\'Code\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountTier","AccountTier", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//compte Contre partie name
var lookupCompteCPCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountContrePartie" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.get(\'AccountGenerals\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountContrePartie,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountContrePartie","AccountContrePartie", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//compte Contre partie code
var lookupCompteCPCodeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountContrePartie" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Code for AccountGeneral in lookupFactory.get(\'AccountGenerals\', \'Code\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountContrePartie,\'Code\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountContrePartie","AccountContrePartie", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
////////////

var CTemplate = '<label tabindex="0" ng-model="row.entity.Name"></label>'
//compte  tiers
var lookupComptenottiersCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.getTiers(\'Name\', $viewValue,row.entity.Id,true,false)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneral,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneral", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//lookUp AccountContact
var lookupContactCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD"  ng-model="row.entity.IdAccountContact" placeholder="Choisissez un contact" ' +
'typeahead="AccountContact as AccountContact.Nom for AccountContact in lookupFactory.get(\'AccountContacts\', \'Nom\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountContact,\'Nom\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountContact","AccountContact", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

//$scope.filterOptions.filterText += ($scope.filterOptions.filterText == '' ? '' : ' and ') + 'IsActive eq true';
//lookUp Compte Collectif
var lookupCompteCellEditTemplate = '<span ng-if="row.entity.IsTiers == true">' +
        '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
        'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.getTiers(\'Name\', $viewValue,row.entity.Id,false,false)" ' +
        'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneralParent,\'Name\')" ' +
        'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneralParent", $item,"Id")\' ' +
        'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
        'class="form-control">' +
     '</span>' +
    '<span ng-if="row.entity.IsTiers == false">' +

     '</span>'
//lookUp code Compte Collectif 
var lookupCompteCodeCellEditTemplate = '<span ng-if="row.entity.IsTiers == true">' +
        '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un compte" ' +
        'typeahead="AccountGeneral as AccountGeneral.Code for AccountGeneral in lookupFactory.getTiers(\'Code\', $viewValue,row.entity.Id,false,false)" ' +
        'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneralParent,\'Code\')" ' +
        'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneralParent", $item,"Id")\' ' +
        'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
        'class="form-control">' +
     '</span>' +
    '<span ng-if="row.entity.IsTiers == false">' +

     '</span>'

//company
var lookupCompanyCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdCompany" placeholder="Choisissez un établissement" ' +
'typeahead="Company as Company.Name for Company in lookupFactory.get(\'Companies\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Company,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCompany","Company", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'


//'<input disabled  type="text" autocomplete="off"  ng-model="row.entity.IdAccountGeneral" placeholder="Choisissez un General" ' +
//'typeahead="AccountGeneral as AccountGeneral.Name for AccountGeneral in lookupFactory.getNotTier(\'Name\', $viewValue)" ' +
//'typeahead-input-formatter="lookupFactory.format(row.entity.AccountGeneralParent,\'Name\')" ' +
//'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountGeneral","AccountGeneralParent", $item,"Id")\' ' +
//'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
//'class="form-control">'



//lookUp Period
var lookupPeriodeCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD" ng-model="row.entity.IdAccountPeriod" placeholder="Choisissez un Periode" ' +
'typeahead="AccountPeriod as AccountPeriod.Name for AccountPeriod in lookupFactory.get(\'AccountPeriods\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountPeriod,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountPeriod","AccountPeriod", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
//lookUp exercice
var lookupExerciseCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD"  ng-model="row.entity.IdAccountExercise" placeholder="Choisissez un Exercice" ' +
'typeahead="AccountExercise as AccountExercise.Annee for AccountExercise in lookupFactory.get(\'AccountExercises\', \'Annee\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AccountExercise,\'Annee\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAccountExercise","AccountExercise", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'



//////////////

var lookupZoneCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdZone" placeholder="Choisissez une zone" ' +
'typeahead="r as r.City+\' \'+r.Name for r in lookupFactory.get(\'Zones\', \'City\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Zone,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdZone","Zone", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupCategoryCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdCategory" placeholder="Choisissez une catégorie" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Categories\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Category,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCategory","Category", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupCategoryWNullCellEditTemplate = ' <p class="input-group"><input type="text" autocomplete="off"  ng-model="row.entity.IdCategory" placeholder="Choisissez une catégorie" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Categories\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Category,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCategory","Category", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control"> ' +
'<span class="input-group-btn"><button type="button" class="btn btn-default" ng-model="row.entity.IdCategory" ' +
'ng-click="lookupFactory.clear(this.$parent.$parent.row.entity,\'IdCategory\',\'Category\',this,\'IdCategory\')"> ' +
'<i class="glyphicon glyphicon-remove"></i></p> ' +
'</button></span>'
var lookupPaymentModeCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdPaymentMode" placeholder="Choisissez une mode de paiement" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'PaymentModes\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.PaymentMode,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdPaymentMode","PaymentMode", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupIngredientCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdIngredient" placeholder="Choisissez un ingredient" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Ingredients\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Ingredient,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdIngredient","Ingredient", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupOptionCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdOption" placeholder="Choisissez une option" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Options\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Option,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdOption","Option", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupMenuCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdMenu" placeholder="Choisissez une Menu" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Menus\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Menu,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdMenu","Menu", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupStepCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdStep" placeholder="Choisissez une Step" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Steps\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Step,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdStep","Step", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupNatureCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdNature" placeholder="Choisissez une Nature" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Natures\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Nature,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdNature","Nature", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupDayCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.Day" placeholder="Choisissez un jour" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Days\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.DayRecord,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"Day","DayRecord", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupWorkUnitCellEditTemplate = '<input type="text" autocomplete="off" ng-input="COL_FIELD"  ng-model="row.entity.IdWorkUnit" placeholder="Choisissez un établissement" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'WorkUnits\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.WorkUnit,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdWorkUnit","WorkUnit", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupCategoryCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdCategory" placeholder="Choisissez une famille" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Categories\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Category,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCategory","Category", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupSubCategoryCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity" placeholder="Choisissez une famille" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Categories\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity,\'Name\')" ' +
'typeahead-on-select=\'setSubCategory(row,item,$item)\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

var lookupAppRoleCellEditTemplate = '<input type="text" required="" autocomplete="off"  ng-model="row.entity.IdAppRole" placeholder="Choisissez un rôle" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'AppRoles\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.AppRole,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdAppRole","AppRole", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

var lookupSaleTypeCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdSaleType" placeholder="Choisissez un type de vente" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'SaleTypes\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.SaleType,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdSaleType","SaleType", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'



var lookupTaxRateCellEditTemplate = '<input type="text" autocomplete="off"  ng-input="COL_FIELD" ng-model="row.entity.IdTaxRate" placeholder="Choisissez un taux de TVA" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'TaxRates\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.TaxRate,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdTaxRate","TaxRate", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

var lookupProductCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdProduct" placeholder="Choisissez un produit" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Products\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Product,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdProduct","Product", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupContentCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdContent" placeholder="Choisissez une page" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Contents\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Content,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdContent","Content", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupMenuItemCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdParent" placeholder="Choisissez un onglet" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'MenuItems\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.ParentMenuItem,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdParent","ParentMenuItem", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupParentComponentCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdParent" placeholder="Choisissez un composant" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Components\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.ParentComponent,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdParent","ParentComponent", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupParentCarteCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdCarteModel" placeholder="Choisissez un modèle" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Cartes\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.CarteModel,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCarteModel","CarteModel", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupProductComponentsCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdComponent" placeholder="Choisissez un composant" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Components\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Component,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdComponent","Component", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupProductCategoriesCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdCategory" placeholder="Choisissez une catégorie" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Categories\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Category,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdCategory","Category", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupProductProvidersCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdProvider" placeholder="Choisissez un fournisseur" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Providers\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.Providers,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdProvider","Providers", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'
var lookupProductStocksCellEditTemplate = '<input type="text" autocomplete="off"  ng-model="row.entity.IdProduct" placeholder="Choisissez un produit" ' +
'typeahead="r as r.Name for r in lookupFactory.get(\'Product\', \'Name\', $viewValue)" ' +
'typeahead-input-formatter="lookupFactory.format(row.entity.ProductStock,\'Name\')" ' +
'typeahead-on-select=\'lookupFactory.set(row.entity,"IdProduct","Product", $item,"Id")\' ' +
'typeahead-wait-ms=" 100" typeahead-append-to-body="true" ' +
'class="form-control">'

app.factory('crudGridDataFactory', ['$http', '$resource', function ($http, $resource) {
    var odataUrl = "/Odata/"
    return function (type, filterText, expand,sortField) {
        var sort = "asc";
        //if (sortDirection == "desc")
        //    sort = "desc";
        //if (sortField)
        //    sortField = sortField.replace(".", "/") + ' ' + sort;
        
        if (sortField != undefined && sortField != null)
        {
            //if (sortField)
                sortField = sortField.replace(".", "/") + ' desc';
            var queryparams = { key: "@key", $inlinecount: "allpages", $orderby: sortField/*, $top: pageSize, $skip: pageSize * (currentPage - 1), $orderby: sortField */ };
        } else
        {
            var queryparams = { key: "@key", $inlinecount: "allpages"/*, $top: pageSize, $skip: pageSize * (currentPage - 1), $orderby: sortField */ };
        }
        if (filterText != null && filterText != '') {
            queryparams.$filter = filterText;
        }
        if (expand != null && expand != '') {
            queryparams.$expand = expand
        }
        return $resource("", {}, {
            'getAll': { method: "GET", url: odataUrl + type },
            'save': { method: "POST", url: odataUrl + type },
            'update': { method: 'PUT', params: { key: "@key" }, url: odataUrl + type + "(:key)" },
            'query': { method: 'GET', params: queryparams, url: odataUrl + type + "(:key)" },
            'remove': { method: 'DELETE', params: { key: "@key" }, url: odataUrl + type + "(:key)" }
        });
    };
}]);
app.factory('crudGridDataFactoryPage', ['$http', '$resource', function ($http, $resource) {
    var odataUrl = "/Odata/"
    return function (type, pageSize, currentPage, filterText, sortField, sortDirection, expand) {
        var sort = "asc";
        if (sortDirection == "desc")
            sort = "desc";
        if (sortField)
            sortField = sortField.replace(".", "/") + ' ' + sort;
        var queryparams = { key: "@key", $inlinecount: "allpages", $top: pageSize, $skip: pageSize * (currentPage - 1), $orderby: sortField };
        if (filterText != null && filterText != '') {
            queryparams.$filter = filterText;
        }
        if (expand != null && expand != '') {
            queryparams.$expand = expand
        }
        return $resource("", {}, {
            'getAll': { method: "GET", url: odataUrl + type },
            'save': { method: "POST", url: odataUrl + type },
            'update': { method: 'PUT', params: { key: "@key" }, url: odataUrl + type + "(:key)" },
            'query': { method: 'GET', params: queryparams, url: odataUrl + type + "(:key)" },
            'remove': { method: 'DELETE', params: { key: "@key" }, url: odataUrl + type + "(:key)" }
        });
    };
}]);


app.constant('Animations', {
    opacity: {
        start: 'opacity: 0',
        end: 'opacity: 1'
    },
    'slide': {
        start: 'transform: translateX(-100%)',
        end: 'transform: translateX(0)'
    }
})

//app.directive('uiSelectWrap', uiSelectWrap);

//uiSelectWrap.$inject = ['$document', 'uiGridEditConstants'];
//function uiSelectWrap($document, uiGridEditConstants) {
//    return function link($scope, $elm, $attr) {
//        $document.on('click', docClick);
//        $document.on('focusout', docClick);
        
//        //$scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);

//        function docClick(evt) {
//            if ($(evt.target).closest('.mesMarticles').size() === 0) {
//            //console.log($(evt.target));
//                $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
//               // $document.off('click', docClick);
//            }
//        }
//    };
//}

//app.directive('uiGridRow', function ($animate, $timeout, uiGridConstants) {
//    return {
//        priority: -1,
//        link: function ($scope, $elm, $attrs) {
//            $scope.$watch('row.entity', function (n, o) {
//                console.log("ok")
//                if ($scope.row.isNew) {
//                    $elm.addClass('new-row');

//                    $timeout(function () {
//                        $animate.removeClass($elm, 'new-row');
//                    });

//                    $scope.row.isNew = false;
//                }
//            });

//            $scope.$on('delete-row', function (evt, row) {
//                $animate.addClass($elm, 'delete-row')
//                  .then(function () {
//                      $elm.removeClass('delete-row');

//                      // Not in $digest
//                      $timeout(function () {
//                          var data = $scope.grid.options.data;
//                          data.splice(data.indexOf(row.entity), 1);
//                      });
//                  });
//            });
//        }
//    }
//})

//app.directive('uiGridCell', function () {
//    return {
//        priority: -1,
//        link: function ($scope, $elm, $attrs) {
//            $scope.deleteRow = deleteRow;

//            function deleteRow(row) {
//                $scope.$emit('delete-row', row);
//            }
//        }
//    }
//})

function getCompanies() {

    var result = []
    $.ajax({
        url: '/Odata/Companies',
        cache: false,
        async: false,
        success: function (data) {
            result = data.value;
        }
    });

    return result;
}
function getCompanies() {

    var result = []
    $.ajax({
        url: '/Odata/Companies',
        cache: false,
        async: false,
        success: function (data) {
            result = data.value;
        }
    });

    return result;
}
app.factory('lookupFactory', function ($http) {
    return {

        get: function (table, field, text) {
            //alert("get");
            var filterText = '';
            if (text > '')
                filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1';
            else

                var result = []
            $.ajax({
                url: '/Odata/' + table + '?&$top=20' + filterText,
                cache: false,
                async: false,
                success: function (data) {
                    result = data.value;
                }
            });
            return result;
        },
        getArticles: function (table, field, text) {
            var filterText = '';
            if (text > '')
                filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1 and Ref ne \'-\'';
            else

                var result = []
            $.ajax({
                url: '/Odata/' + table + '?&$top=20' + filterText,
                cache: false,
                async: false,
                success: function (data) {
                    result = data.value;
                }
            });
            return result;
        },
        getLastId: function (table,field) {
            //alert("get");
            filterText = '&$select=' + field + "&$filter=" + 'year(Date) eq ' + new Date().getFullYear();

                var result = []
            $.ajax({
                url: '/Odata/' + table + '?&$top=1&$orderby=Ref desc' + filterText,
                cache: false,
                async: false,
                success: function (data) {
                    result = data.value;
                }
            });
            return result;
        },
        getAg: function (table, fields, filterText, expand) {
            sumDebit = 0;
            sumCredit = 0;

            var result = []
            filterText = '$filter=' + filterText;
            expand = "&$expand=" + expand;
            fields = "&$select=" + fields;
            $.ajax({
                url: '/Odata/' + table + '?' + filterText + expand + fields,
                cache: false,
                async: false,
                success: function (data) {
                    for (i = 0; i < data.value.length; i++) {
                        sumCredit += data.value[i].Credit;
                        sumDebit += data.value[i].Debit;
                    }
                }
            });
            result = { sumCredit: sumCredit, sumDebit: sumDebit }
            return result;
        },

        getExpand: function (table, field, text, expand, vide, limit, wheres, orderBy) {
            var filterText = '';
            var expandText = '';
            var top = '';
            var whereText = '';
            if (text > '') {
                filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1';
            }
            else {
                if (vide == 'oui') {
                    filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1';
                }
                var result = []
            }
            if (wheres != null && wheres != undefined) {
                angular.forEach(wheres, function (value, key) {
                    if (value == "") {
                        whereText += (filterText == '' ? '&$filter=' : ' and ') + key + ' eq null'
                    } else {
                        whereText += (filterText == '' ? '&$filter=' : ' and ') + key + ' eq (guid\'' + value + '\')'
                    }
                });
            }

            if (expand != null && expand != '') {
                expandText = "$expand=" + expand;
            }

            if (orderBy)
                expandText += '&'+orderBy;

            if (limit == 'oui') {
                top = "&$top=20";
            }



            $.ajax({
                url: '/Odata/' + table + '?' + expandText + top + filterText + whereText,
                cache: false,
                async: false,
                success: function (data) {
                    result = data.value;
                }
            });
            return result;
        },


        getTiers: function (field, text, him, tier, lettrable) {

            //console.log(him);
            var filterText = '';
            var himBool = '';
            if (text != '') {
                var res = field.split(",");
                filterText = ' and ('
                for (i = 0; i < res.length; i++) {
                    filterText += 'indexof(' + res[i] + ',\'' + text + '\') gt -1 ';
                    if (res.length != i + 1) {
                        filterText += 'or '
                    }
                }
                filterText += ')'
            }
            if (him != undefined) {
                himBool = ' and Id ne guid\'' + him + '\'';
            }
            if (lettrable == true) {
                filterText += ' and lettrable eq true';
            }
            var result = [];
            //alert(tiers);
            $.ajax({
                url: '/Odata/AccountGenerals()?$top=20&$filter=IsTiers eq ' + tier + ' ' + filterText + himBool,
                // url: '/Odata/AccountGenerals()?$filter=IsTiers eq false' + ,
                cache: false,
                async: false,
                success: function (data) {

                    result = data.value;
                    // console.log(result)
                }
            });
            return result;
        },
        set: function (item, foreignKey, relatedObject, selectedItem, primarykey) {
            //alert('eeee')

            if (selectedItem == null) {
                item[foreignKey] = null;
                item[relatedObject] = null;

            }
            else {
                item[foreignKey] = selectedItem[primarykey];
                item[relatedObject] = selectedItem;
            }
        }
        , clear: function (item, foreignKey, relatedObject, obj, label) {
            //alert("clear");
            item[foreignKey] = null;
            item[relatedObject] = null;
            obj[label] = null;
        }
        , format: function (obj, name) {

            return (obj ? obj[name] : null);
            //alert("format");
        },
        formattest: function (obj, name) {
            //varr = obj[name];
            //console.log(varr);
            //return varr;
            //alert('oui hadi hiya setect')
            return (obj ? obj[name] : null);
        }

    };

});
toastr.options = {
    "closeButton": false,
    "debug": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "onclick": null,
    "showDuration": "200000",
    "hideDuration": "1000",
    "timeOut": "6000",
    "extendedTimeOut": "10000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
app.factory('notificationFactory', function () {
    return {
        success: function (text) {
            toastr.success(text ? text : "Succès");
        },
        error: function (text, title) {
            if (!title)
                title = "Erreur"
            toastr.error(text, title);
        },
        warning: function (text, title) {
            if (!title)
                title = "Warning"
            toastr.warning(text, title);
        },
        info: function (text, title) {
            if (!title)
                title = "Info"
            toastr.info(text, title);
        }
    };
});
app.directive('timespaninput', function () {

    function parser(data) {
        var converted = moment().startOf('day').add(moment.duration(data, 'H:M:s.SSS')).format('[PT]HH[H]mm[M]ss.SSS[S]');
        return converted;
    };
    function formatter(data) {
        var converted = moment().startOf('day').add(moment.duration(data)).format('HH:mm:ss.SSS');
        return converted;
    };

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ctrl) {
            ctrl.$parsers.unshift(parser);
            ctrl.$formatters.unshift(formatter);
        }
    };
});


//app.directive('jqdatepicker', function () {
//    function formatter(data) {

//        var converted = moment(data).format("DD/MM/YYYY");
//        return converted;
//    };
//    return {
//        restrict: 'A',
//        require: 'ngModel',
//        link: function (scope, element, attrs, ngModelCtrl) {
//            ngModelCtrl.$formatters.unshift(formatter);
//            $(element).datepicker({ language: "fr" })
//  .on('changeDate', function (ev) {
//      date = ev.date;
//      date= moment(moment(date).format("DD/MM/YYYY"+" +0000"), "DD/MM/YYYY Z").toDate();
//      var ngModelName = this.attributes['ng-model'].value;
//      // if value for the specified ngModel is a property of 
//      // another object on the scope
//      if (ngModelName.indexOf(".") != -1) {
//          var objAttributes = ngModelName.split(".");
//          var lastAttribute = objAttributes.pop();
//          var partialObjString = objAttributes.join(".");
//          var partialObj = eval("scope." + partialObjString);
//          partialObj[lastAttribute] = date;
//      }
//          // if value for the specified ngModel is directly on the scope
//      else {
//          scope[ngModelName] = date;
//      }
//      scope.$apply();

//  });
//            $.datepicker.setDefaults($.datepicker.regional["fr"]);
//            $(element).datepicker({
//                dateFormat: 'dd/mm/yy',
//                onSelect: function (date) {
//                    date = moment.utc(date, "DD/MM/YYYY").startOf('day');
//                    var ngModelName = this.attributes['ng-model'].value;
//                    // if value for the specified ngModel is a property of 
//                    // another object on the scope
//                    if (ngModelName.indexOf(".") != -1) {
//                        var objAttributes = ngModelName.split(".");
//                        var lastAttribute = objAttributes.pop();
//                        var partialObjString = objAttributes.join(".");
//                        var partialObj = eval("scope." + partialObjString);
//                        partialObj[lastAttribute] = date;
//                    }
//                        // if value for the specified ngModel is directly on the scope
//                    else {
//                        scope[ngModelName] = date;
//                    }
//                    scope.$apply();
//                }

//            });
//        }
//    };
//});
//app.directive('daate', function () {
//    return {
//        restrict: 'A',
//        require: 'ngModel',
//        link: function (scope, element, attrs, ngModelCtrl) {






//            //$(function () {
//            //    var dp = $(element).datepicker({
//            //        format: 'dd/mm/yyyy'
//            //    });

//            //    dp.on('changeDate', function (e) {
//            //        ngModelCtrl.$setViewValue(e.format('dd/mm/yyyy'));
//            //       // scope.$apply();
//            //    });
//            //});
//            // Store the initial cell value so we can reset to it if need be
//            var oldCellValue;
//            var dereg = scope.$watch('ngModel', function () {
//                oldCellValue = ngModelCtrl.$modelValue;
//                dereg(); // only run this watch once, we don't want to overwrite our stored value when the input changes
//            });
//            element.bind('keydown', function (evt) {
//                switch (evt.keyCode) {
//                    case 37: // Left arrow
//                    case 38: // Up arrow
//                    case 39: // Right arrow
//                    case 40: // Down arrow
//                        evt.stopPropagation();
//                        break;
//                    case 27: // Esc (reset to old value)
//                        if (!scope.$$phase) {
//                            //scope.$apply(function () {
//                                ngModelCtrl.$setViewValue(oldCellValue);
//                                element.blur();
//                            //});
//                        }
//                        break;
//                    case 13: // Enter (Leave Field)
//                        if (scope.enableCellEditOnFocus && scope.totalFilteredItemsLength() - 1 > scope.row.rowIndex && scope.row.rowIndex > 0 || scope.enableCellEdit) {
//                            element.blur();
//                        }
//                        break;
//                }

//                return true;
//            });

//            element.bind('click', function (evt) {
//                //evt.stopPropagation();
//            });

//            element.bind('mousedown', function (evt) {
//                evt.stopPropagation();
//            });

//            scope.$on('ngGridEventStartCellEdit', function () {
//                element.focus();
//                element.select();
//            });

//            // Begin: datepicker specific changes to ng-input

//            scope.isOpen = true;

//            scope.$watch('isOpen', function (newValue, oldValue) {
//                if (newValue === false) {
//                    scope.$emit('ngGridEventEndCellEdit');
//                }
//            });

//            angular.element(element).bind('blur', function () {
//                //scope.$emit('ngGridEventEndCellEdit'); muss manuell (später) ausgeführt werden.
//            });

//            // End: datepicker specific changes to ng-input






//        }
//    }
//});


app.directive("maDate", function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ngModel) {
            console.log("Mon directive")

            var oldCellValue;
            var dereg = scope.$watch('ngModel', function () {
                oldCellValue = ngModel.$modelValue;
                dereg(); // only run this watch once, we don't want to overwrite our stored value when the input changes
            });

            elm.bind('keydown', function (evt) {
                switch (evt.keyCode) {
                    case 37: // Left arrow
                    case 38: // Up arrow
                    case 39: // Right arrow
                    case 40: // Down arrow
                        evt.stopPropagation();
                        break;
                    case 27: // Esc
                        if (!scope.$$phase) {
                            scope.$apply(function () {
                                ngModel.$setViewValue(oldCellValue);
                                elm.blur();
                            });
                        }
                        break;
                    case 13:
                        if (scope.enableCellEditOnFocus && scope.totalFilteredItemsLength() - 1 > scope.row.rowIndex && scope.row.rowIndex > 0 || scope.enableCellEdit) {
                            elm.blur();
                        }
                        break;
                    case 9: // 
                        scope.$emit('ngGridEventEndCellEdit');

                        break;

                }

                return true;
            });

            elm.bind('click', function (evt) {
                evt.stopPropagation();
            });

            elm.bind('mousedown', function (evt) {
                evt.stopPropagation();
            });

            scope.$on('ngGridEventStartCellEdit', function () {
                $(":input").inputmask();

                //elm[0].setSelectionRange(0, 10);
                elm.focus();
                elm.select();
            });



            scope.isOpen = true;

            scope.$watch('isOpen', function (newValue, oldValue) {

                ngModel.$setViewValue(new Date(ngModel.$modelValue)); // Convertir la chaine vers la date

                if (newValue === false) {
                    scope.$emit('ngGridEventEndCellEdit');


                }


            });


            angular.element(elm).bind('blur', function () {
                //scope.$emit('ngGridEventEndCellEdit');
                // console.log(elm);

            });

            // End: datepicker specific changes to ng-input
        }
    };
});


//app.run(function (editableOptions, editableThemes) {
//    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
//    editableThemes.bs3.inputClass = 'input-sm';
//});

app.directive('jqdatepicker',

      function () {
          return {
              restrict: 'A',
              require: 'ngModel',
              scope: {},
              link: function (scope, element, attrs, ngModel) {

                  if (!ngModel) return;

                  var optionsObj = {};
                  optionsObj.format = 'dd/mm/yyyy';
                  optionsObj.autoclose = true;


                  element.datepicker(optionsObj);

              }
          };
      });
app.directive('ckEditor', [function () {
    return {
        require: '?ngModel',
        link: function ($scope, elm, attr, ngModel) {

            var ck = CKEDITOR.replace(elm[0]);

            ck.on('pasteState', function () {
                $scope.$apply(function () {
                    ngModel.$setViewValue(ck.getData());
                });
            });

            ngModel.$render = function (value) {
                ck.setData(ngModel.$modelValue);
            };

        }
    };
}])
app.filter('time', function ($filter) {
    return function (dateToFormat) {
        return moment(dateToFormat).format("HH:mm:ss");
    }
});
app.filter('datewotime', function ($filter) {
    return function (dateToFormat) {
        dateToFormat = moment(dateToFormat).startOf('day');
        return moment(dateToFormat).format("LL");
    }
});


app.filter('timespan', function ($filter) {
    return function (dateToFormat) {

        return moment().startOf('day').add(moment.duration(dateToFormat)).format('HH:mm:ss.SSS');
    }
});
app.filter('deliverymode', function ($filter) {
    return function (dl) {
        return dl == 2 ? "Livraison" : "Emporter";
    }
});
$('#maintabs a').click(function (e) {
    e.preventDefault()
    $(this).tab('show')
})
function remove(arr, item) {
    for (var i = arr.length; i--;) {
        if (arr[i] === item) {
            arr.splice(i, 1);
        }
    }
}