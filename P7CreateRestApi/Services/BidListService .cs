using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class BidListService : IBidListService
    {
        private readonly IBidListRepository _bidListRepository;

        public BidListService(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }

        public async Task<ServiceResult<IEnumerable<BidList>>> GetAllBidsAsync()
        {
            try
            {
                var bids = await _bidListRepository.GetAllAsync();
                return ServiceResult<IEnumerable<BidList>>.Success(bids);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<BidList>>.Failure($"Erreur lors de la récupération des offres: {ex.Message}");
            }
        }

        public async Task<ServiceResult<BidList>> GetBidByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<BidList>.Failure("L'ID doit être supérieur à 0");

                var bid = await _bidListRepository.GetByIdAsync(id);
                if (bid == null)
                    return ServiceResult<BidList>.Failure("Offre non trouvée");

                return ServiceResult<BidList>.Success(bid);
            }
            catch (Exception ex)
            {
                return ServiceResult<BidList>.Failure($"Erreur lors de la récupération de l'offre: {ex.Message}");
            }
        }

        public async Task<ServiceResult<BidList>> CreateBidAsync(BidList bidList)
        {
            
            try
            {
                bidList.CreationDate = DateTime.Now;
                var createdBid = await _bidListRepository.CreateAsync(bidList);
                return ServiceResult<BidList>.Success(createdBid, "Offre créée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<BidList>.Failure($"Erreur lors de la création de l'offre: {ex.Message}");
            }
        }

        public async Task<ServiceResult<BidList>> UpdateBidAsync(int id, BidList bidList)
        {
            try
            {
                if (!await _bidListRepository.ExistsAsync(id))
                    return ServiceResult<BidList>.Failure("Offre non trouvée");

                bidList.BidListId = id;

                var updatedBid = await _bidListRepository.UpdateAsync(bidList);
                return ServiceResult<BidList>.Success(updatedBid, "Offre mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<BidList>.Failure($"Erreur lors de la mise à jour de l'offre: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteBidAsync(int id)
        {
            try
            {

                if (!await _bidListRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Offre non trouvée");

                var deleted = await _bidListRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Offre supprimée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression de l'offre: {ex.Message}");
            }
        }
    }
}
