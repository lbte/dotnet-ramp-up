namespace PRFTLatam.Workshop.Services;

public interface IIdentityService
{
    List<string> GetIds();
    List<string> IdentityValidation(List<string> ids); 
    List<string> AmountOfIdsIdentityValidation(int amount);
}