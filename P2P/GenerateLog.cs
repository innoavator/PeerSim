using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace P2P
{
    class GenerateLog
    {
        System.Data.OleDb.OleDbConnection MyConnection;
        System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
        string sql = null;
        string sql1 = null;
        Form1 f;
        bool file_exists = false;
        String nodes, packets;
       
        public void write_log_file(Form1 f,String type_of_network)
        {
            this.f = f;
            int time = f.time;
            Console.Write("Time in excel : " + time);
            Console.Write("Selected network : " + type_of_network);
            String network_type = make_network_string(type_of_network);
            nodes = Convert.ToString(f.no_of_nodes);
            String connections = make_connections_string();
            if (System.IO.File.Exists("log.xls"))
                file_exists = true;
     //       MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\ABHISHEK\\ExcelData1.xls;;Extended Properties=Excel 8.0;");
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source = log.xls;;Extended Properties=Excel 8.0;");
            MyConnection.Open();
            myCommand.Connection = MyConnection;
            if (!file_exists)
            {
                Console.Write("does not exist");
                sql = "CREATE TABLE Simulation (TypeofNetwork String, Nodes int, Packets int,Seeder_Nodes int,Seeder_Packets int,Download_edges int,Upload_edges int,Ratio float,Thresh_Probability float,Nodes_row int,Connections String,Alturists int,Traders int,Parasites int,Time_taken int)";

                myCommand.CommandText = sql;
                myCommand.ExecuteNonQuery();
        
            }
            sql = "INSERT INTO [Simulation$] (TypeofNetwork, Nodes, Packets,Seeder_Nodes,Seeder_Packets,Download_edges,Upload_edges,Ratio,Thresh_Probability,Nodes_row,Connections,Alturists,Traders,Parasites,Time_taken) values (" + " ' " + network_type + " ' " + "," + nodes + "," + f.no_of_packets.ToString() + "," + f.seeder_nodes.ToString() + "," + f.seeder_packets.ToString() + "," + f.no_of_download_edges.ToString() + "," + f.no_of_upload_edges.ToString() + "," + f.ratio.ToString() + "," + (f.threshold_probability / 100.0).ToString() + "," + f.widthstep.ToString() + "," + "'" + connections +"'" + "," +f.alturists.ToString() + "," + f.traders.ToString() + "," + f.parasites.ToString()+"," +time.ToString()+ ")";

            myCommand.CommandText = sql;
            myCommand.ExecuteNonQuery();
            MyConnection.Close();
        }

        private String make_connections_string()
        {
            String s = "Max edges :" + f.max_no_of_edges.ToString()+"\n" ;
            if (f.is_edges_maximum)
                s += " (Max Edges Mode)";
            else
                s += " (Random no of edges)";
            return s;
        }
        private String make_network_string(String type_of_network)
        {
            String s = type_of_network;
            if (f.is_network_static)
                s += "\n" +" Static";
            else
                s += "\n" + " Dynamic  : Rate of Growth:"+f.rate_of_network_growth.ToString();
            return s;

        }
    }

}
