
namespace Study.TFA.Domain.Authentication
{
    internal interface ISecurityManager
    {
        bool ComparePasswords(string password, string salt, string hash);

        (string Salt, string Hash) GeneratePasswordParts(string password);
    }
}
