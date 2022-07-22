using System.Threading.Tasks;
using CheckPlease.Auth.Models;

namespace CheckPlease.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}