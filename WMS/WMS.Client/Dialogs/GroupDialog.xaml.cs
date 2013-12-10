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
        private List<WarehouseSimpleDto> internalOnes;
        private List<WarehouseSimpleDto> externalOnes;
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
                            foreach (WarehouseSimpleDto w in x.Data)
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
            WarehouseSimpleDto selectedW = ((WarehouseSimpleDto)((sender as ComboBox).SelectedItem));
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

            foreach (WarehouseSimpleDto w in externalOnes)
                PartnersComboBox.Items.Add(w);

            foreach (WarehouseSimpleDto w in internalOnes)
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
            //(sender as Button).IsEnabled = false;

            //if (PartnersComboBox.SelectedIndex < 0 || WarehousesComboBox.SelectedIndex < 0)
            //{
            //    MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
            //    (sender as Button).IsEnabled = true;
            //    return;
            //}
            //DatabaseAccess.Warehouse senderW =
            //    (DatabaseAccess.Warehouse)PartnersComboBox.Items[PartnersComboBox.SelectedIndex];

            //DatabaseAccess.Warehouse recipientW =
            //    ((DatabaseAccess.Sector)WarehousesComboBox.Items[WarehousesComboBox.SelectedIndex]).Warehouse;

            //DatabaseAccess.Sector sector = (DatabaseAccess.Sector)WarehousesComboBox.SelectedItem;

            //int productsCount = Products.Items.Count;
            //List<Tuple<string, int>> productsInfo = new List<Tuple<string, int>>();
            //// productsInfo[0,*] - name
            //// productsInfo[1,*] - quantity
            //foreach (ProductGroupRow p in Products.Items)
            //{
            //    if (p.ProductsComboBox.SelectedIndex < 0)
            //    {
            //        MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
            //        (sender as Button).IsEnabled = true;
            //        return;
            //    }
            //    var pp = productsInfo.Find(q => q.Item1 == (string)p.ProductsComboBox.Text);
            //    try
            //    {
            //        int count = 0;
            //        if (pp == null)
            //            productsInfo.Add(new Tuple<string, int>((string)p.ProductsComboBox.Text, count = int.Parse(p.Quantity.Text)));
            //        else
            //        {
            //            productsInfo.Remove(pp);
            //            productsInfo.Add(new Tuple<string, int>((string)p.ProductsComboBox.Text, count = pp.Item2 + int.Parse(p.Quantity.Text)));
            //        }
            //        if (count < 0)
            //            throw new InvalidOperationException();
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
            //        (sender as Button).IsEnabled = true;
            //        return;
            //    }
            //}

            //DatabaseAccess.SystemContext.Transaction(context =>
            //{
            //    DatabaseAccess.Shift s = new DatabaseAccess.Shift();

            //    s.Sender = senderW;
            //    s.Recipient = recipientW;
            //    context.Warehouses.Attach(s.Recipient);
            //    context.Warehouses.Attach(s.Sender);
            //    s.Date = new DateTime(DateTime.Now.Ticks);
            //    s.Latest = true;

            //    s.Group = new DatabaseAccess.Group()
            //    {
            //        Sector = sector,
            //        GroupDetails = new List<DatabaseAccess.GroupDetails>()
            //    };

            //    for (int k = 0; k < productsInfo.Count; k++)
            //    {
            //        DatabaseAccess.GroupDetails gd = null;
            //        try
            //        {
            //            gd = new DatabaseAccess.GroupDetails()
            //            {
            //                Product = products.Find(delegate(DatabaseAccess.Product prod)
            //                {
            //                    return prod.Name == productsInfo[k].Item1;
            //                }),
            //                Count = productsInfo[k].Item2,
            //            };
            //        }
            //        catch
            //        {
            //            MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
            //            return false;
            //        }
            //        //List<DatabaseAccess.Product> sameProducts = products.FindAll(delegate(DatabaseAccess.Product prod)
            //        //        {
            //        //            return prod.Name == productsInfo[0, k];
            //        //        });
            //        context.Products.Attach(gd.Product);

            //        s.Group.GroupDetails.Add(gd);
            //    }

            //    context.Shifts.Add(s);

            //    context.SaveChanges();

            //    return true;
            //}, t => Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    mainWindow.ReloadWindow();
            //    this.Close();
            //})), tokenSource);
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
