using System;
using CredentialManagement;

namespace BadrAcademy.Helpers
{
    internal class clsCredentials
    {
        //Handle User Credentials
        private const string CredTarget = "MedFinance.StoredUserCredentials";
        public static bool RememberUsernameAndPassword(string username, string password)
        {
            if (username == null || password == null)
            {
                DeleteStoredCredentials();
                return false;
            }

            using (Credential cred = new Credential())
            {
                cred.Target = CredTarget;
                cred.Username = username;
                cred.Password = password;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                return cred.Save();
            }
        }
        public static bool GetStoredCredentials(out string username, out string password)
        {
            bool isLoadSuccessfully = false;
            username = null;
            password = null;

            using (Credential cred = new Credential())
            {
                cred.Target = CredTarget;
                isLoadSuccessfully = cred.Load();
                if (isLoadSuccessfully)
                {
                    username = cred.Username;
                    password = cred.Password;
                }
            }

            return isLoadSuccessfully;
        }
        
        private static void DeleteStoredCredentials()
        {
            using (var cred = new Credential { Target = CredTarget })
            {
                cred.Delete();
            }
        }

    }
}
