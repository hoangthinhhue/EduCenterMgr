using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Constants;
public static class ConstantString
{
    //==========================================================//
    //for button text
    public static string REFRESH => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Refresh");
    public static string EDIT => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Edit");
    public static string DELETE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete");
    public static string ADD => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Add");
    public static string CREATE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Create");
    public static string EXPORT => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Export to Excel");
    public static string EXPORTPDF => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Export to PDF");
    public static string IMPORT => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Import from Excel");
    public static string ACTIONS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Actions");
    public static string SAVE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Save");
    public static string SAVECHANGES => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Save Changes");
    public static string CANCEL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Cancel");
    public static string CLOSE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Close");
    public static string SEARCH => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Search");
    public static string CLEAR => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Clear");
    public static string RESET => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Reset");
    public static string OK => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("OK");
    public static string CONFIRM => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Confirm");
    public static string YES => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Yes");
    public static string NO => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("No");
    public static string NEXT => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Next");
    public static string PREVIOUS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Previous");
    public static string UPLOAD => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Upload");
    public static string DOWNLOAD => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Download");
    public static string UPLOADING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Uploading...");
    public static string DOWNLOADING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Downloading...");
    public static string NOALLOWED => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("No Allowed");
    public static string SIGNINWITH => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Sign in with {0}");
    public static string LOGOUT => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Logout");
    public static string SIGNIN => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Sign In");
    public static string Microsoft => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Microsoft");
    public static string Facebook => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Facebook");
    public static string Google => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Google");

    //============================================================================//
    // for toast message
    public static string SAVESUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Save successfully");
    public static string DELETESUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete successfully");
    public static string DELETEFAIL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete fail");
    public static string UPDATESUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Update successfully");
    public static string CREATESUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Create successfully");
    public static string LOGINSUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Login successfully");
    public static string LOGOUTSUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Logout successfully");
    public static string LOGINFAIL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Login fail");
    public static string LOGOUTFAIL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Logout fail");
    public static string IMPORTSUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Import successfully");
    public static string IMPORTFAIL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Import fail");
    public static string EXPORTSUCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Export successfully");
    public static string EXPORTFAIL => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Export fail");
    public static string UPLOADSUCCESS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Upload successfully");

    //========================================================

    public static string ADVANCEDSEARCH => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Advanced Search");
    public static string ORDERBY => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Order By");
    public static string CREATEAITEM => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Create a new {0}");
    public static string EDITTHEITEM => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Edit the {0}");
    public static string DELETETHEITEM => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete the {0}");
    public static string DELETEITEMS => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete selected items: {0}");
    public static string DELETECONFIRMATION => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Are you sure you want to delete this item: {0}?");
    public static string DELETECONFIRMATIONWITHID => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Are you sure you want to delete this item with Id: {0}?");
    public static string DELETECONFIRMWITHSELECTED => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Are you sure you want to delete the selected items: {0}?");
    public static string NOMACHING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("No matching records found");
    public static string LOADING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Loading...");
    public static string WATING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Wating...");
    public static string PROCESSING => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Processing...");
    public static string DELETECONFIRMATIONTITLE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Delete Confirmation");
    public static string LOGOUTCONFIRMATIONTITLE => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("Logout Confirmation");
    public static string LOGOUTCONFIRMATION => CleanArchitecture.Blazor.Application.Constants.ConstantStringLocalizer.Localize("You are attempting to log out of application. Do you really want to log out?");
}
