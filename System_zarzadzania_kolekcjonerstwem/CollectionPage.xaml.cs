using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;

namespace System_zarzadzania_kolekcjonerstwem
{
    public partial class CollectionPage : ContentPage
    {
        private Collection collection;
        private List<Collection> collections;
        private Action onSave;

        public CollectionPage(Collection collection, List<Collection> collections, Action onSave)
        {
            InitializeComponent();
            this.collection = collection;
            this.collections = collections;
            this.onSave = onSave;
            CollectionNameLabel.Text = collection.Name.Replace("%srd", ";");
            DisplayItems();
        }

        private void DisplayItems()
        {
            ItemsList.Children.Clear();
            foreach (string item in collection.Items)
            {
                StackLayout stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

                Entry entry = new Entry
                {
                    Text = item,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(5)
                };

                Button saveButton = new Button
                {
                    Text = "Save",
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(5)
                };
                saveButton.Clicked += (sender, e) =>
                {
                    int index = collection.Items.IndexOf(item);
                    if (index != -1)
                    {
                        collection.Items[index] = entry.Text;
                        onSave?.Invoke();
                    }
                };

                Button deleteButton = new Button
                {
                    Text = "Delete",
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(5)
                };
                deleteButton.Clicked += (sender, e) =>
                {
                    collection.Items.Remove(item);
                    onSave?.Invoke();
                    DisplayItems();
                };

                stackLayout.Children.Add(entry);
                stackLayout.Children.Add(saveButton);
                stackLayout.Children.Add(deleteButton);

                ItemsList.Children.Add(stackLayout);
            }
        }

        private void AddItemButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewItemEntry.Text))
            {
                collection.Items.Add(NewItemEntry.Text);
                onSave?.Invoke();
                DisplayItems();
                NewItemEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Error", "Please enter an item name", "OK");
            }
        }
    }
}




