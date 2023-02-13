using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Helper;
namespace CleanArchitecture.Blazor.Domain.Constants;
public static class ConstantString
{
    //==========================================================//
    //for button text
    public static string REFRESH => ConstantStringLocalizer.Localize("Refresh");
    public static string EDIT => ConstantStringLocalizer.Localize("Edit");
    public static string DELETE => ConstantStringLocalizer.Localize("Delete");
    public static string ADD => ConstantStringLocalizer.Localize("Add");
    public static string CREATE => ConstantStringLocalizer.Localize("Create");
    public static string EXPORT => ConstantStringLocalizer.Localize("Export to Excel");
    public static string EXPORTPDF => ConstantStringLocalizer.Localize("Export to PDF");
    public static string IMPORT => ConstantStringLocalizer.Localize("Import from Excel");
    public static string ACTIONS => ConstantStringLocalizer.Localize("Actions");
    public static string SAVE => ConstantStringLocalizer.Localize("Save");
    public static string SAVECHANGES => ConstantStringLocalizer.Localize("Save Changes");
    public static string CANCEL => ConstantStringLocalizer.Localize("Cancel");
    public static string CLOSE => ConstantStringLocalizer.Localize("Close");
    public static string SEARCH => ConstantStringLocalizer.Localize("Search");
    public static string CLEAR => ConstantStringLocalizer.Localize("Clear");
    public static string RESET => ConstantStringLocalizer.Localize("Reset");
    public static string OK => ConstantStringLocalizer.Localize("OK");
    public static string CONFIRM => ConstantStringLocalizer.Localize("Confirm");
    public static string YES => ConstantStringLocalizer.Localize("Yes");
    public static string NO => ConstantStringLocalizer.Localize("No");
    public static string NEXT => ConstantStringLocalizer.Localize("Next");
    public static string PREVIOUS => ConstantStringLocalizer.Localize("Previous");
    public static string UPLOAD => ConstantStringLocalizer.Localize("Upload");
    public static string DOWNLOAD => ConstantStringLocalizer.Localize("Download");
    public static string UPLOADING => ConstantStringLocalizer.Localize("Uploading...");
    public static string DOWNLOADING => ConstantStringLocalizer.Localize("Downloading...");
    public static string NOALLOWED => ConstantStringLocalizer.Localize("No Allowed");
    public static string SIGNINWITH => ConstantStringLocalizer.Localize("Sign in with {0}");
    public static string LOGOUT => ConstantStringLocalizer.Localize("Logout");
    public static string SIGNIN => ConstantStringLocalizer.Localize("Sign In");
    public static string Microsoft => ConstantStringLocalizer.Localize("Microsoft");
    public static string Facebook => ConstantStringLocalizer.Localize("Facebook");
    public static string Google => ConstantStringLocalizer.Localize("Google");

    //============================================================================//
    // for toast message
    public static string SAVESUCCESS => ConstantStringLocalizer.Localize("Save successfully");
    public static string DELETESUCCESS => ConstantStringLocalizer.Localize("Delete successfully");
    public static string DELETEFAIL => ConstantStringLocalizer.Localize("Delete fail");
    public static string UPDATESUCCESS => ConstantStringLocalizer.Localize("Update successfully");
    public static string CREATESUCCESS => ConstantStringLocalizer.Localize("Create successfully");
    public static string LOGINSUCCESS => ConstantStringLocalizer.Localize("Login successfully");
    public static string LOGOUTSUCCESS => ConstantStringLocalizer.Localize("Logout successfully");
    public static string LOGINFAIL => ConstantStringLocalizer.Localize("Login fail");
    public static string LOGOUTFAIL => ConstantStringLocalizer.Localize("Logout fail");
    public static string IMPORTSUCCESS => ConstantStringLocalizer.Localize("Import successfully");
    public static string IMPORTFAIL => ConstantStringLocalizer.Localize("Import fail");
    public static string EXPORTSUCESS => ConstantStringLocalizer.Localize("Export successfully");
    public static string EXPORTFAIL => ConstantStringLocalizer.Localize("Export fail");
    public static string UPLOADSUCCESS => ConstantStringLocalizer.Localize("Upload successfully");

    //========================================================

    public static string ADVANCEDSEARCH => ConstantStringLocalizer.Localize("Advanced Search");
    public static string ORDERBY => ConstantStringLocalizer.Localize("Order By");
    public static string CREATEAITEM => ConstantStringLocalizer.Localize("Create a new {0}");
    public static string EDITTHEITEM => ConstantStringLocalizer.Localize("Edit the {0}");
    public static string DELETETHEITEM => ConstantStringLocalizer.Localize("Delete the {0}");
    public static string DELETEITEMS => ConstantStringLocalizer.Localize("Delete selected items: {0}");
    public static string DELETECONFIRMATION => ConstantStringLocalizer.Localize("Are you sure you want to delete this item: {0}?");
    public static string DELETECONFIRMATIONWITHID => ConstantStringLocalizer.Localize("Are you sure you want to delete this item with Id: {0}?");
    public static string DELETECONFIRMWITHSELECTED => ConstantStringLocalizer.Localize("Are you sure you want to delete the selected items: {0}?");
    public static string NOMACHING => ConstantStringLocalizer.Localize("No matching records found");
    public static string LOADING => ConstantStringLocalizer.Localize("Loading...");
    public static string WATING => ConstantStringLocalizer.Localize("Wating...");
    public static string PROCESSING => ConstantStringLocalizer.Localize("Processing...");
    public static string DELETECONFIRMATIONTITLE => ConstantStringLocalizer.Localize("Delete Confirmation");
    public static string LOGOUTCONFIRMATIONTITLE => ConstantStringLocalizer.Localize("Logout Confirmation");
    public static string LOGOUTCONFIRMATION => ConstantStringLocalizer.Localize("You are attempting to log out of application. Do you really want to log out?");
}
