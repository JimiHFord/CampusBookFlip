using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace CampusBookFlip.WebUI.Concrete
{
    public class CBFSecurity : ICBFSecurity
    {
        private IRepository repo;

        public CBFSecurity(IRepository repo)
        {
            this.repo = repo;
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        public bool ConfirmAccount(string accountConfirmationToken)
        {
            return WebSecurity.ConfirmAccount(accountConfirmationToken) || repo.ConfirmAccount(accountConfirmationToken);
        }

        public string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            return WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues, bool requireConfirmationToken)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }

        public int CurrentUserId
        {
            get
            {
                return WebSecurity.CurrentUserId;
            }
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public string CurrentUserName
        {
            get
            {
                return WebSecurity.CurrentUserName;
            }
        }

        public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
        {
            return WebSecurity.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow);
        }

        public int GetUserIdFromPasswordResetToken(string passwordResetToken)
        {
            return WebSecurity.GetUserIdFromPasswordResetToken(passwordResetToken);
        }


        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }

        public bool UserExists(string userName)
        {
            return WebSecurity.UserExists(userName);
        }

        public string NewToken
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}