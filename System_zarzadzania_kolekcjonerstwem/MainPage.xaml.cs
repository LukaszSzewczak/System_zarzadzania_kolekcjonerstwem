using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace System_zarzadzania_kolekcjonerstwem
{
    public partial class MainPage : ContentPage
    {
        private List<Collection> collections;
        private string dataPath;

        public MainPage()
        {
            InitializeComponent();
            dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "collections.txt");
            Debug.WriteLine($"Path: {dataPath}");
            collections = new List<Collection>();
            LoadCollections();
            DisplayCollections();
        }

        private void LoadCollections()
        {
            if (File.Exists(dataPath))
            {
                collections.Clear();
                string[] lines = File.ReadAllLines(dataPath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    Collection collection = new Collection(parts[0]);
                    for (int i = 1; i < parts.Length; i++)
                    {
                        string item = parts[i].Replace("%srd", ";");
                        collection.Items.Add(item);
                    }
                    collections.Add(collection);
                }
            }
        }

        private void SaveCollections()
        {
            using (StreamWriter writer = new StreamWriter(dataPath))
            {
                foreach (Collection collection in collections)
                {
                    string name = collection.Name.Replace(";", "%srd");
                    writer.Write(name);
                    foreach (string item in collection.Items)
                    {
                        string item1 = item.Replace(";", "%srd");
                        writer.Write(";" + item1);
                    }
                    writer.WriteLine();
                }
            }

            Debug.WriteLine($"Path: {dataPath}");

        }

        private void DisplayCollections()
        {
            CollectionList.Children.Clear();
            foreach (Collection collection in collections)
            {
                StackLayout stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

                Button button = new Button
                {
                    Text = collection.Name.Replace("%srd", ";"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(5)
                };
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushAsync(new CollectionPage(collection, collections, SaveCollections));
                };

                Button deleteButton = new Button
                {
                    Text = "Delete",
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(5)
                };
                deleteButton.Clicked += (sender, e) =>
                {
                    collections.Remove(collection);
                    SaveCollections();
                    DisplayCollections();
                };

                stackLayout.Children.Add(button);
                stackLayout.Children.Add(deleteButton);

                CollectionList.Children.Add(stackLayout);
            }
        }

        private void AddCollectionButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewCollectionEntry.Text))
            {
                Collection collection = new Collection(NewCollectionEntry.Text);
                collections.Add(collection);
                SaveCollections();
                DisplayCollections();
                NewCollectionEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Error", "Please enter a collection name", "OK");
            }
        }
    }
}



