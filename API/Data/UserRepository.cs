namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}