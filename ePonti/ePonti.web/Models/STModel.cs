using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ePonti.web.Models
{
    public class STModel
    {
    }
    public class STSessionModel
    {
        public int fileId { get; set; }
        public string fileName { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastUpdate { get; set; }
        public decimal? version { get; set; }
        public string apiId { get; set; }    }

    public class STHostModel
    {
        public int hostId { get; set; }
        public string hostName { get; set; }
    }

    public class STFolderModel
    {
        public int folderId { get; set; }
        public string folderName { get; set; }
    }

    public class STFileModel
    {
        public int fileId { get; set; }
        public string fileName { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastUpdate { get; set; }
        public decimal version { get; set; }
    }

    public class STClientModel
    {
        public string version { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string companyWebsite { get; set; }
        public string contractorName { get; set; }
        public string contractorEmail { get; set; }
        public string contractorPhone { get; set; }
        public string builder { get; set; }
        public string community { get; set; }
        public string otherField { get; set; }
        public string otherFieldVal { get; set; }
        public string emailAddress { get; set; }
        public string mobilePhone { get; set; }
        public string lotNumber { get; set; }
        public HomeAddress homeAddress { get; set; }
        public WorkAddress workAddress { get; set; }
        public string fileName { get; set; }
        public DateTime createDate { get; set; }
        public DateTime lastUpdate { get; set; }
    }

    public class STAdjustmentModel
    {
        public string equipment { get; set; }
        public string equipmentLabel { get; set; }
        public string labor { get; set; }
        public string laborLabel { get; set; }
        public string otherDiscountLabel { get; set; }
        public string otherDiscountValue { get; set; }
        public string discountType { get; set; }
        public string useCostInsteadOfEquipment { get; set; }
    }
    public class STTaxModel
    {
        public string label { get; set; }
        public string value { get; set; }
        public string applyType { get; set; }
        public string discountType { get; set; }
    }
    public class STTaxProfileModel
    {
        public string taxName { get; set; }
        public List<STTaxModel> taxes { get; set; }
    }

    public class STScheduleModel
    {
        public string name { get; set; }
        public string value { get; set; }
        public string discountType { get; set; }
    }

    public class STPackageModel
    {
        public decimal version { get; set; }
        public string packageGroupName { get; set; }
        public string packageName { get; set; }
        public string manufacturer { get; set; }
        public string vendor { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string phase { get; set; }
        public string taxable { get; set; }
        public decimal? quantity { get; set; }
        public decimal? cost { get; set; }
        public decimal? equipment { get; set; }
        public decimal? labor1Price { get; set; }
        public decimal? labor1Cost { get; set; }
        public decimal? labor1Hours { get; set; }
        public decimal? labor1HourlyRate { get; set; }
        public decimal? labor1HourlyRateCost { get; set; }
        public decimal? labor2Price { get; set; }
        public decimal? labor2Cost { get; set; }
        public decimal? labor2Hours { get; set; }
        public decimal? labor2HourlyRate { get; set; }
        public decimal? labor2HourlyRateCost { get; set; }
        public int? id { get; set; }
        public List<STRoomsQuantityModel> roomsQuantity { get; set; }
    }
    public class STRoomsQuantityModel
    {
        public decimal quantity { get; set; }
        public string id { get; set; }
    }
    public class STRoomsModel
    {
        public string roomName { get; set; }
        public string id { get; set; }
        public string position { get; set; }
    }
    public class STPackageGroupsModel
    {
        public string packageGroupName { get; set; }
        public decimal version { get; set; }
        public string fileName { get; set; }
        public int position { get; set; }
        public List<string> gridColor { get; set; }
        public List<string> buttonColor { get; set; }
    }
    public class STPackageContentsModel
    {
        public string packageName { get; set; }
        public decimal version { get; set; }
        public List<decimal> prices { get; set; }
        public int labor1 { get; set; }
        public int labor2 { get; set; }
        public int equipment { get; set; }
        public int state { get; set; }
        public int level { get; set; }
        public int order { get; set; }
        public string description { get; set; }
        public string imagePath { get; set; }
        public string iconName { get; set; }
        public string secondaryIconName { get; set; }
        public string packageIdentifier { get; set; }
        public int? optionId { get; set; }
        public string packageType { get; set; }
        public decimal warrantyAmount { get; set; }
        public decimal contractorPrice { get; set; }
        public decimal cost { get; set; }
        public List<int> salesCommissions { get; set; }
        public List<int> productContracts { get; set; }
        public List<string> primaryProducts { get; set; }
        public string hyperlink { get; set; }
        public int stateOption { get; set; }
        public string masterPackageIdentifier { get; set; }
        public int contractorEquipment { get; set; }
        public List<int> contractorLabors { get; set; }
        public int variable { get; set; }
        public List<string> specs { get; set; }
        public string comments { get; set; }
        public decimal manualPrice { get; set; }

    }

    public class STCommentsModel
    {
        public decimal version { get; set; }
        public string comment { get; set; }
        public DateTime dateTime { get; set; }
    }
    public class STRoomModel
    {
        public decimal version { get; set; }
        public string comment { get; set; }
        public DateTime dateTime { get; set; }
    }

    public class STJobInfoModel
    {
        public STJobHeaderModel jobHeader { get; set; }
        public STJobHeaderModel adjustment { get; set; }
        public STTaxProfileModel taxProfile { get; set; }
        public List<STScheduleModel> scheduleProfiles { get; set; }
    }

    public class STJobHeaderModel
    {
        public string jobName { get; set; }
        public string createdDate { get; set; }
        public string workStreet { get; set; }
        public string emailAddress { get; set; }
        public string workCity { get; set; }
        public string workState { get; set; }
        public string workZip { get; set; }
        public string workPhone { get; set; }
    }

    public class SessionInfoModel
    {
        public string version { get; set; }
        public STClientModel clientInfo { get; set; }
        public STAdjustmentModel adjustment { get; set; }
        public STTaxProfileModel taxProfile { get; set; }
        public List<STScheduleModel> scheduleProfiles { get; set; }
        public STCommentsModel comments { get; set; }
        public List<STPackageModel> selectedPackages { get; set; }
        public List<STRoomsModel> roomGroups { get; set; }
    }
}