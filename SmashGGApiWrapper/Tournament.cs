using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashGGApiWrapper
{
    public class Tournament
    {
        public int id { get; set; }
        public object seriesId { get; set; }
        public int ownerId { get; set; }
        public int state { get; set; }
        public int progressMeter { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string shortSlug { get; set; }
        public object venueFee { get; set; }
        public int processingFee { get; set; }
        public object tiebreakOrder { get; set; }
        public string timezone { get; set; }
        public bool @private { get; set; }
        public bool published { get; set; }
        public bool testMode { get; set; }
        public bool publicSeeding { get; set; }
        public int tournamentType { get; set; }
        public bool hasOnlineEvents { get; set; }
        public bool includeQRCode { get; set; }
        public bool approved { get; set; }
        public int startAt { get; set; }
        public int endAt { get; set; }
        public object startedAt { get; set; }
        public object completedAt { get; set; }
        public int registrationClosesAt { get; set; }
        public int eventRegistrationClosesAt { get; set; }
        public int teamCreationClosesAt { get; set; }
        public string stripeMode { get; set; }
        public object paypalMode { get; set; }
        public string paypalApp { get; set; }
        public string city { get; set; }
        public string addrState { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string mapsPlaceId { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        //public Links links { get; set; }
        public string venueName { get; set; }
        public string venueAddress { get; set; }
        public string region { get; set; }
        public string hashtag { get; set; }
        public object showCity { get; set; }
        public int attendeeLocationInfo { get; set; }
        public int attendeeContactInfo { get; set; }
        //public AttendeeFieldConfig attendeeFieldConfig { get; set; }
        public object attendeeRequirements { get; set; }
        public bool notifyAdmins { get; set; }
        public bool publicAttendees { get; set; }
        public bool hideAdmins { get; set; }
        public string videoUrl { get; set; }
        public string details { get; set; }
        public string registrationMarkdown { get; set; }
        public object gettingThere { get; set; }
        public string prizes { get; set; }
        public string rules { get; set; }
        public object contactInfo { get; set; }
        public bool includeInstructions { get; set; }
        public object emailInstructions { get; set; }
        public bool includeDirections { get; set; }
        public object emailDirections { get; set; }
        public bool includeMap { get; set; }
        public object qrCodeRedirect { get; set; }
        public object emailNote { get; set; }
        public bool includeNote { get; set; }
        public string contactEmail { get; set; }
        public string contactTwitter { get; set; }
        public object contactPhone { get; set; }
        public string currency { get; set; }
        public object onsitePaymentMode { get; set; }
        public string stripePublishableKey { get; set; }
        public object paypalPayerId { get; set; }
        public object customEmailText { get; set; }
        //public RegistrationOptions registrationOptions { get; set; }
        //public LimitsByType limitsByType { get; set; }
        //public Publishing publishing { get; set; }
        public List<object> trackingPixels { get; set; }
        public object generatedTabs { get; set; }
        public string defaultTab { get; set; }
        public List<object> fees { get; set; }
        public List<object> customMarkdown { get; set; }
        //public List<Image> images { get; set; }
        public int scheduleId { get; set; }
        public List<string> expand { get; set; }
        public List<object> userData { get; set; }
        public List<int> eventIds { get; set; }
        public string regionDisplayName { get; set; }
        public List<object> onlineEvents { get; set; }
        public List<object> taskEvents { get; set; }
        public List<string> slugs { get; set; }
        public string permissionType { get; set; }
        public bool supportsPayPal { get; set; }
    }
}
