using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BLL.Common
{
    public class EnumWrapper
    {
        public enum PhoneTypes
        {
            Phone = 1,
            Mobile = 2
        }

        public enum Priority
        {
            Low = 1,
            Medium = 2,
            High = 3
        }

        public enum InviteeTypes
        {
            SiteUser = 1,
            Contact = 2,
        }

        public enum AddressTypes
        {
            Main = 1,
            Billing = 2
        }

        //TODO: check if needed or not
        public enum FromPage
        {
            leadwon
        }

        public enum Pages
        {
            _NONE,//for default values

            [Description("db")] Dashboard,

            [Description("pg")] PeopleGrid,
            [Description("cd")] ContactDetails,

            [Description("cl")] CallDetails,
            [Description("ev")] EventDetails,
            [Description("nt")] NoteDetails,
            [Description("cs")] CaseDetails,
            [Description("pu")] PunchItemDetails,
            [Description("ti")] TimeItDetails,

            [Description("lg")] LeadGrid,
            [Description("ld")] LeadDetails,

            [Description("qg")] QuoteGrid,
            [Description("qd")] QuoteDetails,

            [Description("jg")] JobGrid,
            [Description("jd")] JobDetails,

            [Description("sg")] ServiceGrid,
            [Description("sd")] ServiceDetails,

            [Description("sc")] Scheduling,

            [Description("rg")] PorGrid,
            [Description("pd")] PorDetails,

            [Description("wd")] WorkOrderbDetails,

            [Description("dd")] DeliveryRequestDetails,

            [Description("co")] CorDetails,

            [Description("so")] SoDetails,

            [Description("ca")] CoInfo,

            [Description("tk")] Timekeeper,

            [Description("mc")] mCallDetails,
            [Description("mw")] mWorkOrderDetails

        }

        public enum OptionTypes
        {
            Activities_ActivitiesStatus,
            Activities_PurposesList,
            Activities_PunchListTypes,
            Activities_PunchListDepartments,
            Activities_CaseType,
            Activities_MileStones,
            Activities_MileStonesStatus,
            Activities_MileStonesTemplates,

            People_Types,
            People_Relationship,
            People_SubTypes,

            Lead_NumberFormat,
            Lead_Status,
            Lead_Type,
            Lead_Rating,
            Lead_SalesPhase,
            Lead_Probability,
            Lead_System,

            Quote_NumberFormat,
            Quote_Status,
            Quote_Template,
            Quote_Area,
            Quote_Division,

            Job_NumberFormat,
            Job_Status,
            Job_Template,
            Job_CORStatus,
            Job_CORType,

            Service_NumberFormat,
            Service_Status,
            Service_Template,

            Accounting_SalesOrderStatus,
            Accounting_PORStatus,
            Accounting_DeliveryTypes,
            Accounting_ShippingMethods,
            Accounting_CostCodes,
            Accounting_InventoryTemplates,
            Accounting_CreditMemoStatus,
            Accounting_DeliveryStatus,
            Accounting_RMAStatus,
            Accounting_ShipToList,
            Accounting_CustodyLocations,
            Accounting_Taxes,

            Group_Group,

            Manufacturer_Manufacturer,

            Stage_Master,
            Stage_SubLabor,

            CoAccount_CoInfo,
            CoAccount_PayTypes,
            CoAccount_Payroll,
            CoAccount_Calendar
        }

        public enum WorkOrderTypes
        {
            /// <summary>
            /// WO process started from Job Parts
            /// </summary>
            Scheduled = 1,

            /// <summary>
            /// WO process started from Job Punch List
            /// </summary>
            Punchlist = 2,

            /// <summary>
            /// WO process started from Service Parts
            /// </summary>
            Service = 3,
        }

        public enum SiteUserRoles
        {
            admin = 1,
            user = 2
        }

    }
}
