namespace _01_UserLogin
{
    public interface IAdministration
    {
        public User CurrentUser(string token);
        public void Rename(string userId, string email, string nickname);
        public void ChangePassword(string userId, string password);
        public void Delete(string userId, string password);
    }
}
