using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Item item)
        {
            item.Id = Guid.NewGuid();
            if (item.Id == Guid.Empty)
            {
                throw new InvalidOperationException("Falha ao gerar um ID válido para o item.");
            }

            if (string.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentException("Nome do item não pode ser vazio.");

            await _itemRepository.AddAsync(item);
        }

        public async Task UpdateAsync(Item item)
        {
            var existingItem = await _itemRepository.GetByIdAsync(item.Id);
            if (existingItem == null)
                throw new KeyNotFoundException("Item não encontrado.");

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
                throw new KeyNotFoundException("Item não encontrado.");

            await _itemRepository.DeleteAsync(id);
        }
    }
}
