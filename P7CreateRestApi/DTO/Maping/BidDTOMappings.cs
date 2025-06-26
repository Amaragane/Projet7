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
                AskQuantity = 100.0,
                Bid = 50.0,
                Ask = 55.0,
                Benchmark = "Default Benchmark",
                BidListDate = DateTime.UtcNow,
                Commentary = "Default commentary text",
                BidSecurity = "Default Security",
                BidStatus = "Active",
                Trader = "John Doe",
                Book = "Default Book",
                CreationName = "System",
                CreationDate = DateTime.UtcNow,
                RevisionName = "System",
                RevisionDate = DateTime.UtcNow,
                DealName = "Default Deal",
                DealType = "Type A",
                SourceListId = "SRC-001",
                Side = "Buy"

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
