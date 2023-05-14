using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTC
{
    public class ReportsColumn
    {
        public static string reportId = "Id";
        public static string startTime = "Start Time";
        public static string endTime = "End Time";
        public static string phoneNumber = "Numri i telefonit";
        public static string clientId = "Client ID";
        public static string name = "Emri";
        public static string surname = "Mbiemri";
        public static string email = "Emaili";
        public static string statusMessage = "Konfirmimi i rezervimi";
        public static string status = "Statusi i rezervimit";
        public static string dateCreated = "Data krijimit";
        public static string createdByFullName = "Krijuar nga";
        public static string dateModified = "Data modifikimit";
        public static string modifiedByFullName = "Modifikuar nga";
        public static string actions = null;

        public static string GetPropertyDescription(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(reportId):
                    return reportId;
                case nameof(startTime):
                    return startTime;
                case nameof(endTime):
                    return endTime;
                case nameof(phoneNumber):
                    return phoneNumber;
                case nameof(clientId):
                    return clientId;
                case nameof(name):
                    return name;
                case nameof(surname):
                    return surname;
                case nameof(email):
                    return email;
                case nameof(statusMessage):
                    return statusMessage;
                case nameof(status):
                    return status;
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
                case nameof(reportId):
                    return true;
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                case nameof(status):
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
                case nameof(reportId):
                    return false;
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                case nameof(status):
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

        public static bool GetPropertyHideable(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(reportId):
                    return false;
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                case nameof(status):
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

        public static int GetPropertyMinWidth(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(reportId):
                    return 50;
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                case nameof(status):
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
                case nameof(reportId):
                    return DataType.Number;
                case nameof(startTime):
                case nameof(endTime):
                    return DataType.DateTime;
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                    return DataType.String;
                case nameof(status):
                    return DataType.String;
                case nameof(dateCreated):
                    return DataType.DateTime;
                case nameof(createdByFullName):
                    return DataType.String;
                case nameof(dateModified):
                    return DataType.DateTime;
                case nameof(modifiedByFullName):
                    return DataType.String;
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
                new { name = "delete", icon= "fa-regular fa-trash-can", color = "red" },
            };

            switch (propertyName)
            {
                case nameof(reportId):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(name):
                case nameof(surname):
                case nameof(email):
                case nameof(statusMessage):
                case nameof(status):
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
}
