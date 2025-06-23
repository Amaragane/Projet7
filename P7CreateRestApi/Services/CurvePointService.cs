using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task<ServiceResult<IEnumerable<CurvePoint>>> GetAllCurvePointsAsync()
        {
            try
            {
                var curvePoints = await _curvePointRepository.GetAllAsync();
                return ServiceResult<IEnumerable<CurvePoint>>.Success(curvePoints);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<CurvePoint>>.Failure($"Erreur lors de la récupération des points de courbe: {ex.Message}");
            }
        }

        public async Task<ServiceResult<CurvePoint>> GetCurvePointByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<CurvePoint>.Failure("L'ID doit être supérieur à 0");

                var curvePoint = await _curvePointRepository.GetByIdAsync(id);
                if (curvePoint == null)
                    return ServiceResult<CurvePoint>.Failure("Point de courbe non trouvé");

                return ServiceResult<CurvePoint>.Success(curvePoint);
            }
            catch (Exception ex)
            {
                return ServiceResult<CurvePoint>.Failure($"Erreur lors de la récupération du point de courbe: {ex.Message}");
            }
        }

        public async Task<ServiceResult<CurvePoint>> CreateCurvePointAsync(CurvePoint curvePoint)
        {
            try
            {

                curvePoint.CreationDate = DateTime.Now;

                var createdCurvePoint = await _curvePointRepository.CreateAsync(curvePoint);
                return ServiceResult<CurvePoint>.Success(createdCurvePoint, "Point de courbe créé avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<CurvePoint>.Failure($"Erreur lors de la création du point de courbe: {ex.Message}");
            }
        }

        public async Task<ServiceResult<CurvePoint>> UpdateCurvePointAsync(int id, CurvePoint curvePoint)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<CurvePoint>.Failure("L'ID doit être supérieur à 0");

                if (!await _curvePointRepository.ExistsAsync(id))
                    return ServiceResult<CurvePoint>.Failure("Point de courbe non trouvé");

                curvePoint.Id = id;

                var updatedCurvePoint = await _curvePointRepository.UpdateAsync(curvePoint);
                return ServiceResult<CurvePoint>.Success(updatedCurvePoint, "Point de courbe mis à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<CurvePoint>.Failure($"Erreur lors de la mise à jour du point de courbe: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteCurvePointAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<bool>.Failure("L'ID doit être supérieur à 0");

                if (!await _curvePointRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Point de courbe non trouvé");

                var deleted = await _curvePointRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Point de courbe supprimé avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression du point de courbe: {ex.Message}");
            }
        }

    }
}
