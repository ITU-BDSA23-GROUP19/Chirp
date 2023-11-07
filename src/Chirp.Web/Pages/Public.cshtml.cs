using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public string Text { get; set; } = "";

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public void OnPost(string text)
    {
        Text = text;

        CheepDTO cheepDTO = new CheepDTO("Jacqualine Gilcoine", Text, "2023-08-01 13:17:45");

        _repository.CreateCheep(cheepDTO);
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        if (page < 1)
        {
            page = 1;
        }

        Cheeps = await _repository.GetCheepsAsync(page);

        return Page();
    }
}