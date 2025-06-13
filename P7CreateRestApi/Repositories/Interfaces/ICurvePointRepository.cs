using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface ICurvePointRepository
    {
        Task<IEnumerable<CurvePoint>> GetAllAsync();
        Task<CurvePoint?> GetByIdAsync(int id);
        Task<CurvePoint> CreateAsync(CurvePoint curvePoint);
        Task<CurvePoint> UpdateAsync(CurvePoint curvePoint);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
