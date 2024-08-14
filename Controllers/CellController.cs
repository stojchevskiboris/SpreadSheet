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
        [Route("All", Name = "GetAllCells")]
        public async Task<ActionResult<IEnumerable<Cell>>> GetCellsAsync()
        {
            var cells = await _cellRepository.GetAllAsync();

            //OK - 200 - Success
            return Ok(cells);
        }


        [HttpGet]
        [Route("{id:int}", Name = "GetCellById")]
        public async Task<ActionResult<Cell>> GetCellByIdAsync(int id)
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

        [HttpGet]
        [Route("{x:int}/{y:int}", Name = "GetCellByCoordinates")]
        public async Task<ActionResult<Cell>> GetCellByCoordinates(int x, int y)
        {
            // Validate the input coordinates (optional)
            if (x < 0 || y < 0)
            {
                return BadRequest("Coordinates must be non-negative.");
            }

            // Retrieve the cell by x and y coordinates
            var cell = _cellRepository.GetAllAsync().Result
                .AsQueryable().Where(cell => cell.X == x && cell.Y == y).FirstOrDefault();

            // Return NotFound if the cell doesn't exist
            if (cell == null)
            {
                return NotFound($"The cell with coordinates ({x}, {y}) was not found.");
            }

            // Return the found cell
            return Ok(cell);
        }


        /* 
         [HttpGet]
         [Route("{page:int}/{size:int}", Name = "GetFilmsPaginated")]
         public async Task<ActionResult<IEnumerable<Film>>> GetFilmsPaginatedAsync(int page, int size)
         {
             //BadRequest - 400 - Badrequest - Client error
             if (page <= 0 || size <= 0)
             {
                 return BadRequest();
             }
             var films = _spreadSheetRepository.GetPaginated(page, size);
             return Ok(films);
         }

         [HttpPost]
         [Route("Create")]
         public async Task<ActionResult<Film>> CreateFilmAsync([FromBody] Film model)
         {
             if (model == null)
                 return BadRequest();

             await _spreadSheetRepository.CreateAsync(model);

             //Status - 201

             return Ok(model);
         }

         [HttpPut]
         [Route("Update")]
         public async Task<ActionResult> UpdateFilmAsync([FromBody] Film model)
         {
             if (model == null || model.FilmId <= 0)
                 BadRequest();

             var existing = await _spreadSheetRepository.GetByIdAsync(f => f.FilmId == model.FilmId, true);

             if (existing == null)
                 return NotFound();

             var newRecord = _mapper.Map<Film>(model);

             await _spreadSheetRepository.UpdateAsync(newRecord);

             return NoContent();
         }

         [HttpPatch]
         [Route("{id:int}/UpdatePartial")]
         public async Task<ActionResult> UpdateFilmPartialAsync(int id, [FromBody] JsonPatchDocument<Film> patchDocument)
         {
             if (patchDocument == null || id <= 0)
                 BadRequest();

             var existing = await _spreadSheetRepository.GetByIdAsync(s => s.FilmId == id, true);

             if (existing == null)
                 return NotFound();

             var dto = _mapper.Map<Film>(existing);

             patchDocument.ApplyTo(dto);

             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

             existing = _mapper.Map<Film>(dto);

             await _spreadSheetRepository.UpdateAsync(existing);

             //204 - NoContent
             return NoContent();
         }


         [HttpDelete("Delete/{id}", Name = "DeleteFilmById")]
         public async Task<ActionResult<bool>> DeleteFilmAsync(int id)
         {
             //BadRequest - 400 - Badrequest - Client error
             if (id <= 0)
                 return BadRequest();

             var film = await _spreadSheetRepository.GetByIdAsync(s => s.FilmId == id);
             //NotFound - 404 - NotFound - Client error
             if (film == null)
                 return NotFound($"The film with id {id} is not found");

             await _spreadSheetRepository.DeleteAsync(film);

             //OK - 200 - Success
             return Ok(true);
         }*/

    }
}
