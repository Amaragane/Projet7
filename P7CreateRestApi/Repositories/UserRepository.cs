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
        /// Version synchrone pour compatibilit� avec l'existant
        /// </summary>
        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // ====================================================================
        // READ - Lire/R�cup�rer des utilisateurs
        // ====================================================================

        /// <summary>
        /// R�cup�re tous les utilisateurs
        /// </summary>
        /// <returns>Liste de tous les utilisateurs</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        /// <summary>
        /// Version synchrone pour compatibilit�
        /// </summary>
        public async Task<List<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// R�cup�re un utilisateur par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <returns>Utilisateur trouv� ou null</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Version synchrone pour compatibilit�
        /// </summary>
        public User? FindById(int id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// R�cup�re un utilisateur par son nom d'utilisateur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <returns>Utilisateur trouv� ou null</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Version synchrone pour compatibilit�
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
        // UPDATE - Mettre � jour un utilisateur
        // ====================================================================

        /// <summary>
        /// Met � jour un utilisateur existant
        /// </summary>
        /// <param name="user">Utilisateur avec les nouvelles donn�es</param>
        /// <returns>Utilisateur mis � jour</returns>
        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // V�rifier que l'utilisateur existe
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException($"Utilisateur avec l'ID {user.Id} non trouv�");

            // Mettre � jour les propri�t�s
            existingUser.UserName = user.UserName;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Fullname = user.Fullname;
            existingUser.Roles = user.Roles;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Met � jour partiellement un utilisateur (sans le mot de passe)
        /// </summary>
        /// <param name="user">Utilisateur avec les nouvelles donn�es</param>
        /// <returns>Utilisateur mis � jour</returns>
        public async Task<User> UpdateWithoutPasswordAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException($"Utilisateur avec l'ID {user.Id} non trouv�");

            // Mettre � jour sans le mot de passe
            existingUser.UserName = user.UserName;
            existingUser.Fullname = user.Fullname;
            existingUser.Roles = user.Roles;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingUser;
        }

        /// <summary>
        /// Met � jour uniquement le mot de passe d'un utilisateur
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="newPassword">Nouveau mot de passe (d�j� hash�)</param>
        /// <returns>True si mis � jour avec succ�s</returns>
        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Le mot de passe ne peut pas �tre vide", nameof(newPassword));

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
        /// <param name="id">ID de l'utilisateur � supprimer</param>
        /// <returns>True si supprim� avec succ�s, False si non trouv�</returns>
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
        /// <returns>True si supprim� avec succ�s</returns>
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
        // EXISTENCE - V�rifier l'existence
        // ====================================================================

        /// <summary>
        /// V�rifie si un utilisateur existe par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <returns>True si l'utilisateur existe</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id.ToString());
        }

        /// <summary>
        /// V�rifie si un nom d'utilisateur est d�j� utilis�
        /// </summary>
        /// <param name="username">Nom d'utilisateur � v�rifier</param>
        /// <param name="excludeUserId">ID utilisateur � exclure (pour les mises � jour)</param>
        /// <returns>True si le nom d'utilisateur existe d�j�</returns>
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