namespace Chirp.Core;

public interface IAuthorRepository
{
    public void CreateAuthor(AuthorDTO author);
    public void DeleteAuthor(string author);
    public Task<AuthorDTO> GetAuthorFromNameAsync(string name);
    public bool CheckAuthorExists(string author);
}