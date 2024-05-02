using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.ListView;
using TFG_UOC_2024.APP.Model;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.Behaviours
{
    public class CategoryBehaviour : Behavior<ContentPage>
    {
        #region Fields

        SfListView ListView;
        SearchBar SearchBar;
        #endregion

        #region Overrides
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
        #endregion

        #region Methods

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListView.DataSource != null)
            {
                ListView.DataSource.Filter = FilterContacts;
                ListView.DataSource.RefreshFilter();
            }
            ListView.RefreshView();
        }

        private bool FilterContacts(object obj)
        {
            if (SearchBar == null || SearchBar.Text == null)
                return true;
            var cat = obj as CategoryModel;
            return (cat.Name.ToLower().Contains(SearchBar.Text.ToLower()));
        }
        #endregion
    }
}
