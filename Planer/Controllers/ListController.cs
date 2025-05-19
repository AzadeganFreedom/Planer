using Microsoft.AspNetCore.Mvc;
using Planer.Models;
using Planer.Repositories;

namespace Planer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly PlanerDbContext _context;
        private readonly ListRepository listRepo;
        private readonly ItemRepository itemRepo;

        public ListController(PlanerDbContext context, ListRepository repo1, ItemRepository repo2)
        {
            _context = context;
            this.listRepo = repo1;
            this.itemRepo = repo2;
        }

        //##########################################################################################################

        // Create: 


        //##########################################################################################################

        // Read:

        [HttpGet("{id:int}")] // Get List by Id
        public async Task<ActionResult> GetListByIdAsync(int id)
        {
            var selectedList = await listRepo.GetListByIdAsync(id);

            if (selectedList == null)
            {
                return NotFound("List not found.");
            }
            return Ok(selectedList);
        }

        [HttpGet("GetListByUserId")]// Get a List by UserId
        public async Task<ActionResult> GetListByUserIdAsync(int userId)
        {
            var selectedList = await listRepo.GetListByUserIdAsync(userId);

            if (selectedList == null)
            {
                return NotFound("List not found.");
            }
            return Ok(selectedList);
        }

        //##########################################################################################################

        // Update: 

        [HttpPatch("{id:int}")] // Add Item to List by Item Id
        public async Task<ActionResult> AddItemToListByIdAsync(int id, int itemId)
        {
            var selectedList = await listRepo.AddItemToListByIdAsync(id, itemId);

            if (selectedList == null)
            {
                return NotFound("List not found.");
            }
            return Ok(selectedList);
        }

        //##########################################################################################################

        // Delete: 

    }
}
