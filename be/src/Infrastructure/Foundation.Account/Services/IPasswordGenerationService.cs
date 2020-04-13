namespace Foundation.Account.Services
{
    public interface IPasswordGenerationService
    {
        string GeneratePassword(int minlength);
    }
}
