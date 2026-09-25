using System;
using System.Collections.Generic;
using System.Text;



    namespace SRSRoelProjekt.Core.Services
    {
    public interface IDialogService
    {
        bool ShowConfirm(string message); // Ja/Nej
        void ShowMessage(string message); // OK-only
        void CloseDialog(object viewModel);
    }
}

