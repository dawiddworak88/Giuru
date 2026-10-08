namespace Client.Api.ServicesModels.DiscountCodes
{
    public enum DiscountCodeValidationStatus
    {
        Valid,
        NotFound,
        Disabled,
        NotAssigned,
        ClientUnknown
    }
}
