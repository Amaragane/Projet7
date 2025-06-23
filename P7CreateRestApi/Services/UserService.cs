using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return ServiceResult<IEnumerable<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<User>>.Failure($"Erreur lors de la récupération des utilisateurs: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> GetUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<User>.Failure("L'ID doit être supérieur à 0");

                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return ServiceResult<User>.Failure("Utilisateur non trouvé");

                return ServiceResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.Failure($"Erreur lors de la récupération de l'utilisateur: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> GetUserByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    return ServiceResult<User>.Failure("Le nom d'utilisateur est requis");

                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null)
                    return ServiceResult<User>.Failure("Utilisateur non trouvé");

                return ServiceResult<User>.Success(user);
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.Failure($"Erreur lors de la recherche de l'utilisateur: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> CreateUserAsync(User user)
        {
            try
            {

                // Hash du mot de passe (à implémenter avec BCrypt ou similaire)
                user.Password = HashPassword(user.Password);

                var createdUser = await _userRepository.CreateAsync(user);
                return ServiceResult<User>.Success(createdUser, "Utilisateur créé avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.Failure($"Erreur lors de la création de l'utilisateur: {ex.Message}");
            }
        }

        public async Task<ServiceResult<User>> UpdateUserAsync(int id, User user)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<User>.Failure("L'ID doit être supérieur à 0");

                if (!await _userRepository.ExistsAsync(id))
                    return ServiceResult<User>.Failure("Utilisateur non trouvé");

                user.Id = id;

                var updatedUser = await _userRepository.UpdateAsync(user);
                return ServiceResult<User>.Success(updatedUser, "Utilisateur mis à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<User>.Failure($"Erreur lors de la mise à jour de l'utilisateur: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<bool>.Failure("L'ID doit être supérieur à 0");

                if (!await _userRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Utilisateur non trouvé");

                var deleted = await _userRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Utilisateur supprimé avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression de l'utilisateur: {ex.Message}");
            }
        }

        private bool IsValidRole(string role)
        {
            var validRoles = new[] { "Admin", "User", "Trader" };
            return validRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        private string HashPassword(string password)
        {
            // TODO: Implémenter un vrai hash avec BCrypt
            // Pour l'instant, simulation simple
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // TODO: Implémenter la vérification avec BCrypt
            // Pour l'instant, simulation simple
            string hashedInput = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            return hashedInput == hashedPassword;
        }
    }
    }
