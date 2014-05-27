﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using BusinessLayer;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using inventory.Helpers;
namespace inventory.ViewModel
{
    public class ModifyDealersViewModel : DealersViewModelBase
    {

        public override string Name
        {
            get { return InventoryHelper.ModifyDealer; }
        }

        public override string Icon
        {
            get { return InventoryHelper.ModifyDealerIcon; }
        }
        public ModifyDealersViewModel()
        {
            BindGrid();
        }

        protected void BindGrid()
        {
            this._Delars = DelarServices.GetAllDelars(0);
        }

        private IList<dealer> _Delars;
        public IList<dealer> Delars
        {
            get { return _Delars; }
            set
            {
                _Delars = value;
                RaisedPropertyChanged("Delars");
            }
        }

        #region Button Command


        private ICommand _updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand(new Action<object>(UpdateDealer));
                }
                return _updateCommand;
            }
            set
            {
                _updateCommand = value;
                RaisedPropertyChanged("UpdateCommand");

            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(new Action<object>(DeleteDealer));
                }
                return _deleteCommand;
            }
            set
            {
                _updateCommand = value;
                RaisedPropertyChanged("DeleteCommand");

            }
        }
        #endregion

        public void DeleteDealer(object parameter)
        {
            bool flag = false;
            int delar_id = 0;
            TextBox tb = (TextBox)parameter;
            int.TryParse(tb.Text, out delar_id);
            if (delar_id > 0)
            {
                flag = DelarServices.DeleteDealer(delar_id);
            }
            if (flag == true)
            {
                BindGrid();
                RaisedPropertyChanged("Delars");
                MessageBox.Show("Dealer Deleted");
            }
        }

        public void UpdateDealer(object parameter)
        {
            int delar_id = 0;
            TextBox tb = (TextBox)parameter;
            int.TryParse(tb.Text, out delar_id);
            if (delar_id > 0)
            {
                dealer ob_dealer = Delars.Where(s => s.id == delar_id).First();
                List<dealer> temp_list = DelarServices.GetDelarByName(ob_dealer.dealer_name.Trim());
                if (temp_list.Count == 0 || temp_list.Count == 1)
                {
                    if (temp_list.Count == 1)
                    {
                        dealer temp = temp_list.FirstOrDefault();
                        if (temp.id != ob_dealer.id)
                        {
                            BindGrid();
                            RaisedPropertyChanged("Delars");
                            return;
                        }

                    }
                    int temp_delar_id = DelarServices.AddUpdateDealer(ob_dealer);
                    if (temp_delar_id == ob_dealer.id)
                    {
                        MessageBox.Show("Dealer " + ob_dealer.dealer_name + " is Updated");
                        BindGrid();
                        RaisedPropertyChanged("Delars");
                    }
                }
                else
                {

                    MessageBox.Show("Dealer Name already Exist");

                }

            }
        }

    }
}