using Microsoft.Extensions.DependencyInjection;
using uitleen_app.Models;

namespace uitleen_app.Views
{
    public partial class ItemsPage : ContentPage
    {
        private readonly ApiClient _apiClient;
        private readonly List<Item> _items = new();
        
        public string CategoryName { get; private set; } = string.Empty;

        public ItemsPage(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            BindingContext = this;
        }
        
        public void Initialize(List<Item> items, string categoryName)
        {
            _items.Clear();
            if (items != null)
            {
                _items.AddRange(items);
            }
            
            CategoryName = categoryName;
            
            // Create an enriched view model for each item
            var itemViewModels = _items.Select(item => new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Available = item.Available,
                NotAvailable = !item.Available,
                ImageUrl = !string.IsNullOrEmpty(item.ImageUrl) ? item.ImageUrl : "dotnet_bot.png",
                HasImage = !string.IsNullOrEmpty(item.ImageUrl)
            }).ToList();
            
            ItemsCollection.ItemsSource = itemViewModels;
            
            OnPropertyChanged(nameof(CategoryName));
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ItemViewModel selectedItem)
            {
                // Clear selection
                ((CollectionView)sender).SelectedItem = null;
                
                try
                {
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    
                    // Fetch detailed item information
                    var itemDetails = await _apiClient.GetAsync<Item>($"/items/{selectedItem.Id}");
                    
                    if (itemDetails != null)
                    {
                        // Show item details
                        await DisplayAlert(itemDetails.Name, 
                            $"Beschrijving: {itemDetails.Description}\n" +
                            $"Status: {(itemDetails.Available ? "Beschikbaar" : "Niet beschikbaar")}", 
                            "OK");
                    }
                    else
                    {
                        await DisplayAlert("Fout", "Item details konden niet worden geladen", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Fout", $"Item details laden mislukt: {ex.Message}", "OK");
                }
                finally
                {
                    ActivityIndicator.IsVisible = false;
                    ActivityIndicator.IsRunning = false;
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Available { get; set; }
        public bool NotAvailable { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool HasImage { get; set; }
    }
}