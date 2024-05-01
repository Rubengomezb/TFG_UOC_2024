using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(SearchRecipesView), typeof(SearchRecipesView));
            Routing.RegisterRoute(nameof(RecipeDetail), typeof(RecipeDetail));
            Routing.RegisterRoute(nameof(MainRecipeView), typeof(MainRecipeView));
            Routing.RegisterRoute(nameof(IngredientsView), typeof(IngredientsView));
            Routing.RegisterRoute(nameof(CategoryView), typeof(CategoryView));
            Routing.RegisterRoute(nameof(UserProfileView), typeof(UserProfileView));
        }
    }
}
