using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTO.BidDTO;

namespace P7CreateRestApi.DTO.Maping
{
    public static class BidDTOMappings
    {
        /// <summary>
        /// Convertit ton CreateBidDTO vers l'entité BidList
        /// </summary>
        public static BidList ToEntity(CreateBidDTO dto)
        {
            return new BidList
            {
                Account = dto.Account,
                BidType = dto.BidType,
                BidQuantity = dto.BidQuantity,

            };
        }

        /// <summary>
        /// Convertit l'entité BidList vers ton GetUpdateBidDTO
        /// </summary>
        public static GetUpdateBidDTO ToGetUpdateDTO(BidList entity)
        {
            return new GetUpdateBidDTO
            {
                BidListId = entity.BidListId,
                Account = entity.Account,
                BidType = entity.BidType,
                BidQuantity = entity.BidQuantity,

            };
        }

        /// <summary>
        /// Convertit ton GetUpdateBidDTO vers l'entité BidList (pour les mises à jour)
        /// </summary>
        public static BidList ToEntity(GetUpdateBidDTO dto)
        {
            return new BidList
            {
                BidListId = dto.BidListId,
                Account = dto.Account,
                BidType = dto.BidType,
                BidQuantity = dto.BidQuantity
            };
        }
    }
    }
