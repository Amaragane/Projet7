using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Interface pour les opérations de repository sur les utilisateurs
        /// </summary>

            // ====================================================================
            // CREATE - Créer un nouvel utilisateur
            // ====================================================================

            /// <summary>
            /// Crée un nouvel utilisateur en base de données
            /// </summary>
            /// <param name="user">Utilisateur à créer</param>
            /// <returns>Utilisateur créé avec son ID généré</returns>
            Task<User> CreateAsync(User user);

            /// <summary>
            /// Version synchrone pour compatibilité avec l'existant
            /// </summary>
            /// <param name="user">Utilisateur à ajouter</param>
            void Add(User user);

            // ====================================================================
            // READ - Lire/Récupérer des utilisateurs
            // ====================================================================

            /// <summary>
            /// Récupère tous les utilisateurs
            /// </summary>
            /// <returns>Liste de tous les utilisateurs</returns>
            Task<IEnumerable<User>> GetAllAsync();

            /// <summary>
            /// Version synchrone pour compatibilité
            /// </summary>
            /// <returns>Liste de tous les utilisateurs</returns>
            Task<List<User>> FindAll();

            /// <summary>
            /// Récupère un utilisateur par son ID
            /// </summary>
            /// <param name="id">ID de l'utilisateur</param>
            /// <returns>Utilisateur trouvé ou null</returns>
            Task<User?> GetByIdAsync(int id);

            /// <summary>
            /// Version synchrone pour compatibilité
            /// </summary>
            /// <param name="id">ID de l'utilisateur</param>
            /// <returns>Utilisateur trouvé ou null</returns>
            User? FindById(int id);

            /// <summary>
            /// Récupère un utilisateur par son nom d'utilisateur
            /// </summary>
            /// <param name="username">Nom d'utilisateur</param>
            /// <returns>Utilisateur trouvé ou null</returns>
            Task<User?> GetByUsernameAsync(string username);

            /// <summary>
            /// Version synchrone pour compatibilité
            /// </summary>
            /// <param name="username">Nom d'utilisateur</param>
            /// <returns>Utilisateur trouvé ou null</returns>
            User? FindByUserName(string username);



            /// <summary>
            /// Recherche des utilisateurs par nom complet (partiel)
            /// </summary>
            /// <param name="searchTerm">Terme de recherche</param>
            /// <returns>Liste des utilisateurs correspondants</returns>
            Task<IEnumerable<User>> SearchByFullnameAsync(string searchTerm);

            // ====================================================================
            // UPDATE - Mettre à jour un utilisateur
            // ====================================================================

            /// <summary>
            /// Met à jour un utilisateur existant
            /// </summary>
            /// <param name="user">Utilisateur avec les nouvelles données</param>
            /// <returns>Utilisateur mis à jour</returns>
            Task<User> UpdateAsync(User user);

            /// <summary>
            /// Met à jour partiellement un utilisateur (sans le mot de passe)
            /// </summary>
            /// <param name="user">Utilisateur avec les nouvelles données</param>
            /// <returns>Utilisateur mis à jour</returns>
            Task<User> UpdateWithoutPasswordAsync(User user);

            /// <summary>
            /// Met à jour uniquement le mot de passe d'un utilisateur
            /// </summary>
            /// <param name="userId">ID de l'utilisateur</param>
            /// <param name="newPassword">Nouveau mot de passe (déjà hashé)</param>
            /// <returns>True si mis à jour avec succès</returns>
            Task<bool> UpdatePasswordAsync(int userId, string newPassword);

            // ====================================================================
            // DELETE - Supprimer un utilisateur
            // ====================================================================

            /// <summary>
            /// Supprime un utilisateur par son ID
            /// </summary>
            /// <param name="id">ID de l'utilisateur à supprimer</param>
            /// <returns>True si supprimé avec succès, False si non trouvé</returns>
            Task<bool> DeleteAsync(int id);

            /// <summary>
            /// Supprime un utilisateur par son nom d'utilisateur
            /// </summary>
            /// <param name="username">Nom d'utilisateur</param>
            /// <returns>True si supprimé avec succès</returns>
            Task<bool> DeleteByUsernameAsync(string username);

            // ====================================================================
            // EXISTENCE - Vérifier l'existence
            // ====================================================================

            /// <summary>
            /// Vérifie si un utilisateur existe par son ID
            /// </summary>
            /// <param name="id">ID de l'utilisateur</param>
            /// <returns>True si l'utilisateur existe</returns>
            Task<bool> ExistsAsync(int id);

            /// <summary>
            /// Vérifie si un nom d'utilisateur est déjà utilisé
            /// </summary>
            /// <param name="username">Nom d'utilisateur à vérifier</param>
            /// <param name="excludeUserId">ID utilisateur à exclure (pour les mises à jour)</param>
            /// <returns>True si le nom d'utilisateur existe déjà</returns>
            Task<bool> UsernameExistsAsync(string username, int? excludeUserId = null);

        
    }
}
