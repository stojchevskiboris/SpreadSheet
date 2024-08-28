using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using SpreadSheet.Data.Repository;
using SpreadSheet.Models;


namespace SpreadSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellController : ControllerBase
    {
        private readonly IRepository<Cell> _cellRepository;
        private readonly IMapper _mapper;


        public CellController(IRepository<Cell> cellRepository, IMapper mapper)
        {
            _cellRepository = cellRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("GetAll", Name = "GetAll")]
        public async Task<ActionResult<IEnumerable<Cell>>> GetAll()
        {
            var cells = await _cellRepository.GetAllAsync();

            //OK - 200 - Success
            return Ok(cells);
        }


        [HttpGet]
        [Route("{id:int}", Name = "GetById")]
        public async Task<ActionResult<Cell>> GetById(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
            {
                return BadRequest();
            }

            var cell = await _cellRepository.GetByIdAsync(c => c.Id == id);
            //NotFound - 404 - NotFound - Client error
            if (cell == null)
            {
                return NotFound($"The cell with id {id} not found");
            }

            //OK - 200 - Success
            return Ok(cell);
        }

        // api/Cell/GetByCoordinates
        // CellViewModel
        [HttpPost]
        [Route("GetByCoordinates", Name = "GetByCoordinates")]
        public async Task<ActionResult<Cell>> GetByCoordinates(CellViewModel model)
        {
            // Validate the input coordinates (optional)
            if (model == null || model.X < 0 || model.Y < 0)
            {
                return BadRequest("Coordinates must be non-negative.");
            }

            // Retrieve the cell by x and y coordinates
            var cell = _cellRepository.GetAllAsync().Result
                .AsQueryable().Where(cell => cell.X == model.X && cell.Y == model.Y).FirstOrDefault();

            // Return NotFound if the cell doesn't exist
            if (cell == null)
            {
                return NotFound($"The cell with coordinates ({model.X}, {model.Y}) was not found.");
            }

            // Return the found cell
            return Ok(cell);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Cell>> CreateCell(CellViewModel model)
        {
            if (model == null || model.X < 0 || model.Y < 0)
            {
                return BadRequest("Invalid cell data.");
            }

            var cell = new Cell
            {
                X = model.X,
                Y = model.Y,
                Content = model.Content,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            // Save cell to the database
            await _cellRepository.CreateAsync(cell);

            return Ok(cell);
        }

        // api/Cell/EditById
        // CellViewModel
        [HttpPost]
        [Route("EditById", Name = "EditById")]
        public async Task<ActionResult<Cell>> EditById(CellViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Cell model cannot be null. Please provide valid payload.");
            }

            if (model.Id == 0)
            {
                return BadRequest("Cell ID must be provided.");
            }

            // Retrieve the cell by id
            var cell = _cellRepository.GetAllAsync().Result
                .AsQueryable().Where(cell => cell.Id == model.Id).FirstOrDefault();

            // Return NotFound if the cell doesn't exist
            if (cell == null)
            {
                return NotFound($"The cell with id ({model.Id}) was not found.");
            }

            cell.Content = model.Content;
            cell.ModifiedOn = DateTime.Now;
            await _cellRepository.UpdateAsync(cell);

            // Return the found cell
            return Ok(cell);
        }
    }
}
