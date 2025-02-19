using Newtonsoft.Json;

namespace Buyer.Web.Shared.GraphQlResponseModels.PersonalDataAdministrator
{
    public class PersonalDataAdministratorGraphQlResponseModel
    {
        [JsonProperty("globalConfiguration")]
        public PersonalDataAdministratorComponent Component { get; set; }
    }

    public class PersonalDataAdministratorComponent
    {
        [JsonProperty("data")]
        public PersonalDataAdministratorData Data { get; set; }
    }

    public class PersonalDataAdministratorData
    {
        [JsonProperty("attributes")]
        public PersonalDataAdministratorAttributes Attributes { get; set; }
    }

    public class PersonalDataAdministratorAttributes
    {
        [JsonProperty("personalDataAdministrator")]
        public string PersonalDataAdministrator { get; set; }
    }
}
