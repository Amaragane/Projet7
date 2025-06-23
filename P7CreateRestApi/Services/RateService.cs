using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<ServiceResult<IEnumerable<Rating>>> GetAllRatingsAsync()
        {
            try
            {
                var ratings = await _ratingRepository.GetAllAsync();
                return ServiceResult<IEnumerable<Rating>>.Success(ratings);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Rating>>.Failure($"Erreur lors de la récupération des évaluations: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Rating>> GetRatingByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<Rating>.Failure("L'ID doit être supérieur à 0");

                var rating = await _ratingRepository.GetByIdAsync(id);
                if (rating == null)
                    return ServiceResult<Rating>.Failure("Évaluation non trouvée");

                return ServiceResult<Rating>.Success(rating);
            }
            catch (Exception ex)
            {
                return ServiceResult<Rating>.Failure($"Erreur lors de la récupération de l'évaluation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Rating>> CreateRatingAsync(Rating rating)
        {
            try
            {

                var createdRating = await _ratingRepository.CreateAsync(rating);
                return ServiceResult<Rating>.Success(createdRating, "Évaluation créée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<Rating>.Failure($"Erreur lors de la création de l'évaluation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Rating>> UpdateRatingAsync(int id, Rating rating)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<Rating>.Failure("L'ID doit être supérieur à 0");

                if (!await _ratingRepository.ExistsAsync(id))
                    return ServiceResult<Rating>.Failure("Évaluation non trouvée");

                rating.Id = id;

                var updatedRating = await _ratingRepository.UpdateAsync(rating);
                return ServiceResult<Rating>.Success(updatedRating, "Évaluation mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<Rating>.Failure($"Erreur lors de la mise à jour de l'évaluation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteRatingAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<bool>.Failure("L'ID doit être supérieur à 0");

                if (!await _ratingRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Évaluation non trouvée");

                var deleted = await _ratingRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Évaluation supprimée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression de l'évaluation: {ex.Message}");
            }
        }




    }
}
