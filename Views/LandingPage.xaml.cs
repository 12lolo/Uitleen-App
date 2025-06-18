using System.Windows.Input;
using uitleen_app.Models;

namespace uitleen_app.Views
{
    public partial class LandingPage : ContentPage
    {
        private readonly ApiClient _apiClient;
        public ICommand RefreshCommand { get; }

        public LandingPage(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            
            RefreshCommand = new Command(async () => await LoadCategories(true));
            BindingContext = this;
            
            // Load categories when the page appears
            Appearing += async (s, e) => await LoadCategories();
        }

        private async Task LoadCategories(bool isRefreshing = false)
        {
            try
            {
                if (!isRefreshing)
                {
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                }

                var categories = await _apiClient.GetAsync<List<Category>>("/categories");
                
                if (categories != null)
                {
                    CategoriesCollection.ItemsSource = categories;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Categorieën laden mislukt: {ex.Message}", "OK");
            }
            finally
            {
                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
                CategoriesRefreshView.IsRefreshing = false;
            }
        }

        private async void OnCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Category selectedCategory)
            {
                // Clear selection
                ((CollectionView)sender).SelectedItem = null;
                
                try
                {
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    
                    // Fetch items for this category
                    var items = await _apiClient.GetAsync<List<Item>>($"/categories/{selectedCategory.Id}/items");
                    
                    if (items != null && items.Count > 0)
                    {
                        // Get the ItemsPage from the service provider
                        var itemsPage = Handler.MauiContext.Services.GetService<ItemsPage>();
                        
                        // Initialize it with the items and category name
                        itemsPage.Initialize(items, selectedCategory.Name);
                        
                        // Navigate to the items page
                        await Navigation.PushAsync(itemsPage);
                    }
                    else
                    {
                        await DisplayAlert("Info", $"Geen items gevonden in de categorie '{selectedCategory.Name}'", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Fout", $"Items laden mislukt: {ex.Message}", "OK");
                }
                finally
                {
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                }
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Navigate to login page
            await Navigation.PushAsync(new LoginPage(_apiClient));
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}