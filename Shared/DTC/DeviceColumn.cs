using Shared.ResponseFeatures;
using System;

namespace Shared.DTC;

public class DeviceColumn
{
    public static string id = "Id";
    public static string name = "Emri";
    public static string isActive = "Pajisja aktive";
    public static string dateCreated = "Data krijimit";
    public static string createdByFullName = "Krijuar nga";
    public static string dateModified = "Data modifikimit";
    public static string modifiedByFullName = "Modifikuar nga";
    public static string actions = null;

    public static string GetPropertyDescription(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return id;
            case nameof(name):
                return name;
            case nameof(isActive):
                return isActive;
            case nameof(dateCreated):
                return dateCreated;
            case nameof(createdByFullName):
                return createdByFullName;
            case nameof(dateModified):
                return dateModified;
            case nameof(modifiedByFullName):
                return modifiedByFullName;
            case nameof(actions):
                return null;
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
            case nameof(isActive):
            case nameof(modifiedByFullName):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(actions):
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
            case nameof(name):
            case nameof(isActive):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
            case nameof(actions):
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
            case nameof(name):
            case nameof(isActive):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
            case nameof(actions):
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
            case nameof(name):
            case nameof(isActive):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
            case nameof(actions):
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
                return DataType.String;
            case nameof(isActive):
                return DataType.Boolean;
            case nameof(createdByFullName):
            case nameof(modifiedByFullName):
                return DataType.String;
            case nameof(dateCreated):
            case nameof(dateModified):
                return DataType.DateTime;
            case nameof(actions):
                return DataType.Actions;
            default:
                return DataType.String;
        }
    }

    public static object[] GetPropertyActions(string propertyName)
    {

        object[] actionsData =
        {
                new { name = "edit", icon= "fa-regular fa-pen-to-square", color = "blue"},
                new { name = "delete", icon= "fa-regular fa-trash-can", color = "red" },
            };

        switch (propertyName)
        {
            case nameof(id):
            case nameof(name):
            case nameof(isActive):
            case nameof(dateCreated):
            case nameof(createdByFullName):
            case nameof(dateModified):
            case nameof(modifiedByFullName):
                return null;
            case nameof(actions):
                return actionsData;
            default:
                return actionsData;
        }
    }
}