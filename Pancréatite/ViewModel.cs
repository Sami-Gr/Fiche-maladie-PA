using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pancréatite
{
    public class ViewModel : ViewModelBase
    {

        private Dictionary<string, object> _items;
        private Dictionary<string, object> _selectedItems;
      

        public Dictionary<string, object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public Dictionary<string, object> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }

        

        public ViewModel()
        {
            Items = new Dictionary<string, object>();
            Items.Add("Aucun", "un");
            Items.Add("Sténose de la VBP", "deux");
            Items.Add("Lithiase residuelle", "trois");
            Items.Add("Eventration", "quatre");
            Items.Add("Moignon vésiculaire", "Cinq");
            Items.Add("NP", "six");

            SelectedItems = new Dictionary<string, object>();
        }

        private void Submit()
        {
            foreach (KeyValuePair<string, object> s in SelectedItems)
                MessageBox.Show(s.Key);
        }


    }


}
