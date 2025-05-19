using Microsoft.AspNetCore.Mvc;
using Planer.Models;
using Planer.Class;
using Planer.Repositories;

namespace Planer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ItemController : ControllerBase
    {
        private readonly PlanerDbContext _context;
        private readonly ItemRepository itemRepo;
        private readonly ListRepository listRepo;

        public ItemController(PlanerDbContext context, ItemRepository repo1, ListRepository listRepo)
        {
            _context = context;
            this.itemRepo = repo1;
            this.listRepo = listRepo;
        }

        //##########################################################################################################

        // Create: 

        [HttpPost("CreateItem")] // Create Item
        public async Task<ActionResult> CreateItemAsync(Item item, int ListId)
        {
            if ( item == null)
            {
                return BadRequest("Item is null.");
            }

            var newItemCreated = await itemRepo.CreateItemAsync(item, ListId);

            if (newItemCreated == null)
            {
                return StatusCode(500, "Failed to create item.");
            }

            // Add Item to List
            var updatedList = await listRepo.AddItemToListByIdAsync(ListId, newItemCreated.Id);

            if (updatedList == null)
            {
                return StatusCode(500, "Failed to add item to list.");
            }

            return Ok(newItemCreated);
        }

        //##########################################################################################################

        // Read: 

        [HttpGet("{id:int}")] // Get Item by Id
        public async Task<ActionResult> GetItemByIdAsync(int id)
        {
            var selectedItem = await itemRepo.GetItemByIdAsync(id);

            if (selectedItem == null)
            {
                return NotFound("Item not found.");
            }

            return Ok(selectedItem);
        }

        //##########################################################################################################

        // Update: 

        //##########################################################################################################

        // Delete: 

        [HttpDelete("DeleteItemById={id:int}")] // Delete Item by Id
        public async Task<ActionResult> DeleteItemByIdAsync(int id)
        {
            var selectedItem = await itemRepo.DeleteItemByIdAsync(id);

            if (selectedItem == null)
            {
                return NotFound();
            }

            return Ok(selectedItem);
        }
    }
}
