namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task CreateAuthor(AuthorDTO author);
    public Task<AuthorDTO> GetAuthorFromNameAsync(string name);
    public Task<AuthorDTO> GetAuthorFromEmailAsync(string email);
    public void FollowAuthor(FollowDTO followDTO);
}