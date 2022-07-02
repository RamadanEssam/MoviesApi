﻿using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllAsync(CreateGenraDto dto)
        {
            var genra = new Genre
            {
                Name = dto.Name
            };
            await _context.Genres.AddAsync(genra);
            _context.SaveChanges();
                return Ok(genra);

        }
    }
}