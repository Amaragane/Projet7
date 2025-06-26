using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public LocalDbContext _context { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Version synchrone pour compatibilité avec l'existant
        /// </summary>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // ====================================================================
        // READ - Lire/Récupérer des utilisateurs
        // ====================================================================

        /// <summary>
        /// Récupère tous les utilisateurs
        /// </summary>
        /// <returns>Liste de tous les utilisateurs</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        /// <summary>
        /// Version synchrone pour compatibilité
        /// </summary>
        public async Task<List<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Récupère un utilisateur par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <returns>Utilisateur trouvé ou null</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Version synchrone pour compatibilité
        /// </summary>
        public User? FindById(int id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// Récupère un utilisateur par son nom d'utilisateur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <returns>Utilisateur trouvé ou null</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Version synchrone pour compatibilité
        /// </summary>
        public User? FindByUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return _context.Users
                .FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }


        /// <summary>
        /// Recherche des utilisateurs par nom complet (partiel)
        /// </summary>
        /// <param name="searchTerm">Terme de recherche</param>
        /// <returns>Liste des utilisateurs correspondants</returns>
        public async Task<IEnumerable<User>> SearchByFullnameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Users
                .Where(u => u.Fullname.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(u => u.Fullname)
                .ToListAsync();
        }

        // ====================================================================
        // UPDATE - Mettre à jour un utilisateur
        // ====================================================================

        /// <summary>
        /// Met à jour un utilisateur existant
        /// </summary>
        /// <param name="user">Utilisateur avec les nouvelles données</param>
        /// <returns>Utilisateur mis à jour</returns>
        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Vérifier que l'utilisateur existe
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException($"Utilisateur avec l'ID {user.Id} non trouvé");

            // Mettre à jour les propriétés
            existingUser.UserName = user.UserName;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Fullname = user.Fullname;
            existingUser.Roles = user.Roles;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Met à jour partiellement un utilisateur (sans le mot de passe)
        /// </summary>
        /// <param name="user">Utilisateur avec les nouvelles données</param>
        /// <returns>Utilisateur mis à jour</returns>
        public async Task<User> UpdateWithoutPasswordAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException($"Utilisateur avec l'ID {user.Id} non trouvé");

            // Mettre à jour sans le mot de passe
            existingUser.UserName = user.UserName;
            existingUser.Fullname = user.Fullname;
            existingUser.Roles = user.Roles;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Met à jour uniquement le mot de passe d'un utilisateur
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="newPassword">Nouveau mot de passe (déjà hashé)</param>
        /// <returns>True si mis à jour avec succès</returns>
        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Le mot de passe ne peut pas être vide", nameof(newPassword));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordHash = newPassword;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        // ====================================================================
        // DELETE - Supprimer un utilisateur
        // ====================================================================

        /// <summary>
        /// Supprime un utilisateur par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur à supprimer</param>
        /// <returns>True si supprimé avec succès, False si non trouvé</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Supprime un utilisateur par son nom d'utilisateur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <returns>True si supprimé avec succès</returns>
        public async Task<bool> DeleteByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            var user = await GetByUsernameAsync(username);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // ====================================================================
        // EXISTENCE - Vérifier l'existence
        // ====================================================================

        /// <summary>
        /// Vérifie si un utilisateur existe par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <returns>True si l'utilisateur existe</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id.ToString());
        }

        /// <summary>
        /// Vérifie si un nom d'utilisateur est déjà utilisé
        /// </summary>
        /// <param name="username">Nom d'utilisateur à vérifier</param>
        /// <param name="excludeUserId">ID utilisateur à exclure (pour les mises à jour)</param>
        /// <returns>True si le nom d'utilisateur existe déjà</returns>
        public async Task<bool> UsernameExistsAsync(string username, int? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            var query = _context.Users.Where(u => u.UserName.ToLower() == username.ToLower());

            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.Id != excludeUserId.Value.ToString());
            }

            return await query.AnyAsync();
        }
    }
   }