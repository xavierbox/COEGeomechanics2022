using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWinformsStuffOutOfPetrel
{
    public partial class Form1 : Form
    {
        HttpClient httpClient = new HttpClient();
        string urlToPost = "http://localhost:8080/project/post";

        DateTimePicker oDateTimePicker;


        public Form1()
        {
            InitializeComponent();

            oDateTimePicker = new DateTimePicker();
            oDateTimePicker.Visible = false;
            oDateTimePicker.Format = DateTimePickerFormat.Long;// Short;

            // An event attached to dateTimePicker Control which is fired when DateTimeControl is closed  
            oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
            oDateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);



            MII mii = new MII("2018.2");
            this.richTextBox1.Text = mii.ToString();
        }

        void oDateTimePicker_CloseUp(object osender, EventArgs e)
        {
            DateTimePicker sender = (DateTimePicker)(osender);
            // Hiding the control after use   
            sender.Visible = false;
        }

        private void dateTimePicker_OnTextChange(object osender, EventArgs e)
        {
            // Saving the 'Selected Date on Calendar' into DataGridView current cell  
            DateTimePicker sender = (DateTimePicker)(osender);
        }







        private void Form1_Load(object sender, EventArgs e)
        {

        }




        private void button1_Click(object sender, EventArgs e)
        {
            postNewSimulation();

        }



        private async void postNewSimulation()//  sender, EventArgs e)
        {
            /*
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>()
            {
               { "thing1", "hello" },
               { "thing2", "world" }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://localhost:8080/UploadInputParameters", content);
            var responseString = await response.Content.ReadAsStringAsync();
            */
            HttpClient httpClient = new HttpClient();
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, path);

            MultipartFormDataContent form = new MultipartFormDataContent();

            //form.Add(new StringContent("DEM"), "Simulator");
            //form.Add(new StringContent("xteijeiro"), "user");
            //form.Add(new StringContent((123.456f).ToString()), "scale");
            //form.Add(new StringContent("ClusterName"), "cluster");
            //form.Add(new StringContent("nodes"), "machines");

            int[] floats = new int[] { 1234, 22342, 3334, 22342 };

            byte[] data = new byte[floats.Length * sizeof(int)];// { 1.0f,2.0f,3.0f,4.0f};
            Buffer.BlockCopy(floats, 0, data, 0, floats.Length * sizeof(int));

            //byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            ByteArrayContent byteContent = new ByteArrayContent(data);
            form.Add(byteContent, "data");
            for (int n = 0; n < data.Count(); n++)
                Console.WriteLine(data[n].ToString());

            /*string[] files = 
          {
               "D:\\AppData\\DEM\\Models\\GaussianDome\\GaussianDome.sav",
               "D:\\AppData\\DEM\\Models\\GaussianDome\\GaussianDome_RunSettings.sim",
               "D:\\AppData\\DEM\\Models\\GaussianDome\\FoldingModel_180000.bin"
          };*/

            //put all the files in a temp folder
            //compress the folder
            //send it as a single file as a post with parameters. 


            HttpResponseMessage response = null;// = await httpClient.PostAsync("http://localhost:8ss080/", form);
            string textToShow = "";
            try
            {
                response = await httpClient.PostAsync(urlToPost, form);
                string sd = response.Content.ReadAsStringAsync().Result;
                textToShow = sd.Replace('{', ' ').Replace('}', ' ').Replace('"', ' ').Trim();//.Split(','); ;

                //response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    throw new SystemException(response.ReasonPhrase);
                }
                httpClient.Dispose();


            }
            catch
            {
                ;//serverMessage.Text = "Data transfer failed. " + textToShow;
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunTasksAsync();
        }

        
        public async Task RunTasksAsync()
        {
            int N = 500000; 
            float[] arr = Enumerable.Repeat(1.0f, N).ToArray();

            arr[N - 1] = 12345.34f;
            Product product = new Product
            {
                Name = "Gizmo",
                Price = 100,
                Category = "Widgets",
                Data = arr
            };

            Console.WriteLine("Request being sent");
            //httpClient
            var url = await CreateProductAsync(product);

            Console.WriteLine("Request sent"); 
        }


        public async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(urlToPost, product);

            //HttpContent content = new
            //HttpResponseMessage response = await httpClient.PostAsync(urlToPost, product);

            response.EnsureSuccessStatusCode();


            // return URI of the created resource.
            return response.Headers.Location;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
            {
                dataGridView1.Controls.Add(richTextBox1);
                richTextBox1.Visible = true;
            }
            else
                richTextBox1.Visible = false;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
            textBox1.Tag = null;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "") textBox1.Text = "Drop a seismic cube here";
        }

    }


    public class Product
    {
        public Product()
        {
            Name = "Gizmo";
            Price = 100;
            Category = "Widgets";
        }

        public float[] Data { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
    }
}
