using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Guldkortet
{
    public partial class Form1 : Form
    {
        /*kallar på Tcp klasserna för att kunna
         använda dem i metoder*/
        TcpListener listener;
        TcpClient client;
        int port = 12345;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile("nos.png");
        }
        //Skapar en lista för alla guldkort
        List<Guldkort> CardList = new List<Guldkort>();
        //Lista för spara alla kunder från textfilen
        List<CustomerData> CustomerList = new List<CustomerData>();
        
        //Metod för att ladda in listan med vinstnummrerna
        public void CardListLoader()
        {
            if (File.Exists("kortlista.txt"))
            {
                /*Skapar en stringlista för att spara varje rad i kortlistan
                 * rader läses in med hjälp av streamreader klassen*/
                List<string> ItemSaver = new List<string>();
                StreamReader reader = new StreamReader("kortlista.txt", Encoding.Default, true);
                string item = "";
                while ((item = reader.ReadLine()) != null)
                {
                    /*Lägger till varje rad i textfilen i
                     en string lista*/
                    ItemSaver.Add(item);
                    foreach (string card in ItemSaver)
                    {
                        string[] array = card.Split(new string[] { "###" },
                            StringSplitOptions.None);
                        //switchsats för att ta se till att rätt objekt skapas.
                        //array[0] = kortnummer , array[0] = korttyp
                        switch (array[1])
                        {
                            case "Dunderkatt":
                                CardList.Add(new Dunderkatt(array[0], array[1]));
                                break;
                            case "Kristallhäst":
                                CardList.Add(new Kristallhäst(array[0], array[1]));
                                break;
                            case "Överpanda":
                                CardList.Add(new Överpanda(array[0], array[1]));
                                break;
                            case "Eldtomat":
                                CardList.Add(new Eldtomat(array[0], array[1]));
                                break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Filen för sparade guldkort saknas.");
            }
        }
        //Laddar in kunddatan som sparas som objekt.
        public void LoadCustomerData()
        {
            //Kontrollerar att textfilen finns tillgänglig
            if (File.Exists("kundlista.txt"))
            {
                List<string> ItemSaver = new List<string>();
                StreamReader reader = new StreamReader("kundlista.txt",
                    Encoding.Default, true);
                string item = "";
                while ((item = reader.ReadLine()) != null)
                {
                    /*Lägger till varje rad i textfilen i
                     en string lista*/
                    ItemSaver.Add(item);
                    //Går igenom varje sträng och delar upp strängen i vektorer
                    foreach (string customerString in ItemSaver)
                    {
                        string[] array = customerString.Split(new string[] { "###" },
                            StringSplitOptions.None);
                        CustomerList.Add(new CustomerData(array[0],
                            array[1], array[2]));
                    }
                }
            }
            else
            {
                MessageBox.Show("Filen för sparade kunder saknas.");
            }
        }
        //Väntar på strängen som sedan läses in
        public async void StartReading(TcpClient c)
        {
            byte[] buffer = new byte[1024];

            int n = 0;
            try
            {
                n = await c.GetStream().ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
            StartReading(c);
            //Tar den mottagna datan och omvandlar den till en sträng
            // CheckString kontrollerar om kunden har vunnit ett guldkort
            CheckString(Encoding.Unicode.GetString(buffer, 0, n));
        }
        //Metod för att börja ta emot strängar
        public async void StartRecieving()
        {
            try
            {
                client = await listener.AcceptTcpClientAsync();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
            StartReading(client);
        }
        /*Skapar två strängar för att kunna spara Namn och stad 
         * till den registrerade kunden  */
        string saveCustomerName;
        string saveCustomerCity;
        public void CheckString(string newEntry)
        {
            
            /*Delar upp strängen som vi har fått av
             * exportprogrammet i två olika nummer */
            string[] array = newEntry.Split(new string[] { "-" },
                            StringSplitOptions.None);
            /*skapar varsin sträng åt */
            string regNumber = array[0];
            string cardNumber = array[1];
            int customerFound = 0;
            //Forloop för att söka efter matchande användare.
            for (int i = 0; i < CustomerList.Count; i++)
            {
                if (CustomerList[i].customerRegNumber == regNumber)
                {
                    /*Sparar namn och stad till den hittade kunden */
                    saveCustomerName = CustomerList[i].customerName;
                    saveCustomerCity = CustomerList[i].customerCity;
                    i = CustomerList.Count; //Avslutar forloopen
                    customerFound = 1;
                }
            }
            if (customerFound == 1)
            {
                /*När en användare har hittats så kontrolleras
                 * att kortet är ett guldkort*/
                CheckCard(cardNumber);
            }
            else
            {
                //Meddelar nos programmet om resultatet
                SendData("Det finns inte någon registrerad " +
                    "användare med det detta nummer.");
                MessageBox.Show("Det finns inte någon" +
                    " användare registrerat med det nummret.");
            }
        }
        public void CheckCard(string cardNumber)
        {
            try
            {   /*loopar igenon listan med sparade kort för att se
                 * om det finns ett matchande guldkort, om ett
                 matchande guldkort hittas så hittas det specifika
                typen av vilket kort det handlar om med en switch sats*/
                int cardFound = 0;
                for (int i = 0; i < CardList.Count; i++)
                {
                    if (cardNumber == CardList[i].cardNumber)
                    {
                        switch (CardList[i].cardType)
                        {
                            case "Dunderkatt":
                                Dunderkatt Cat = new Dunderkatt("", "");
                                label1.Text = Cat.ToString(saveCustomerName, saveCustomerCity);
                                ChangePicture(CardList[i].cardType);
                                SendData(Cat.ToString(saveCustomerName, saveCustomerCity));
                                break;
                            case "Kristallhäst":
                                Kristallhäst horse = new Kristallhäst("", "");
                                label1.Text = horse.ToString(saveCustomerName, saveCustomerCity);
                                ChangePicture(CardList[i].cardType);
                                SendData(horse.ToString(saveCustomerName, saveCustomerCity));
                                break;
                            case "Överpanda":
                                Överpanda panda = new Överpanda("", "");
                                label1.Text = panda.ToString(saveCustomerName, saveCustomerCity);
                                ChangePicture(CardList[i].cardType);
                                SendData(panda.ToString(saveCustomerName, saveCustomerCity));
                                break;
                            case "Eldtomat":
                                Eldtomat tomato = new Eldtomat("", "");
                                label1.Text = tomato.ToString(saveCustomerName, saveCustomerCity);
                                ChangePicture(CardList[i].cardType);
                                SendData(tomato.ToString(saveCustomerName, saveCustomerCity));
                                break;
                        }
                        //Avslutar loopen när ett matchande kort hittats.
                        i = CardList.Count; 
                        cardFound = 1;
                    }
                }
                if (cardFound == 0)
                {
                    /*Det finns inte något giltigt kort med det
                     * nummret så användaren meddelas 
                     * att denne inte vunnit något*/
                    Guldkort result = new Guldkort("", "");
                    label1.Text = result.ToString(saveCustomerName, saveCustomerCity);
                    //Ändrar bild till standardbilden
                    pictureBox1.Image = Image.FromFile("nos.png");
                    //Skickar beskedet även till exportpogrammet
                    SendData(result.ToString(saveCustomerName, saveCustomerCity));
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
            }
            
        }
        /*Metod för att visa rätt bild till vinsten */
        public void ChangePicture(string pictureType)
        {
            try
            {
                if(File.Exists(pictureType + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(pictureType + ".png");
                }
                else 
                {
                    throw new PictureNotFoundException("Bilden till"+
                       pictureType + " kortet saknas" );
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void btnStartServer(object sender, EventArgs e)
        {
            /*Skapar en lyssnare som väntar på att en 
             * klient ska ansluta till programmet.*/
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
            StartRecieving();
            //ska endast gå att starta servern en gång
            btnStartServe.Enabled = false;
            btnStartServe.BackColor = Color.Green;
            //Läser in data från textfilerna
            LoadCustomerData();
            CardListLoader();
        }
        private void label1_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e) { }
        //Metod för att skicka data till exportprogrammet
        public async void SendData(string message)
        {
            //Omvandlar strängen till bytes för att kunna skickas
            byte[] outputData = Encoding.Unicode.GetBytes(message);
            try
            {
                //Skickar datan till klienten
                await client.GetStream().WriteAsync(outputData, 0, outputData.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text);
            }
        }
    }
}
