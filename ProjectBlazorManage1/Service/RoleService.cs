namespace ProjectBlazorManage1.Service
{
    public class RoleService : IRoleservice
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<bool> Add(RoleDto roleDto)
        {
            var identityRole = new IdentityRole
            {
                Name = roleDto.Name,
                NormalizedName = roleManager.NormalizeKey(roleDto.Name)
            };
            var result = await roleManager.CreateAsync(identityRole);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<bool> Delete(string name)
        {
            var identityRole = await Find(name);
            if (identityRole == null) return false;

            var usersInRole = await userManager.GetUsersInRoleAsync(name);
            if (usersInRole.Count != 0) return false;
            var result = await roleManager.DeleteAsync(identityRole);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<IdentityRole> Find(string name)
        {
            var identityRole = await roleManager.FindByNameAsync(name);
            return identityRole;
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            var result = await roleManager.Roles.ToListAsync();
            return result;
        }

        public async Task<bool> Update(RoleUpdateDto roleUpdateDto)
        {
            var identityRole = await Find(roleUpdateDto.Name);

            if (identityRole == null) return false;

            identityRole.Name = roleUpdateDto.UpdateName;
            identityRole.NormalizedName = roleManager.NormalizeKey(roleUpdateDto.UpdateName);

            var result = await roleManager.UpdateAsync(identityRole);

            if (!result.Succeeded) return false;

            return true;
        }
    }
}
