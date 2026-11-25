namespace ProjectBlazorManage1.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task<List<UserRoleViewModel>> GetAllUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var list = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                list.Add(new UserRoleViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                  
                    CurrentRoles = string.Join(", ", userRoles),
                    SelectedRole = userRoles.FirstOrDefault(),
                    Roles = roles,
                });
            }

            return list;
        }

        public async Task<bool> ChangeUserRoleAsync(string userName, string newRole)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return false;

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            return addResult.Succeeded;
        }

       
    }
}
