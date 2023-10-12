using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepViewModel>();
    }

    public ActionResult OnGet()
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

        Cheeps = _service.GetCheeps(pageNumber);
        return Page();
    }
}
