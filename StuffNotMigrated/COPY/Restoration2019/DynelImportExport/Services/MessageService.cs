using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restoration.Services
{
    enum MessageType { ERROR, INFO, WARNING };

    internal class MessageService
    {

        public static void ShowMessage(string text, MessageType type = MessageType.INFO)
        {
            switch (type)
            {
                case MessageType.ERROR:
                    {
                        //Slb.Ocean.Petrel.PetrelLogger.Error(text);
                        MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Messa MessageBoxButtons.OK);
                        break;
                    }
                case MessageType.WARNING:
                    {
                        //Slb.Ocean.Petrel.PetrelLogger.WarnBox(text);
                        MessageBox.Show(text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Messa MessageBoxButtons.OK);

                        break;
                    }
                default:
                    {
                        //Slb.Ocean.Petrel.PetrelLogger.InfoBox(text);
                        MessageBox.Show(text, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Messa MessageBoxButtons.OK);

                        break;
                    }
            }
        }

        public static void ShowError(string text)
        {
            Slb.Ocean.Petrel.PetrelLogger.Error(text);
        }



    };
}
