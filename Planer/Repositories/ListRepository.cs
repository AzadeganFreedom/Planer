using Microsoft.EntityFrameworkCore;
using Planer.Class;
using Planer.Models;

namespace Planer.Repositories
{
    public class ListRepository
    {

        private readonly PlanerDbContext _context;
        private readonly ItemRepository _itemRepository;
        public ListRepository(PlanerDbContext context)
        {
            _context = context;
            _itemRepository = new ItemRepository(context);
        }

        //##########################################################################################################

        // Create: 

        // Create List when a user is created: 
        public async Task<List>? CreateListAsync(int userId)
        {
            var list = new List();

            list.UserId = userId;

            _context.Lists.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        //##########################################################################################################

        // Read: 

        // Get a List by Id: 
        public async Task<List> GetListByIdAsync(int id)
        {
            //List<Item> item = new List<Item>();

            var list = await _context.Lists.FirstOrDefaultAsync(list => list.Id == id);

            var itemObj = _context.Lists.Include(l => l.Items)
                                        .FirstOrDefault(l => l.Id == id);

            return itemObj;
        }

        // Get List by UserId
        public async Task<List> GetListByUserIdAsync(int userId)
        {
            //List<Item> item = new List<Item>();

            var list = await _context.Lists.FirstOrDefaultAsync(list => list.UserId == userId);

            var itemObj = _context.Lists.Include(l => l.Items)
                                        .FirstOrDefault(l => l.UserId == userId);

            return itemObj;
        }

        //##########################################################################################################

        // Update:

        // Add Item to List by Item Id:
        public async Task<List>? AddItemToListByIdAsync(int listId, int itemId)
        {
            List<Item> item = new List<Item>();

            // Get current data for the Lidt
            var list = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId);

            // Get item data from itemId
            var itemObj = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            // Add item data to item variable
            item.Add(itemObj);

            list.Items = item;

            _context.Lists.Update(list);
            await _context.SaveChangesAsync();
            return list;
        }

        //##########################################################################################################

        // Delete: 
    }
}
