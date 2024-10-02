using UserAuthentication.Models;

namespace UserAuthentication.DataService
{
    public class UserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUserWithRelatedData(User user, UserLogin userLogin, UserLoginDataExternal userLoginDataExternal, 
            long roleId)
        {

        }
    }
}
