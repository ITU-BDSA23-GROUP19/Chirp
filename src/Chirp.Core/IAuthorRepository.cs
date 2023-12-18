namespace Chirp.Core;

public interface IAuthorRepository
{
    public void CreateAuthor(AuthorDTO author);
    public Task<AuthorDTO> GetAuthorFromNameAsync(string name);
}