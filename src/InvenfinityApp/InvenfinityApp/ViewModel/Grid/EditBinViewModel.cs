using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InvenfinityApp.ViewModel.Grid
{
    public class EditBinViewModel : INotifyPropertyChanged
    {
        // Value welches selected ist

        // prop für binType

        // prop für gridID

        // pro list für Parts in slots




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
