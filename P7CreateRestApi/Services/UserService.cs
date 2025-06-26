using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.DTO.Maping;
using P7CreateRestApi.DTO.UsersDTO;
using P7CreateRestApi.Services.Interfaces;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService>? _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService>? logger = null)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public  ServiceResult<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                _logger?.LogInformation("Getting all users");

                var users = _userManager.Users.ToList();
                var userDTOs = users.Select(user => UserDTOMappings.ToUserDTO(user)); // Specify the type explicitly  

                return ServiceResult<IEnumerable<UserDTO>>.Success(userDTOs);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all users");
                return ServiceResult<IEnumerable<UserDTO>>.Failure("Erreur lors de la récupération des utilisateurs");
            }
        }

        public async Task<ServiceResult<UserDTO>> GetUserByIdAsync(int id)
        {
            try
            {
                _logger?.LogInformation("Getting user with ID: {UserId}", id);

                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return ServiceResult<UserDTO>.Failure("Utilisateur non trouvé");
                }

                var userDTO = UserDTOMappings.ToUserDTO(user);
                return ServiceResult<UserDTO>.Success(userDTO);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting user {UserId}", id);
                return ServiceResult<UserDTO>.Failure("Erreur lors de la récupération de l'utilisateur");
            }
        }

        public async Task<ServiceResult<UserDTO>> CreateUserAsync(CreateUserDTO createUser)
        {
            try
            {
                _logger?.LogInformation("Creating new user: {Email}", createUser.Email);

                // Vérifier si l'email existe déjà
                var existingUserByEmail = await _userManager.FindByEmailAsync(createUser.Email);
                if (existingUserByEmail != null)
                {
                    return ServiceResult<UserDTO>.Failure("Un utilisateur avec cet email existe déjà");
                }

                // Vérifier si le username existe déjà
                var existingUserByUsername = await _userManager.FindByNameAsync(createUser.Username);
                if (existingUserByUsername != null)
                {
                    return ServiceResult<UserDTO>.Failure("Ce nom d'utilisateur est déjà pris");
                }

                // Créer le nouvel utilisateur
                var newUser = new User
                {
                    UserName = createUser.Username,
                    Email = createUser.Email,
                    Fullname = createUser.Fullname,
                    PhoneNumber = createUser.PhoneNumber,
                    Roles = createUser.Roles,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Créer avec UserManager (gère le hachage du mot de passe)
                var result = await _userManager.CreateAsync(newUser, createUser.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger?.LogWarning("Failed to create user {Email}: {Errors}",
                        createUser.Email, string.Join(", ", errors));
                    return ServiceResult<UserDTO>.Failure(errors);
                }

                _logger?.LogInformation("User created successfully: {Email}", createUser.Email);
                var userDTO = UserDTOMappings.ToUserDTO(newUser);
                return ServiceResult<UserDTO>.Success(userDTO);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating user {Email}", createUser.Email);
                return ServiceResult<UserDTO>.Failure("Erreur lors de la création de l'utilisateur");
            }
        }

        public async Task<ServiceResult<UserDTO>> UpdateUserAsync(int id, UpdateUserDTO updateUser)
        {
            try
            {
                _logger?.LogInformation("Updating user: {UserId}", id);

                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return ServiceResult<UserDTO>.Failure("Utilisateur non trouvé");
                }

                // Vérifier l'unicité de l'email si changé
                if (user.Email != updateUser.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(updateUser.Email);
                    if (existingUser != null)
                    {
                        return ServiceResult<UserDTO>.Failure("Un utilisateur avec cet email existe déjà");
                    }
                }

                // Vérifier l'unicité du username si changé
                if (user.UserName != updateUser.Username)
                {
                    var existingUser = await _userManager.FindByNameAsync(updateUser.Username);
                    if (existingUser != null)
                    {
                        return ServiceResult<UserDTO>.Failure("Ce nom d'utilisateur est déjà pris");
                    }
                }

                // Mettre à jour les propriétés
                user.UserName = updateUser.Username;
                user.Email = updateUser.Email;
                user.Fullname = updateUser.Fullname;
                user.PhoneNumber = updateUser.PhoneNumber;
                user.EmailConfirmed = updateUser.EmailConfirmed;
                user.IsActive = updateUser.IsActive;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<UserDTO>.Failure(errors);
                }

                _logger?.LogInformation("User updated successfully: {UserId}", id);
                var userDTO = UserDTOMappings.ToUserDTO(user);
                return ServiceResult<UserDTO>.Success(userDTO);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating user {UserId}", id);
                return ServiceResult<UserDTO>.Failure("Erreur lors de la mise à jour de l'utilisateur");
            }
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                _logger?.LogInformation("Deleting user: {UserId}", id);

                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return ServiceResult<bool>.Failure("Utilisateur non trouvé");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<bool>.Failure(errors);
                }

                _logger?.LogInformation("User deleted successfully: {UserId}", id);
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting user {UserId}", id);
                return ServiceResult<bool>.Failure("Erreur lors de la suppression de l'utilisateur");
            }
        }

    }
}