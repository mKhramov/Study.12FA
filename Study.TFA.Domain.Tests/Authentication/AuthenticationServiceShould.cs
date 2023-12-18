using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using Study.TFA.Domain.Authentication;
using System.Reflection.PortableExecutable;

namespace Study.TFA.Domain.Tests.Authentication
{
    public class AuthenticationServiceShould
    {     
        private readonly AuthenticationService sut;
        private readonly Mock<IAuthenticationStorage> storage;
        private readonly ISetup<IAuthenticationStorage, Task<RecognisedUser?>> findUserSetup;
        private readonly Mock<IOptions<AuthenticationConfiguration>> options;

        public AuthenticationServiceShould()
        {
            storage = new Mock<IAuthenticationStorage>();
            findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
            
            var securityManager = new Mock<ISecurityManager>();
            securityManager
                .Setup(s => s.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            options = new Mock<IOptions<AuthenticationConfiguration>>();
            options
                .Setup(o => o.Value)
                .Returns(new AuthenticationConfiguration()
                {
                    Key = "QkEeenXpHqgP6t0WwpUetAFvUUZiMb4f",
                    Iv = "dtEzMsz2ogg="
                });

            sut = new AuthenticationService(storage.Object, securityManager.Object, options.Object);
        }

        [Fact]
        public async Task ReturnSuccess_WhenUserFound()
        {
            findUserSetup.ReturnsAsync(new RecognisedUser()
            { 
                Salt = "bGl0dGxlIHN0cmluZw==",
                PasswordHash = "dmVyeSBsb25nIHN0cmluZyB2ZXJ5IGxvbmcgc3RyaW5nIHZlcnkgbG9uZyBzdHJpbmcgdmVyeSBsb25nIHN0cmluZyB2ZXJ5IGxvbmcgc3RyaW5nIHZlcnkgbG9uZyBzdHJpbmcgdmVyeSBsb25nIHN0cmluZyB2ZXJ5IGxvbmcgc3RyaW5nIHZlcnkgbG9uZyBzdHJpbmcgdmVyeSBsb25nIHN0cmluZyB2ZXJ5IGxvbmcgc3RyaW5nIHZlcnkgbG9uZyBzdHJpbmcg",
                UserId = Guid.Parse("A23C361A-34DD-4FF9-AE7B-5586FC5A2662")
            });

            var (success, authToken) = await sut.SingIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
            success.Should().BeTrue();
            authToken.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AuthenticateUser_AftetTheySignIn() 
        {
            var userId = Guid.Parse("D32C361A-34DD-4FF9-AE7B-5586FC5A2662");
            findUserSetup.ReturnsAsync(new RecognisedUser() { UserId = userId });
            var (_, authToken) = await sut.SingIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);

            var identity = await sut.Authenticate(authToken, CancellationToken.None);
            identity.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task SignInUser_WhenPasswordMatches()
        {
            var password = "qwerty";
            var securityManager = new SecurityManager();
            var (salt, hash) = securityManager.GeneratePasswordParts(password);

            findUserSetup.ReturnsAsync(new RecognisedUser()
            {
                UserId = Guid.Parse("F99C361A-34DD-4FF9-AE7B-5586FC5A2662"),
                PasswordHash = hash,
                Salt = salt,
            }); 

            var localSut = new AuthenticationService(storage.Object, securityManager, options.Object);
            var (success, _) = await localSut.SingIn(new BasicSignInCredentials("User", password), CancellationToken.None);
            success.Should().BeTrue();
        }
    }
}
