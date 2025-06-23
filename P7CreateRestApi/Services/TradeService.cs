using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services.Interfaces;

namespace P7CreateRestApi.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<ServiceResult<IEnumerable<Trade>>> GetAllTradesAsync()
        {
            try
            {
                var trades = await _tradeRepository.GetAllAsync();
                return ServiceResult<IEnumerable<Trade>>.Success(trades);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Trade>>.Failure($"Erreur lors de la récupération des transactions: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Trade>> GetTradeByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<Trade>.Failure("L'ID doit être supérieur à 0");

                var trade = await _tradeRepository.GetByIdAsync(id);
                if (trade == null)
                    return ServiceResult<Trade>.Failure("Transaction non trouvée");

                return ServiceResult<Trade>.Success(trade);
            }
            catch (Exception ex)
            {
                return ServiceResult<Trade>.Failure($"Erreur lors de la récupération de la transaction: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Trade>> CreateTradeAsync(Trade trade)
        {
            try
            {


                trade.CreationDate = DateTime.Now;
                trade.CreationName = "System";

                var createdTrade = await _tradeRepository.CreateAsync(trade);
                return ServiceResult<Trade>.Success(createdTrade, "Transaction créée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<Trade>.Failure($"Erreur lors de la création de la transaction: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Trade>> UpdateTradeAsync(int id, Trade trade)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<Trade>.Failure("L'ID doit être supérieur à 0");

                if (!await _tradeRepository.ExistsAsync(id))
                    return ServiceResult<Trade>.Failure("Transaction non trouvée");


                trade.TradeId = id;
                trade.RevisionDate = DateTime.Now;
                trade.RevisionName = "System";

                var updatedTrade = await _tradeRepository.UpdateAsync(trade);
                return ServiceResult<Trade>.Success(updatedTrade, "Transaction mise à jour avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<Trade>.Failure($"Erreur lors de la mise à jour de la transaction: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteTradeAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return ServiceResult<bool>.Failure("L'ID doit être supérieur à 0");

                if (!await _tradeRepository.ExistsAsync(id))
                    return ServiceResult<bool>.Failure("Transaction non trouvée");

                var deleted = await _tradeRepository.DeleteAsync(id);
                return ServiceResult<bool>.Success(deleted, "Transaction supprimée avec succès");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Erreur lors de la suppression de la transaction: {ex.Message}");
            }
        }

    }
}
