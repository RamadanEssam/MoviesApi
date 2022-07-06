using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.OrderByDescending(r=>r.Rate).Include(m => m.Genre)
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    Poster = m.Poster,
                    Rate = m.Rate,
                    Storeline = m.Storeline,
                    Title = m.Title,
                    Year = m.Year


                }).ToListAsync();
            return Ok(movies);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
              
                return NotFound();

            var dto = new MovieDetailsDto
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre?.Name,
                Poster = movie.Poster,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                Title = movie.Title,
                Year = movie.Year
            };
            return Ok(dto);
            
        }

        [HttpGet("{GetByGenraIdAsync}")]
        public async Task<IActionResult> GetByGenraIdAsync(byte genraId)
        {

            var movies = await _context.Movies
                .Where(m=>m.GenreId==genraId)
                .OrderByDescending(r => r.Rate).Include(m => m.Genre)
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    Poster = m.Poster,
                    Rate = m.Rate,
                    Storeline = m.Storeline,
                    Title = m.Title,
                    Year = m.Year


                }).ToListAsync();
            return Ok(movies); 

        }

            [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("poster is requerd !");

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
        [HttpPut("id")]

        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)

        {

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movie was Found with id {id} ");

            var isValidGenra = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenra)

                return BadRequest("Invalid genera ID !");

            if (dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))

                    return BadRequest("only .png and Jpg images are allowed");


                if (dto.Poster.Length > _maxAllowedPosterSize)

                    return BadRequest("Max allow size for poster is 1M");


                using var dataStreem = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStreem);
                movie.Poster = dataStreem.ToArray();
            }


            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
         
            movie.Storeline = dto.Storeline;
           
            movie.Rate = dto.Rate;  
            _context.SaveChanges();
            return Ok(movie);

        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movie was Found with id {id} ");

            _context.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
    }
}
