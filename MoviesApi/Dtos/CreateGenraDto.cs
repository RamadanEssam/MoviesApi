namespace MoviesApi.Dtos
{
    public class CreateGenraDto
    {
        [MaxLength(100)]    
        public string Name { get; set; }
    }
}
