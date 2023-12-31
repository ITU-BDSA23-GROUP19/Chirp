namespace Chirp.Core;

public interface IAuthorRepository
{
    public void CreateAuthor(AuthorDTO author);
    public void DeleteAuthor(string author);
    public Task<bool> CheckAuthorExistsAsync(string author);
    public Task<AuthorDTO> GetAuthorAsync(string author);
}