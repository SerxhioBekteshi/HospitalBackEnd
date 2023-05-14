using Shared.ResponseFeatures;
using Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTC
{
    public class DelayColumn
    {
        //public static string id = "Id";
        //public static string staffId = "Id e mjekut";
        public static string staffName = "Emri i mjekut";
        public static string count = "Numri i vonesa";
        public static string actions = null;

        public static string GetPropertyDescription(string propertyName)
        {
            switch (propertyName)
            {
                //case nameof(id):
                //    return id;
                //case nameof(staffId):
                //    return staffId;
                case nameof(staffName):
                    return staffName;
                case nameof(count):
                    return count;
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
                //case nameof(id):
                //case nameof(staffId):
                //    return true;
                case nameof(staffName):
                case nameof(count):
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
                //case nameof(id):
                //case nameof(staffId):
                    //return false;
                case nameof(staffName):
                case nameof(count):
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
                //case nameof(id):
                //case nameof(staffId):
                    //return false;
                case nameof(staffName):
                case nameof(count):
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
                //case nameof(id):
                //case nameof(staffId):
                    //return 50;
                case nameof(staffName):
                case nameof(count):
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
                //case nameof(id):
                //case nameof(staffId):
                //    return DataType.Number;
                case nameof(staffName):
                    return DataType.String;
                case nameof(count):
                    return DataType.Number;
                case nameof(actions):
                    return DataType.Actions;
                default:
                    return DataType.String;
            }
        }

        public static object[] GetPropertyActions(string propertyName)
        {


            switch (propertyName)
            {
                //case nameof(id):
                //case nameof(staffId):
                case nameof(staffName):
                case nameof(count):
                    return null;
                case nameof(actions):
                    return null;
                default:
                    return null;
            }
        }
    }
}
