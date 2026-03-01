using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Tree
{
    internal class LocationTreeViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
