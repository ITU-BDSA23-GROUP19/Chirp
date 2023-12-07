﻿using System.Reflection.Metadata;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    [BindProperty]
    public string Text { get; set; } = "";

    public PublicModel(ICheepRepository repository, IAuthorRepository authorRepository)
    {
        _repository = repository;
        _authorRepository = authorRepository;
    }


    public async Task<RedirectToPageResult> OnPostFollow(Author author)
    {
        Console.WriteLine("WOWSADOWSA");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            //var followDTO = new FollowDTO(User.Identity.Name, author.Name);
            //_authorRepository.FollowAuthor(followDTO);
            Console.WriteLine("kan jeg følge folk nu så?");
        }
        return RedirectToPage("");

    }

    public void changeText(string text){
        
    }

    public async Task OnPostUnfollow(Author author)
    {
        Console.WriteLine("Ja nu unfollower vi");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            //mangler at fjerne en followdto fra _authorrepository - måske mangler også funktionalitet til at fjerne overhovedet??
        }

    }

    public IActionResult OnPost(string text)
    {
        Console.WriteLine("onPost method is accessed.");
        if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            Text = text;
            _repository.CreateCheep(new CheepDTO(User.Identity.Name, Text, Utility.GetTimeStamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())));

        }
        return RedirectToPage("Public");
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        int cheepCount = await _repository.GetCheepCountAsync();
        int pageSize = 32;

        PageCount = cheepCount / pageSize;

        if (cheepCount % pageSize != 0)
        {
            PageCount++;
        }

        if (page < 1)
        {
            page = 1;
            CurrentPage = 1;
        }

        Cheeps = await _repository.GetCheepsAsync(page, pageSize);

        return Page();
    }
}