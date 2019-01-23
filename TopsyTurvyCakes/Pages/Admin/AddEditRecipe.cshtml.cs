using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models; //access to Models to get object Recipe, this is how?


namespace TopsyTurvyCakes.Pages.Admin
{
    [Authorize]
    public class AddEditRecipeModel : PageModel
    {
        [FromRoute]
        public long? Id { get; set; }

        public bool IsNewRecipe {
            get{
                return Id == null;
            }
        }   

        [BindProperty]
        public Recipe Recipe { get; set; } // add object Recipe

        [BindProperty]
        public IFormFile image { get; set; } // for the image

        public IRecipesService recipesService { get; set; } // add controller

        public AddEditRecipeModel(IRecipesService recipesService) //where does that get called?
        {
            this.recipesService = recipesService; // it's called and assigned
        }
        
        [HttpGet]
        public async Task OnGetAsync()
        {
            Recipe = await recipesService.FindAsync(Id.GetValueOrDefault() ) ?? new Recipe();
            
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            var recipe = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();
            recipe.Name = Recipe.Name;
            recipe.Description = Recipe.Description;
            recipe.Ingredients = Recipe.Ingredients;
            recipe.Directions = Recipe.Directions;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (image != null)
            {
                recipe.SetImage(image);
            }
            //Recipe.Id = Id.GetValueOrDefault(); // said that is not nessary anymore
            await recipesService.SaveAsync(recipe);
            return RedirectToPage("/Recipe", new { id = recipe.Id });

        }

        public async Task<IActionResult> OnPostDelete()
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }
    }
}