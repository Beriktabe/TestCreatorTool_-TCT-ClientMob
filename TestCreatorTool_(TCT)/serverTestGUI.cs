using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using UDP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestCreatorTool__TCT_
{
    public partial class serverTestGUI : Form
    {
        private fullTest test;
        private List<string> ipClientList = new List<string>();
        private void parseCommand(string com)
        {

        }

        UDPSocket server = new UDPSocket();
        UDPSocket client = new UDPSocket();
        public serverTestGUI(fullTest test)
        {
            InitializeComponent();
            this.test = test;

            server.Server("0.0.0.0", 27000);
            server.onReceive += Server_onReceive;
            //UDPSocket c = new UDPSocket();
            //c.Client("127.0.0.1", 27000);
            //c.Send("TEST!");

        }

        private void Server_onReceive(object sender, UDPSocket.dataReceiveEventArgs e)
        {

            if(e.data.Length>0)
            {
                if (e.data.Split('|')[0].Contains("GET"))
                {
                    string json = JsonSerializer.Serialize(test, typeof(fullTest));

                    if(!ipClientList.Contains(e.ip.Address.ToString()))
                        ipClientList.Add(e.ip.Address.ToString());

                    client.Client(e.ip.Address.ToString(), 27001);
                    client.Send("TEST|" + json);

                    dataGridView1.Invoke((Action)delegate
                    {
                        dataGridView1.Rows.Add(e.data.Split('|')[1], "0", "0", "0", "-");
                    });
                }

                if (e.data.Split('|')[0].Contains("STATUS"))
                {
                    dataGridView1.Invoke((Action)delegate
                    {
                        string searchValue = e.data.Split('|')[1];
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            //MessageBox.Show(row.Cells[0].Value.ToString());
                            if (row.Cells[0].Value.ToString().Equals(searchValue))
                            {

                                
                                //dataGridView1.Rows.Add(e.data.Split('|')[1], e.data.Split('|')[2], e.data.Split('|')[3], e.data.Split('|')[4], e.data.Split('|')[5]);
                                
                                dataGridView1.Rows[row.Index].Cells[1].Value = e.data.Split('|')[2];
                                dataGridView1.Rows[row.Index].Cells[2].Value = e.data.Split('|')[3];
                                dataGridView1.Rows[row.Index].Cells[3].Value = e.data.Split('|')[4];
                                dataGridView1.Rows[row.Index].Cells[4].Value = e.data.Split('|')[5];


                                break;
                            }
                        }
                    });

                }

            }


  


        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(string a in ipClientList)
            {
                client.Client(a, 27001);
                client.Send("STOP|");
            }

            server._socket.Close();
        }
    }
}
