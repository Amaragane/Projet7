using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTO.AuthDTO;
using P7CreateRestApi.DTO.UsersDTO;

namespace P7CreateRestApi.DTO.Maping
{
    public static class UserDTOMappings
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Username = user.UserName ?? "",
                Fullname = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                AccessFailedCount = user.AccessFailedCount
            };
        }


        public static User ToUserEntity(this CreateUserDTO createUserDTO)
        {
            return new User
            {
                UserName = createUserDTO.Username,
                Email = createUserDTO.Email,
                Fullname = createUserDTO.Fullname,
                PhoneNumber = createUserDTO.PhoneNumber,
                EmailConfirmed = createUserDTO.EmailConfirmed,
                IsActive = createUserDTO.IsActive,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateFromDTO(this User user, UpdateUserDTO updateUserDTO)
        {
            user.UserName = updateUserDTO.Username;
            user.Email = updateUserDTO.Email;
            user.Fullname = updateUserDTO.Fullname;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.EmailConfirmed = updateUserDTO.EmailConfirmed;
            user.PhoneNumberConfirmed = updateUserDTO.PhoneNumberConfirmed;
            user.IsActive = updateUserDTO.IsActive;
        }
    }
}
