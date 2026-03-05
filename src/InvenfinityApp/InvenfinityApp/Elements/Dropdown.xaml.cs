using Backend.Application.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace InvenfinityApp.Elements
{
    public partial class Dropdown : UserControl
    {
        public Dropdown()
        {
            InitializeComponent();
        }

        public ObservableCollection<IDtoDropdownElement> ItemsSourceInternal
        {
            get { return (ObservableCollection<IDtoDropdownElement>)GetValue(ItemsSourceInternalProperty); }
            set { SetValue(ItemsSourceInternalProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceInternalProperty =
            DependencyProperty.Register(nameof(ItemsSourceInternal),
                typeof(ObservableCollection<IDtoDropdownElement>),
                typeof(Dropdown),
                new PropertyMetadata(new ObservableCollection<IDtoDropdownElement>()));



        public IEnumerable<IDtoDropdownElement> ItemsSource
        {
            get { return (IEnumerable<IDtoDropdownElement>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource),
                typeof(IEnumerable<IDtoDropdownElement>),
                typeof(Dropdown),
                new PropertyMetadata(null, OnItemsSourceChanged));



        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Dropdown)d;
            control.RefreshItems();
        }



        public bool AllowEmpty
        {
            get { return (bool)GetValue(AllowEmptyProperty); }
            set { SetValue(AllowEmptyProperty, value); }
        }

        public static readonly DependencyProperty AllowEmptyProperty =
            DependencyProperty.Register(nameof(AllowEmpty),
                typeof(bool),
                typeof(Dropdown),
                new PropertyMetadata(false, OnAllowEmptyChanged));



        private static void OnAllowEmptyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Dropdown)d;
            control.RefreshItems();
        }



        public int SelectedId
        {
            get { return (int)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        public static readonly DependencyProperty SelectedIdProperty =
            DependencyProperty.Register(nameof(SelectedId),
                typeof(int),
                typeof(Dropdown),
                new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        private void RefreshItems()
        {
            var list = new ObservableCollection<IDtoDropdownElement>();

            if (AllowEmpty)
            {
                list.Add(new EmptyDropdownElement());
            }

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                    list.Add(item);
            }

            ItemsSourceInternal = list;
        }
    }



    class EmptyDropdownElement : IDtoDropdownElement
    {
        public int Id => -1;
        public string Name => "Empty";
    }
}