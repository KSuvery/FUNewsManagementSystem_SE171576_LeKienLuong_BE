using FUNewsManagementSystem.Repo.Base;
using FUNewsManagementSystem.Repo.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Repo.Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount> GetSystemAccount(string email, string password);
        Task<SystemAccount> GetSystemAccountByEmail(string email);
        Task<SystemAccount> GetProfile(int id);
        void CreateSystemAccount(SystemAccount account);
    }
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(NewsManagementDBContext context) : base(context)
        {
        }

        public async Task<SystemAccount> GetSystemAccount(string email, string password)
        {
            return await _context.SystemAccount.FirstOrDefaultAsync(s => s.AccountEmail == email && s.AccountPassword == password);
        }

        public async void CreateSystemAccount(SystemAccount account)
        {
            await CreateAsync(account);
        }

        public async Task<SystemAccount> GetSystemAccountByEmail(string email)
        {
            return await _context.SystemAccount.FirstOrDefaultAsync(s => s.AccountEmail == email);
        }

        public async Task<SystemAccount> GetProfile(int id)
        {
            return await _context.SystemAccount.FirstOrDefaultAsync(s => s.AccountId == id);
        }
    }
}
