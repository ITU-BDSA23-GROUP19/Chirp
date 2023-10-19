using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
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