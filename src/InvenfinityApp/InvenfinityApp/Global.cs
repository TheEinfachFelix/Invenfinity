using Backend.Application.UseCases;
using InvenfinityApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvenfinityApp
{
    internal static class Global
    {
        public static UcRoot UcRoot { get; } = new UcRoot();
        public static ViewGridViewModel ViewGridViewModel { get; } = new ViewGridViewModel(UcRoot);
        
    }
}
