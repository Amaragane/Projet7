using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;

namespace P7CreateRestApi.Services.Interfaces
{
    public interface ICurvePointService
    {
        Task<ServiceResult<IEnumerable<CurvePoint>>> GetAllCurvePointsAsync();
        Task<ServiceResult<CurvePoint>> GetCurvePointByIdAsync(int id);
        Task<ServiceResult<CurvePoint>> CreateCurvePointAsync(CurvePoint curvePoint);
        Task<ServiceResult<CurvePoint>> UpdateCurvePointAsync(int id, CurvePoint curvePoint);
        Task<ServiceResult<bool>> DeleteCurvePointAsync(int id);
    }
}
