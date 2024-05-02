using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.ListView;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.Behaviours
{
    public class SearchRecipesBehaviour : Behavior<ContentPage>
    {
        SfListView ListView;
        SearchBar SearchBar;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            ListView = bindable.FindByName<SfListView>("listView");
            SearchBar = bindable.FindByName<SearchBar>("filterText");
            SearchBar.TextChanged += SearchBar_TextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            SearchBar.TextChanged -= SearchBar_TextChanged;
            SearchBar = null;
            ListView = null;
            base.OnDetachingFrom(bindable);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListView.DataSource != null)
            {
                ListView.DataSource.Filter = Filter;
                ListView.DataSource.RefreshFilter();
            }
            ListView.RefreshView();
        }

        private bool Filter(object obj)
        {
            if (SearchBar == null || SearchBar.Text == null)
                return true;
            var recipe = obj as RecipeDTO;
            return (recipe.Name.ToLower().Contains(SearchBar.Text.ToLower()) 
                || recipe.Description.ToLower().Contains(SearchBar.Text.ToLower()));
        }
    }
}