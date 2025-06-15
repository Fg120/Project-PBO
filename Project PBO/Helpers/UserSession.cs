using Project_PBO.Models;

namespace Project_PBO.Helpers
{
    public static class UserSession
    {
        private static AkunModel? _currentUser;

        public static AkunModel? CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static bool IsLoggedIn
        {
            get { return _currentUser != null; }
        }

        public static string Username
        {
            get { return _currentUser?.Username ?? string.Empty; }
        }

        public static string Role
        {
            get { return _currentUser?.Role ?? string.Empty; }
        }

        public static int IdAkun
        {
            get { return _currentUser?.IdAkun ?? 0; }
        }

        public static void Logout()
        {
            _currentUser = null;
        }

        public static bool IsAdmin()
        {
            return Role.ToLower() == "admin";
        }

        public static bool IsPetani()
        {
            return Role.ToLower() == "petani";
        }
    }
}
