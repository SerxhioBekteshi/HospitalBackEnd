using Shared.ResponseFeatures;
using System.Xml.Linq;

namespace Shared.DTC;

public class UserColumn
{
    public static string id = "Id";
    public static string firstName = "Emri";
    public static string lastName = "Mbiemri";
    public static string email = "Email";
    public static string phoneNumber = "Numri i telefonit";
    public static string address = "Adresa";
    public static string city = "Qyteti";
    public static string state = "Shteti";
    public static string zipCode = "Kodi ZIP";
    public static string gender = "Gjinia";
    public static string birthday = "Ditëlindja";
    public static string roleId = "Id e rolit";
    public static string role = "Roli";
    public static string dateCreated = "Data e regjistrimit";
    public static string actions = null;

    public static string GetPropertyDescription(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
                return id;
            case nameof(firstName):
                return firstName;
            case nameof(lastName):
                return lastName;
            case nameof(email):
                return email;
            case nameof(phoneNumber):
                return phoneNumber;
            case nameof(dateCreated):
                return dateCreated;
            case nameof(address):
                return address;
            case nameof(city):
                return city;
            case nameof(state):
                return state;
            case nameof(zipCode):
                return zipCode;
            case nameof(gender):
                return gender;
            case nameof(birthday):
                return birthday;
            case nameof(roleId):
                return roleId;
            case nameof(role):
                return role;
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
            case nameof(roleId):
                return true;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(phoneNumber):
            case nameof(dateCreated):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
            case nameof(gender):
            case nameof(birthday):
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
            case nameof(roleId):
                return false;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(phoneNumber):
            case nameof(dateCreated):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
            case nameof(gender):
            case nameof(birthday):
            case nameof(actions):
                return true;
            default:
                return true;
        }
    }

    public static bool GetPropertyHideable(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
            case nameof(roleId):
                return true;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(phoneNumber):
            case nameof(dateCreated):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
            case nameof(gender):
            case nameof(birthday):
            case nameof(actions):
                return false;
            default:
                return true;
        }
    }

    public static int GetPropertyMinWidth(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(id):
            case nameof(roleId):
            case nameof(gender):
                return 50;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(phoneNumber):
            case nameof(dateCreated):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
            case nameof(birthday):
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
            case nameof(roleId):
            case nameof(phoneNumber):
                return DataType.Number;
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
            case nameof(gender):
                return DataType.String;
            case nameof(dateCreated):
            case nameof(birthday):
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
            case nameof(roleId):
            case nameof(gender):
            case nameof(firstName):
            case nameof(lastName):
            case nameof(email):
            case nameof(phoneNumber):
            case nameof(dateCreated):
            case nameof(role):
            case nameof(address):
            case nameof(city):
            case nameof(state):
            case nameof(zipCode):
                return null;
            case nameof(actions):
                return actionsData;
            default:
                return actionsData;
        }
    }
}