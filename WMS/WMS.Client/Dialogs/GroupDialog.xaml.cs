using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMS.Client.Misc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for GroupDialog.xaml
    /// </summary>
    public partial class GroupDialog : BaseDialog
    {
        private MainWindow mainWindow;
        private List<ProductDto> products;
        private List<WarehouseDetailsDto> internalOnes = new List<WarehouseDetailsDto>();
        private List<WarehouseDetailsDto> externalOnes = new List<WarehouseDetailsDto>();
        private List<SectorDto> secotrs;
        private bool isLoaded;

        public GroupDialog(MainWindow mainWindow, int sectorId)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;

            isLoaded = false;
            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => ProductsService.GetProducts(new Request()), t =>
                {
                    products = t.Data;
                    Execute(() => WarehousesService.GetWarehouses(new Request()), x =>
                        {
                            foreach (WarehouseDetailsDto w in x.Data)
                            {
                                if (w.Internal)
                                    this.internalOnes.Add(w);
                                else
                                    this.externalOnes.Add(w);
                            }
                            isLoaded = true;
                            InitializeData();
                        });
                });
        }

        private void WarehousesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WarehouseDetailsDto selectedW = ((WarehouseDetailsDto)((sender as ComboBox).SelectedItem));
            int wId = selectedW != null ? selectedW.Id : -1;
            if (wId == -1)
                return;

            Execute(() => WarehousesService.GetSectors(new Request<int>(wId)), t =>
            {
                secotrs = t.Data;
                LoadSectorsForChosenWarehouse();
            });
        }

        private void LoadSectorsForChosenWarehouse()
        {
            if (secotrs == null)
                return;

            SectorsComboBox.Items.Clear();

            foreach (SectorDto s in secotrs)
                if (!s.Deleted && s.GroupsCount < s.Limit)
                    SectorsComboBox.Items.Add(s);
        }

        /// <summary>
        /// Przygotowywanie danych do wyświetlania
        /// </summary>
        private void InitializeData()
        {
            if (!isLoaded)
                return;

            foreach (WarehouseDetailsDto w in externalOnes)
                PartnersComboBox.Items.Add(w);

            foreach (WarehouseDetailsDto w in internalOnes)
                if ((w.Internal == true && w.FreeSectorsCount > 0) || w.Internal == false)
                    WarehousesComboBox.Items.Add(w);

            ProductGroupRow productRow = new ProductGroupRow(Products);
            foreach (ProductDto p in products)
            {
                productRow.ProductsComboBox.Items.Add(p);
            }
            Products.Items.Add(productRow);
        }

        /// <summary>
        /// Dodaje nowy wiersz do wprowadzenia danych produktu w partii.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            ProductGroupRow productRow = new ProductGroupRow(Products);
            Products.Items.Add(productRow);

            foreach (ProductDto p in products)
                productRow.ProductsComboBox.Items.Add(p.Name);
        }

        /// <summary>
        /// Zapisuje partię, po wcześniejszym sprawdzeniu danych i zamyka okno.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            if (PartnersComboBox.SelectedIndex < 0 || WarehousesComboBox.SelectedIndex < 0 || SectorsComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }

            GroupDetailsDto group = new GroupDetailsDto()
            {
                Internal = true,
                SectorId = ((SectorDto)SectorsComboBox.SelectedItem).Id,
                SectorNumber = ((SectorDto)SectorsComboBox.SelectedItem).Number,
                WarehouseName = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Name,
            };
            group.Products = new List<ProductDetailsDto>(Products.Items.Count);
            foreach (ProductGroupRow productRow in Products.Items)
            {
                if (productRow.ProductsComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
                    return;
                }
                ProductDetailsDto p = (ProductDetailsDto)productRow.ProductsComboBox.SelectedItem;
                group.Products.Add(new ProductDetailsDto()
                {
                    Count = int.Parse(productRow.Quantity.Text),
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ProductionDate = p.ProductionDate,
                });
            }

        }

        /// <summary>
        /// Anuluje tworzenie nowej partii i zamyka okna bez zapisu danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
