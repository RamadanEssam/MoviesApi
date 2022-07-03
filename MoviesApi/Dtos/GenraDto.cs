namespace MoviesApi.Dtos
{
    public class GenraDto
    {
        [MaxLength(100)]    
        public string Name { get; set; }
    }
}
