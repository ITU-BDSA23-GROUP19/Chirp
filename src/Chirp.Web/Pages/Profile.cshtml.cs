﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ProfileModel : PageModel
{
    public ICheepRepository CheepRepository { get; private set; }
    public IAuthorRepository AuthorRepository { get; private set; }
    public IFollowRepository FollowRepository { get; private set; }
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public IEnumerable<FollowDTO> Followers { get; set; }
    public IEnumerable<FollowDTO> Followings { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingsCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }

    public ProfileModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        CheepRepository = cheepRepository;
        AuthorRepository = authorRepository;
        FollowRepository = followRepository;

        Cheeps = new List<CheepDTO>();
        Followers = new List<FollowDTO>();
        Followings = new List<FollowDTO>();
    }

    public async Task<ActionResult> OnGetAsync([FromQuery] int page)
    {
        CurrentPage = page;

        if (CheepRepository != null && User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
        {
            int cheepCount = await CheepRepository.GetCheepCountFromAuthorAsync(User.Identity.Name);
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

            Cheeps = await CheepRepository.GetCheepsFromAuthorAsync(User.Identity.Name, page, pageSize);
            Followers = await FollowRepository.GetFollowersAsync(User.Identity.Name);
            Followings = await FollowRepository.GetFollowingsAsync(User.Identity.Name);
            FollowersCount = await FollowRepository.GetFollowersCountAsync(User.Identity.Name);
            FollowingsCount = await FollowRepository.GetFollowingsCountAsync(User.Identity.Name);
            return Page();
        }

        return RedirectToPage();
    }
}