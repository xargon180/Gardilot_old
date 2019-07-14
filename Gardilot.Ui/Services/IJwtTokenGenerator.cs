namespace Gardilot.Ui.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateJSONWebToken();
        bool ValidateGenerateJSONWebToken(string token);
    }
}
