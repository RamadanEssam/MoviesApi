using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> _allowedExtenstions=new List<string> { ".jpg",".png"};
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync( [FromForm]MovieDto dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
         
                return BadRequest("only .png and Jpg images are allowed");
            

            if (dto.Poster.Length > _maxAllowedPosterSize)
            
                return BadRequest("Max allow size for poster is 1M");
          
            var isValidGenra = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenra)
            
                return BadRequest("Invalid genera ID !");
          
            using var dataStreem = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStreem);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStreem.ToArray(),
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Year = dto.Year,


            };
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie); 
               

        }
    }
}
