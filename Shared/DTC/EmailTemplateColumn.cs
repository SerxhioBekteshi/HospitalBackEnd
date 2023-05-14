using Shared.ResponseFeatures;

namespace Shared.DTC;

public class EmailTemplateColumn
{
    public static string id = "Id";
    public static string name = "Emri";
    public static string code = "Kodi";
    public static string subject = "Subjekti";
    public static string dateCreated = "Data krijimit";
    public static string createdByFullName = "Krijuar nga";
    public static string dateModified = "Data modifikimit";
    public static string modifiedByFullName = "Modifikuar nga";

    public static string GetPropertyDescription(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return id;
            case nameof(name):
                return name;
            case nameof(code):
                return code;
            case nameof(subject):
                return subject;
            case nameof(dateCreated):
                return dateCreated;
            case nameof(createdByFullName):
                return createdByFullName;
            case nameof(dateModified):
                return dateModified;
            case nameof(modifiedByFullName):
                return modifiedByFullName;
            default:
                return "";
        }
    }

    public static bool GetPropertyIsHidden(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return true;
            case nameof(name):
            case nameof(code):
            case nameof(modifiedByFullName):
            case nameof(subject):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
                return false;
            default:
                return true;
        }
    }

    public static bool GetPropertyHideable(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return true;
            case nameof(code):
            case nameof(name):
            case nameof(subject):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
                return false;
            default:
                return true;
        }
    }
    
    public static bool GetPropertyFilterable(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return false;
            case nameof(code):
            case nameof(name):
            case nameof(subject):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
                return true;
            default:
                return true;
        }

    }
    
    public static int GetPropertyMinWidth(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return 50;
            case nameof(code):
            case nameof(name):
            case nameof(subject):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
                return 80;
            default:
                return 50;
        }
    }
    
    public static DataType GetPropertyDataType(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return DataType.Number;
            case nameof(name):
            case nameof(code):
            case nameof(subject):
            case nameof(createdByFullName):
            case nameof(modifiedByFullName):
                return DataType.String;
            case nameof(dateCreated):
            case nameof(dateModified):
                return DataType.DateTime;
            default:
                return DataType.String;
        }
    }
}