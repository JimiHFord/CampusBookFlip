using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CampusBookFlip.WebUI.Abstract
{
    public interface ICBFSecurity
    {
        bool ChangePassword(string userName, string currentPassword, string newPassword);

        bool ConfirmAccount(string accountConfirmationToken);

        string CreateAccount(string userName, string password, bool requireConfirmationToken);

        string CreateUserAndAccount(string userName, string password, object propertyValues, bool requireConfirmationToken);

        int CurrentUserId { get; }

        int GetUserId(string userName);

        string CurrentUserName { get; }

        string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440);

        int GetUserIdFromPasswordResetToken(string passwordResetToken);

        bool Login(string userName, string password, bool persistCookie = false);

        void Logout();

        bool ResetPassword(string passwordResetToken, string newPassword);

        bool UserExists(string userName);

        string NewToken { get; }

    }
}