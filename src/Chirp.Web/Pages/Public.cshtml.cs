using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public IEnumerable<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        string? page = Request.Query["page"];

        int pageNumber = 1;
        if (page != null)
        {
            pageNumber = int.Parse(page);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
        }

        Cheeps = await _repository.GetCheepsAsync(pageNumber);
        return Page();
    }
}
