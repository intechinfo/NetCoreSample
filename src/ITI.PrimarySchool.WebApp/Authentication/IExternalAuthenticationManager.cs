using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ITI.PrimarySchool.WebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        User FindUser( OAuthCreatingTicketContext context );
    }
}