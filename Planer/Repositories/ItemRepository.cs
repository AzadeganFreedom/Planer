using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Planer.Class;
using Planer.Models;

using Planer.Models;

namespace Planer.Repositories
{
    public class ItemRepository
    {
        private readonly PlanerDbContext _context;
        public ItemRepository(PlanerDbContext context)
        {
            _context = context;
        }

        //##########################################################################################################

        // Create: 

        //Create Item:
        public async Task<Item>? CreateItemAsync(Item item, int ListId)
        {
            var newItem = new Item
            {
                Description = item.Description,
                Created = DateTime.Now,
                ListId = ListId
            };

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }

        //##########################################################################################################

        // Read: 

        // Get Item by Id:
        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(item => item.Id == id);
        }

        //##########################################################################################################

        // Update: 

        //##########################################################################################################

        // Delete:

        // Delete Item by Id:
        public async Task<Item>? DeleteItemByIdAsync(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
