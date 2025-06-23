using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTO.CurveDTO;

namespace P7CreateRestApi.DTO.Maping
{
    public static class CurvePointDTOMappings
    {
        /// <summary>
        /// Convertit ton CreateCurvePointDTO vers l'entité CurvePoint
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static CurvePoint ToEntity(CreateCurvePointDTO dto)
        {
            return new CurvePoint
            {
                CurveId = dto.CurveId,
                Term = dto.Term,
                CurvePointValue = dto.CurvePointValue
            };
        }
        /// <summary>
        /// Convertit l'entité CurvePoint vers ton GetUpdateCurvePointDTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static GetUpdateCurvePointDTO ToGetUpdateDTO(CurvePoint entity)
        {
            return new GetUpdateCurvePointDTO
            {
                Id = entity.Id,
                CurveId = entity.CurveId,
                Term = entity.Term,
                CurvePointValue = entity.CurvePointValue,
            };
        }
        /// <summary>
        /// Convertit ton GetUpdateCurvePointDTO vers l'entité CurvePoint (pour les mises à jour)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public static CurvePoint ToEntity(GetUpdateCurvePointDTO dto)
        {
            return new CurvePoint
            {
                Id = dto.Id,
                CurveId = dto.CurveId,
                Term = dto.Term,
                CurvePointValue = dto.CurvePointValue
            };
        }
    }
}


