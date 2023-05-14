using Shared.ResponseFeatures;
using Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTC
{
    public class ReservationTableColumn
    {
        public static string id = "Id";
        public static string serviceName = "Emri i sherbimit";
        public static string startTime = "Start Time";
        public static string endTime = "End Time";
        public static string phoneNumber = "Numri i telefonit";
        public static string clientId = "Client ID";
        public static string clientName = "Emri i klientit";
        public static string email = "Emaili";
        public static string status = "Statusi i rezervimit";
        public static string actions = null;

        public static string GetPropertyDescription(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(id):
                    return id;
                case nameof(serviceName):
                    return serviceName;
                case nameof(startTime):
                    return startTime;
                case nameof(endTime):
                    return endTime;
                case nameof(phoneNumber):
                    return phoneNumber;
                case nameof(clientId):
                    return clientId;
                case nameof(clientName):
                    return clientName;
                case nameof(email):
                    return email;
                case nameof(status):
                    return status;
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
                case nameof(serviceName):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
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
                case nameof(serviceName):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
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
                    return false;
                case nameof(serviceName):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
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
                    return 50;
                case nameof(serviceName):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
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
                case nameof(startTime):
                case nameof(endTime):
                    return DataType.DateTime;
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
                case nameof(serviceName):
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
                new { name = "edit", icon= "fa-regular fa-pen-to-square", color = "blue" },
                new { name = "insert", icon= "fa-solid fa-plus", color = "green" },
            };


            switch (propertyName)
            {
                case nameof(id):
                case nameof(serviceName):
                case nameof(startTime):
                case nameof(endTime):
                case nameof(phoneNumber):
                case nameof(clientId):
                case nameof(clientName):
                case nameof(email):
                case nameof(status):
                    return null;
                case nameof(actions):
                    return actionsData;
                default:
                    return actionsData;
            }
        }
    }
}
