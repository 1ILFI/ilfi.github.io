using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace iBlazorWebAssembly.Services
{
    public class AdminAuthService
    {
        private readonly ILocalStorageService _localStorage;
        private const string AdminPassKey = "admin_password";
        private const string AdminStateKey = "admin_logged_in";
        private const string DefaultAdminPass = "admin123"; // 默认密码，实际使用时应更改为更强的密码

        public AdminAuthService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            return await _localStorage.ContainKeyAsync(AdminStateKey) && 
                   await _localStorage.GetItemAsync<bool>(AdminStateKey);
        }

        public async Task<bool> LoginAsync(string password)
        {
            if (!await _localStorage.ContainKeyAsync(AdminPassKey))
            {
                // 首次使用，设置默认管理员密码
                await _localStorage.SetItemAsync(AdminPassKey, DefaultAdminPass);
            }

            var storedPassword = await _localStorage.GetItemAsync<string>(AdminPassKey);
            
            if (storedPassword == password)
            {
                await _localStorage.SetItemAsync(AdminStateKey, true);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.SetItemAsync(AdminStateKey, false);
        }

        public async Task ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var storedPassword = await _localStorage.GetItemAsync<string>(AdminPassKey);

            if (storedPassword == oldPassword)
            {
                await _localStorage.SetItemAsync(AdminPassKey, newPassword);
                return;
            }

            throw new System.Exception("旧密码不正确");
        }
    }
}