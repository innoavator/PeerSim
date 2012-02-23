/*

    Copyright (C) 2011  Abhishek Choudhary

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms; 
public struct node
{
    public int x;
    public int y;
    
    public int num_packets;
    public bool is_enqueued;
    public bool dnl_finished;
    public int dnl_edges;
    public int upl_edges;
    public int[] packets;
    public int[] edges; //0 for left , 1 for right, 2 for up,3 for down
    public bool[] is_linked;
    public bool is_alive;
    public int no_of_connections;
    public bool is_connected;
    public int times_dnl_completed;
    public int category;       //1 for alturists,2 for traders, 3 for parasites
}
       
 

namespace P2P
{
    public partial class Form1 : Form
    {
      //  Thread prim;
        public node[] network;
        Exceptions ex = new Exceptions();
        Connection con_random = new Connection();
        GenerateLog log = new GenerateLog();
        ExcelCharts chart = new ExcelCharts();
        Help h = new Help();
        int[,] packet_distribution,packet_distribution1;
        public int graph_type=0;
        public int no_of_nodes,seeder_nodes;
        int[] prevalence_arr,sorted_packets_arr;
        public int no_of_packets = 10,no_of_upload_edges = 1,no_of_download_edges = 4;
        public float ratio;
        public bool enable_simu_upl_dnl = false;
        public int widthstep, max_time, cols, vert_dist,max_running_time = 2000;
        int[] tmp_edges;
        int[] incidence_arr ;
        int[] arr,pac_arr,accu_density_arr;
        int[] seedees;
        int random_node = 0;
        public int seeder_packets = 0,eligible_from_seedeer_nodes = -1;
        int plotting_time;
        public bool is_network_static = true;
        public int no_of_alive_nodes = 0,rate_of_network_growth;
        int test_count = 0,temp = 0,end = 0;
        int desired_time;
        float[] barwidth;
        int max_value1 = 0,no_of_repititions = 0;
        Graphics g, g1,g2,g3,g4;
        int panel1_height, panel1_width,x_offset = 5,y_offset = 5,panel4_height,panel4_width;
        public int time = 0,width,height,hori_dist,panel2_width,panel2_height;
        public int head =0,tail =0,mode = 0,max_no_of_edges;
        Random rand;
        public bool is_edges_maximum = true;
        bool packet_distribution_flag = false,per_node_flag = false,prevalence_flag = false,stop_flag =false;
        bool acc_density_flag = false;
        public int threshold_probability =50,leaving_probability = 50;
        bool stop_time = true;
        Bitmap b,b1,b2,b3,b4;
        public int alturists = 40, traders = 30, parasites = 30;
        
        public Form1()
        {
            InitializeComponent();
            panel1_height = this.pictureBox5.Height;
            panel1_width = this.pictureBox5.Width;
            panel2_width = this.pictureBox7.Width;
            panel2_height = this.pictureBox7.Height;
            panel4_height = this.pictureBox8.Height;
            panel4_width = this.pictureBox8.Width;
            b = new Bitmap(this.pictureBox1.Width,this.pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            b1 = new Bitmap(panel1_width,panel2_width, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            b2 = new Bitmap(panel1_width,panel1_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            b3 = new Bitmap(panel2_width,panel2_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            b4 = new Bitmap(panel4_width, panel4_width, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //    g = this.pictureBox1.CreateGraphics();
            this.pictureBox1.Image = b;
            g = Graphics.FromImage(pictureBox1.Image);
            g1 = Graphics.FromImage(b1);
            g2 = Graphics.FromImage(b2);
            g3 = Graphics.FromImage(b3);
            g4 = Graphics.FromImage(b4);
            
          /*  g1 = this.panel1.CreateGraphics();
            g2 = this.panel2.CreateGraphics();
            g3 = this.panel3.CreateGraphics();
            g4 = this.panel4.CreateGraphics();*/
            this.textBox2.Text = "3";
            this.label6.Text = "0";
            tmp_edges = new int[4];
            this.radioButton1.Checked = true;
            barwidth = new float[4];
            this.textBox3.Text = "5";
            this.comboBox3.SelectedIndex = 0;
            this.textBox4.Text = "10";
            desired_time = 2000;
            this.trackBar4.Maximum = max_running_time;
            this.trackBar4.Value = max_running_time;
            this.label27.Text = Convert.ToString(this.trackBar4.Value);
            this.textBox5.Text = "0";
            this.comboBox4.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            
           
        }
        void print_array(int[] a, int start, int end)
        {
     //       Console.Write("Stack array\n");
            for (int i = start; i < end; i++)
              Console.Write(a[i] + "  ");
            Console.Write("\n\n");
        }
        void initialise()
        {
            graph_type = this.comboBox6.SelectedIndex;
            test_count = 0;
            no_of_nodes = Convert.ToInt32(this.textBox8.Text);
            no_of_packets = Convert.ToInt32(this.textBox1.Text);
            seeder_nodes = Convert.ToInt32(this.textBox2.Text);
            max_no_of_edges = Convert.ToInt32(this.textBox7.Text);
            
            if (this.checkBox2.Checked)
                enable_simu_upl_dnl = true;
       // ******************Upload Edges********************************//     
            if (this.radioButton1.Checked)
                no_of_upload_edges = 1;
            else if (this.radioButton2.Checked)
                no_of_upload_edges = 2;
            else if (this.radioButton3.Checked)
                no_of_upload_edges = 3;
            else if (this.radioButton5.Checked)
                no_of_upload_edges = 0;
            else no_of_upload_edges = 4;
    //************************Download Edges****************************//
            if (this.radioButton6.Checked)
                no_of_download_edges = 1;
            else if (this.radioButton7.Checked)
                no_of_download_edges = 2;
            else if (this.radioButton8.Checked)
                no_of_download_edges = 3;
            else if (this.radioButton9.Checked)
                no_of_download_edges = 4;
            else no_of_download_edges = 0;
     
    //**********************Connections ***************************************
      
            
            if (this.radioButton11.Checked)
            {
                is_network_static = true;
                this.trackBar3.Enabled = false;
                rate_of_network_growth = no_of_nodes;
            }
            arr = new int[no_of_nodes];
     //       incidence_arr = new int[2*no_of_nodes];
    //        prevalence_arr = new int[2*no_of_nodes];
            
            accu_density_arr = new int[no_of_packets];
            this.progressBar1.Maximum = no_of_nodes;
            this.progressBar2.Maximum = no_of_nodes;
            this.progressBar1.Value = 0;
            this.progressBar2.Value = 0;
            seeder_packets = Convert.ToInt32(this.textBox3.Text);
            mode = this.comboBox2.SelectedIndex;
            network = new node[no_of_nodes];
            int count = 0;
            head = 0; tail = 0;
            g.Clear(Color.Black);
            rand = new Random();
            eligible_from_seedeer_nodes = -1;
            this.trackBar4.Maximum = max_running_time;
            desired_time = this.trackBar4.Value;
    //        this.trackBar4.Value = 2 * no_of_nodes;
            this.label27.Text = Convert.ToString(this.trackBar4.Value);
            sorted_packets_arr = new int[no_of_packets];
            this.trackBar1.Value = 65;
            ratio = (float)((65 - 50) / 5 + 1);
            this.label6.Text = Convert.ToString(ratio);
        }
  
        void swap(int a, int b)
        {
            int t = a;
            a = b;
            b = t;
        }
        
        public void set_mode_parametres()
        {
            mode = this.comboBox2.SelectedIndex;
            if (mode == 1)
            {
                this.radioButton8.Checked = true;
                this.groupBox2.Enabled = true;
                this.radioButton3.Checked = true;
                this.groupBox1.Enabled = true;
                alturists = 100;
                traders = 0;
                parasites = 0;
             }
            else if (mode == 0)
            {
                this.radioButton8.Checked = true;
                this.groupBox2.Enabled = true;
                this.radioButton3.Checked = true;
                this.groupBox1.Enabled = true;
                alturists = Convert.ToInt16(this.textBox9.Text) ;
                traders = Convert.ToInt16(this.textBox10.Text);
                parasites = Convert.ToInt16(this.textBox11.Text);
            }
            else if (mode == 2)
            {
                parasites = 100;
                traders = 0;
                alturists = 0;
                this.radioButton5.Checked = true;
                this.groupBox1.Enabled = false;
                this.groupBox2.Enabled = true;
            }
            else
            {
                this.groupBox2.Enabled = true;
                this.groupBox1.Enabled = true;
                traders = 100;
                alturists = 0;
                parasites = 0;
                this.groupBox2.Enabled = true;

            }
            this.textBox9.Text = Convert.ToString(alturists);
            this.textBox10.Text = Convert.ToString(traders);
            this.textBox11.Text = Convert.ToString(parasites);
            
        }
        
        public void reset_vertices()
        {
            for (int i = 0; i < temp; i++)
            {
                network[arr[i]].upl_edges = no_of_upload_edges;
            }
            int y = no_of_alive_nodes;
            for (int i = 0; i < rate_of_network_growth; i++)
            {
                if (i+y < no_of_nodes)
                {
                    network[i+y].is_alive = true;
                    no_of_alive_nodes++;
                    this.progressBar2.Value++;
                }
                else
                    break;
            }
       //     no_of_alive_nodes += rate_of_network_growth;
       //     this.progressBar2.Value = no_of_alive_nodes;
        }
        
        void distribute_packets_from_seeder(int i)
        {
            int decide;
            int seeder_count = 0;
            for (int l = 0; l < i; l++)
            {
                seeder_count = 0;
                if (!network[seedees[l]].is_enqueued)
                {
                    arr[tail++] = seedees[l];
                    network[seedees[l]].is_enqueued = true;
                }
                for (int a = 0; a < max_no_of_edges; a++)
                {
                    if (network[seedees[l]].edges[a] != -1)
                    {
                        if (!network[network[seedees[l]].edges[a]].is_enqueued && network[network[seedees[l]].edges[a]].is_alive)
                        {
                            arr[tail++] = network[seedees[l]].edges[a];
                            network[network[seedees[l]].edges[a]].is_enqueued = true;
                        }
                    }
                }

              
                    for (int s = 0; s < no_of_packets; s++)
                    {
                        if (network[seedees[l]].packets[s] == 0)
                        {
                            decide = rand.Next() % 2;
                            if (mode != 3)
                                decide = 1;
                            if (decide == 1)
                            {
                                if (seeder_count >= seeder_packets)
                                    break;
                                network[seedees[l]].num_packets++;
                                network[seedees[l]].packets[s] = time;
                                seeder_count++;
                            }
                            
                        }

                    }
                    //        packet_distribution[time,network[seedees[l]].num_packets]++;
                    if ((mode != 3) && (network[seedees[l]].num_packets == no_of_packets))
                    {
                        incidence_arr[time]++;
                        if (graph_type % 2 == 0)
                        {
                            network[seedees[l]].dnl_finished = true;

                            if (this.progressBar1.Value < this.progressBar1.Maximum)
                                this.progressBar1.Value++;
                            if (this.checkBox1.Checked)
                            {
                                g.DrawRectangle(new Pen(Color.Yellow), network[seedees[l]].x, network[seedees[l]].y, 1, 1);
                                this.pictureBox1.Image = b;
                            }
                        }
                    }
                
            }
        }
        
        void enqueue_rand(int no)
        {
    //        Console.Write("Enqueue_rand - No of Alive Nodes :" + no_of_alive_nodes+" \n");
         //generate seeder_nodes different random numbers and enqueue them******************

            int number, k = 0, i;
            bool continue_flag = false;
            seedees = new int[no];
            for ( i = 0; i < no; )
            {
                continue_flag = false;
                if (k > 7)
                    break;
                if (eligible_from_seedeer_nodes == -1)
                    number = rand.Next() % no_of_alive_nodes;
                else
                    number = rand.Next() % eligible_from_seedeer_nodes;
                for (int l = 0; l < i; l++)
                {
                    if (number == seedees[l])
                    {
                        continue_flag = true;
                        break;
                    }
                }
                if ((network[number].dnl_finished)||(continue_flag))
                {
                    k++;
                    continue;
                }
                k = 0;
                seedees[i++] = number;
            }
            distribute_packets_from_seeder(i);     		
        }
        
        int check_if_can_upload(node vertex,int l)
        {
            if (vertex.upl_edges < 0)
                return (-1);
            for(int k=0;k<no_of_packets;k++)
            {
                if ((network[vertex.edges[l]].packets[k] == 0) && (vertex.packets[k] != 0))
                    return k;
            }
            return (-1);
        }
       
        
        void transfer_data()
        {
            int  download_counter = 0;
            int exchange_packet,fokat;
            float ratio_factor;
            Console.Write("Threshold probability : " + threshold_probability + "\n");
            Console.Write("Leaving probability : " + leaving_probability + "\n");
                while ((head < no_of_nodes) && (!stop_flag))
                {
                    
            //**********************Ratio Implementation*********************************//
                    
                    if ((ratio < 1) && (ratio > 0))
                    {
                        ratio_factor = 1 - ratio;
                        Console.Write("ratio factor : " + time % (int)(ratio_factor * 10) + "\n");
                        if ((time >= ratio * 10) && (time % (int)(ratio_factor * 10) == 0))
                            enqueue_rand(seeder_nodes);
                    }
                    else if ((ratio >= 1) && (time > 2))
                    {
                        for (int a = 0; a < ratio; a++)
                            enqueue_rand(seeder_nodes);
                    }
         //***********************Head Updation*************************************************//
                    if (graph_type % 2 == 0)
                    {
                        if (time > 1)
                        {
                            if (tail < no_of_nodes)
                                while ((network[arr[head]].dnl_finished) && (head < tail - 1))
                                    head++;
                            else
                                while ((network[arr[head]].dnl_finished) && (head < no_of_nodes))
                                {
                                    head++;
                                    if (head == no_of_nodes)
                                        break;
                                }
                        }
                    }
                    packet_distribution[time,no_of_packets] += head;
                    
                    prevalence_arr[time] = prevalence_arr[time - 1] + incidence_arr[time];
                    packet_distribution1[time, no_of_packets] = no_of_nodes - prevalence_arr[time];
                    packet_distribution[time, 0] += no_of_nodes - temp;
                    if (time >= max_running_time)
                    {
                        MessageBox.Show("Time has exceeded its maximum limit. Please increase the no of download and upload edges");
                        break;
                    }

                    //                packet_distribution[time, 0] += no_of_nodes - temp;

                    if (time >= desired_time - 1)
                        break;
                    Thread.Sleep(10);
                    Application.DoEvents();
                    if(!stop_time)
                    time += 1;
                    /*             if(time>2)
                                 while ((network[arr[head]].dnl_finished) && (head < tail-1 ))
                                     head++;*/
                    stop_time = false;
                    temp = tail;
                    reset_vertices();
                    //              Console.Write("I hav reset the vertices\n");
        //            Console.Write("No of Alive Nodes : " + no_of_alive_nodes + "\n");
                        Console.Write("\nTime : " + time + "\n");
                   //     Console.Write("Head : " + head + "     Tail :  " + tail + "\n Packets\n ");
                        Console.Write("Nodes completed download at this instant : " + incidence_arr[time - 1] + "\n");
                        Console.Write("Nodes completed download till this time : " + prevalence_arr[time - 1] + "\n");
                    //*************************************************************************//
                    for (int i = head; i < temp; i++)
                    {
           //                        Console.Write("Node no : " + arr[i] + "  Num of Packets : " + network[arr[i]].num_packets +"\n");
          //                         print_array(network[arr[i]].packets, 0, no_of_packets);
          //                         Console.Read();

                        //*******Enqueue Neighbours******//
                        for (int a = 0; a < max_no_of_edges; a++)
                        {
                            if (network[arr[i]].edges[a] != -1)
                            {
                                if (!network[network[arr[i]].edges[a]].is_enqueued && network[network[arr[i]].edges[a]].is_alive)
                                {
                                    arr[tail++] = network[arr[i]].edges[a];
                                    network[network[arr[i]].edges[a]].is_enqueued = true;
                                }
                            }
                        }


                        //***********************************Download****************************************//
                        download_counter = 0;
                        for (int k = 0; k < no_of_packets; k++)
                        {

                            if (download_counter >= network[arr[i]].dnl_edges)
                                break;
                            if (network[arr[i]].packets[k] == 0)
                            {

                                for (int l = 0; l < max_no_of_edges; l++)
                                {
                                    if (network[arr[i]].edges[l] != -1)
                                    {

                                        if ((network[network[arr[i]].edges[l]].is_alive) && (network[network[arr[i]].edges[l]].packets[k] != 0) && (network[network[arr[i]].edges[l]].upl_edges > 0))
                                        {
                                            if ((mode != 3) && (network[arr[i]].category != 2) && (network[network[arr[i]].edges[l]].category != 2))
                                            {
                                                if ((rand.Next()%101) < threshold_probability)
                                                  break;
                                                
                                                network[network[arr[i]].edges[l]].upl_edges--;
                                                network[arr[i]].packets[k] = time;
                                                network[arr[i]].num_packets++;
                                                download_counter++;
                                                if (!this.checkBox2.Checked)
                                                    network[arr[i]].upl_edges--;
                                                break;
                                            }
                                            else if (mode == 3)
                                            {
                                                exchange_packet = check_if_can_upload(network[arr[i]], l);

                                                if (exchange_packet >= 0)
                                                {
                                                    if ((rand.Next() % 101) < threshold_probability)
                                                        break;
                                                    network[network[arr[i]].edges[l]].upl_edges--;
                                                    network[arr[i]].packets[k] = time;
                                                    network[arr[i]].num_packets++;
                                                    network[network[arr[i]].edges[l]].packets[exchange_packet] = time;
                                                    network[network[arr[i]].edges[l]].num_packets++;
                                                    network[arr[i]].upl_edges--;
                                                    download_counter++;
                                                    if (!this.checkBox2.Checked)
                                                        network[arr[i]].upl_edges--;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        packet_distribution[time, network[arr[i]].num_packets]++;
                        packet_distribution1[time, network[arr[i]].num_packets]++;

    //**********************Implementation of infinite network*************************************//
 
                        if ((network[arr[i]].num_packets == no_of_packets) && (network[arr[i]].dnl_finished == false))
                        {
                            if(graph_type%2 == 1)
                            {
                              //  g.DrawRectangle(new Pen(Color.Yellow), network[arr[i]].x, network[arr[i]].y, 1, 1);
                              //  this.pictureBox1.Image = b;
                                if ((rand.Next() % 101) > leaving_probability)
                                {
                                    network[arr[i]].dnl_finished = false;
                                    network[arr[i]].times_dnl_completed++;
                                    fokat = network[arr[i]].times_dnl_completed;
                             //       g.DrawRectangle(new Pen(Color.FromArgb(fokat % 255, (fokat * 2) % 255, (fokat * 3) % 255)), network[arr[i]].x, network[arr[i]].y, 1, 1);
                             //         g.DrawRectangle(new Pen(Color.Yellow), network[arr[i]].x, network[arr[i]].y, 1, 1);

                                    
                                    //   Thread.Sleep(10);
                               //     this.pictureBox1.Image = b;
                                    incidence_arr[time]++;
                                    g.DrawRectangle(new Pen(Color.Yellow), network[arr[i]].x, network[arr[i]].y, 1, 1);
                                    this.pictureBox1.Image = b;
                                    for (int p = 0; p < no_of_packets; p++)
                                        network[arr[i]].packets[p] = 0;
                                    network[arr[i]].no_of_connections = 0;
                                    network[arr[i]].num_packets = 0;
                                    con_random.connect_node(this, arr[i], is_edges_maximum);
                                 }
                            }
        //********************************Finite Network***************************************
                            else
                            {
                                network[arr[i]].dnl_finished = true;
                                network[arr[i]].times_dnl_completed++;
                                incidence_arr[time]++;
                                if (this.progressBar1.Value < this.progressBar1.Maximum)
                                    this.progressBar1.Value++;
                                if (this.checkBox1.Checked)
                                {
                                    g.DrawRectangle(new Pen(Color.Yellow), network[arr[i]].x, network[arr[i]].y, 1, 1);
                                    this.pictureBox1.Image = b;
                                }
                            }
                        }
                    }
                    //        packet_distribution[time, no_of_packets] += head;

                }
            }
        // length is the maximum value of x axis,the array is plotted on the y axis with its max value as the max y axis value.
        private void draw_plot(int[] array,int length,Graphics gr,int max_value,int panel_width,int panel_height,int size,int barwidth_index)
        {
            int tmp, prev_tmp =(int) (array[0] * 1.0/max_value * panel_height) ;
            int increment = 1;
            gr.Clear(Color.White);
            gr.DrawLine(new Pen(Color.Blue), 0, 0, 0, panel_height);
            gr.DrawLine(new Pen(Color.Blue), 0, panel_height-1, panel_width,panel_height-1);
   //         float y_factor = (float)(max_value*1.0/panel_height);
            if (length > panel_width)
                increment = length / panel_width;
            barwidth[barwidth_index] = (float)(panel_width*1.0 / length);
            SolidBrush mybrush = new SolidBrush(Color.Green);
            for (int i = 1; i < length;i+=increment)
            {
                mybrush = new SolidBrush(Color.GreenYellow);
                tmp = (int)(array[i]*1.00/max_value * panel_height);
                gr.FillRectangle(SystemBrushes.ActiveCaption,i * barwidth[barwidth_index],panel_height-tmp,size,size);
                gr.DrawLine(new Pen(Color.Red), (i - 1) * barwidth[barwidth_index], panel_height - prev_tmp, i * barwidth[barwidth_index], panel_height - tmp);
                prev_tmp = tmp;
                
            }

        }
        
        private void draw_bargraph(int[] array, int length, Graphics gr, int max_value, int panel_width, int panel_height)
        {
            int tmp ;
            int increment = 1;
            gr.Clear(Color.White);
            gr.DrawLine(new Pen(Color.Blue), 0, 0, 0, panel_height);
            gr.DrawLine(new Pen(Color.Blue), 0, panel_height - 1, panel_width, panel_height - 1);
            float barwidth = 15;
            SolidBrush mybrush = new SolidBrush(Color.Black);
            int x,y;
            for (int i = 0; i < length; i+= increment)
            {
                x = (int)(i * barwidth);
                tmp = (int)(array[i] * 1.00 / max_value * panel_height);
                y = panel_height - tmp;
                gr.FillRectangle(mybrush, x, y, barwidth, tmp);
           }

        }
        
        public void init_graph()
        {
            height = this.pictureBox1.Height;
            width = this.pictureBox1.Width;
            vert_dist = (int)(this.numericUpDown2.Value);
            
            widthstep = (int)(this.numericUpDown1.Value);
            if (widthstep > no_of_nodes)
                widthstep = no_of_nodes;
            hori_dist = width / widthstep;       //no of nodes in 1 row
            cols = no_of_nodes / widthstep + 1;
            int k = 0;
            
            for (int i = 0; i < cols; i++)
            {
                if (k >= no_of_nodes)
                    break;
                for (int j = 0; j < widthstep; j++)
                {
                    if (k >= no_of_nodes)
                        break;
                    network[k].x = j * hori_dist + x_offset;
                    network[k].y = i * vert_dist + y_offset;
                    network[k].packets = new int[no_of_packets];
                    network[k].is_linked = new bool[max_no_of_edges];
                    network[k].edges = new int[max_no_of_edges];
                    network[i].num_packets = 0;
                    network[k].dnl_finished = false;
                    network[k].is_connected = false;
                    network[k].is_enqueued = false;
                    network[k].is_alive = false;
                    network[k].dnl_edges = no_of_download_edges;
                    network[k].upl_edges = no_of_upload_edges;
                    network[k].no_of_connections = 0;
                    network[k].times_dnl_completed = 0;
                    network[k].category = 0;
                    for (int a = 0; a < max_no_of_edges; a++)
                    {
                        network[k].edges[a] = -1;
                    }
                    if (this.comboBox3.SelectedIndex == 1)
                    {
                        this.textBox4.Text = Convert.ToString(widthstep);
                        eligible_from_seedeer_nodes = widthstep;
                    }
                    if(this.checkBox3.Checked)
                    g.DrawRectangle(new Pen(Color.Red), new Rectangle(network[k].x, network[k].y, 1, 1));
                    k++;
                }
            }
            alturists = Convert.ToInt16(this.textBox9.Text);
            traders = Convert.ToInt16(this.textBox10.Text);
            parasites = Convert.ToInt16(this.textBox11.Text);
            if ((alturists + traders + parasites) != 100)
            {
                parasites = 100 - traders - alturists;
                this.textBox11.Text = Convert.ToString(parasites);
            }
            con_random.allot_category(this, network, alturists * no_of_nodes / 100, traders * no_of_nodes / 100, parasites * no_of_nodes / 100);
           
            
          
        }

        private void connect_lattice_graph()
        {
            
            for (int i = 0; i < no_of_nodes; i++)
            {
                if ((!network[i].is_linked[1]) && (network[i].x < network[widthstep - 1].x) && (i + 1 < no_of_nodes))
                {
                    network[i].edges[1] = i + 1;
                    network[i + 1].edges[0] = i;
                    network[i].is_linked[1] = true;
                    network[i + 1].is_linked[0] = true;
                    if (this.checkBox3.Checked)
                        g.DrawLine(new Pen(Color.Green), network[i].x, network[i].y, network[network[i].edges[1]].x, network[network[i].edges[1]].y);
                }
                if ((!network[i].is_linked[0]) && (network[i].x > hori_dist + x_offset) && (i - 1 >= 0))
                {
                    network[i].edges[0] = i - 1;
                    network[i - 1].edges[1] = i;
                    network[i].is_linked[0] = true;
                    network[i - 1].is_linked[1] = true;
                    if (this.checkBox3.Checked)
                        g.DrawLine(new Pen(Color.Green), network[i].x, network[i].y, network[network[i].edges[0]].x, network[network[i].edges[0]].y);
                }
                if ((!network[i].is_linked[2]) && (network[i].y > network[0].y) && (i - widthstep >= 0))
                {
                    network[i].edges[2] = i - widthstep;
                    network[i - widthstep].edges[3] = i;
                    network[i].is_linked[2] = true;
                    network[i - widthstep].is_linked[3] = true;
                    if (this.checkBox3.Checked)
                        g.DrawLine(new Pen(Color.Green), network[i].x, network[i].y, network[network[i].edges[2]].x, network[network[i].edges[2]].y);
                }
                if ((!network[i].is_linked[3]) && (network[i].y < network[no_of_nodes - 1].y - vert_dist) && (i + widthstep < no_of_nodes))
                {
                    network[i].edges[3] = i + widthstep;
                    network[i + widthstep].edges[2] = i;
                    network[i].is_linked[3] = true;
                    network[i + widthstep].is_linked[2] = true;
                    if (this.checkBox3.Checked)
                        g.DrawLine(new Pen(Color.Green), network[i].x, network[i].y, network[network[i].edges[3]].x, network[network[i].edges[3]].y);

                }
                pictureBox1.Image = b;


            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            set_mode_parametres();
            initialise();
	        init_graph();
            g.Clear(Color.Black);
            if (graph_type <=1)
            {
                if(max_no_of_edges<4)
                System.Windows.Forms.MessageBox.Show("Lattice structure of graph comprises of a node connected to its four neighbours. The max no of edges is presently less than 4.Please make it 4 or else choose random graph model.");
                else
                connect_lattice_graph();
            }
            else
            {
                if (this.radioButton13.Checked)
                {
                    con_random.connect_network(this, true);
                    is_edges_maximum = true;
                }
                else
                {
                    con_random.connect_network(this, false);
                    is_edges_maximum = false;
                }
                if (this.checkBox3.Checked)
                {
                    Console.Write("Checking connections\n");
                    for (int i = 0; i < no_of_nodes; i++)
                    {
                        if (!network[i].is_connected)
                            Console.Write(i + " : Not connected to the network\n");
                        for (int j = 0; j < network[i].no_of_connections; j++)
                        {
                            g.DrawLine(new Pen(Color.Green), network[i].x, network[i].y, network[network[i].edges[j]].x, network[network[i].edges[j]].y);
                            this.pictureBox1.Image = b;
                        }
                    }
                }
                
            }
                 this.button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            head = 0; tail = 0; time = 1;
            this.progressBar1.Enabled = true;
            mode = this.comboBox2.SelectedIndex;
            this.progressBar1.Value = 0;
            this.progressBar2.Value = 0;
            stop_flag = false;
            for (int i = 0; i < rate_of_network_growth; i++)
                network[i].is_alive = true;
            no_of_alive_nodes = rate_of_network_growth;
            this.progressBar2.Value += rate_of_network_growth;
             if(ex.check_exceptions(this))
            {
                packet_distribution = new int[desired_time, no_of_packets + 1];
                packet_distribution1 = new int[desired_time, no_of_packets + 1];
                prevalence_arr = new int[desired_time];
                incidence_arr = new int[desired_time];
                enqueue_rand(seeder_nodes);
                this.button7.Enabled = true;
                transfer_data();
                //     this.progressBar1.Value = this.progressBar1.Maximum;
                this.progressBar1.Enabled = false;
                this.button7.Enabled = false;
                max_time = time;
                this.richTextBox1.Text = Convert.ToString(time);
                this.button2.Enabled = false;
                this.button3.Enabled = true;
                this.button4.Enabled = true;
                this.pictureBox5.Enabled = true;
                this.pictureBox6.Enabled = true;
                this.pictureBox7.Enabled = true;
                this.button5.Enabled = true;
                this.button6.Enabled = true;
                Console.Write("Time : " + time + "\n");
                print_array(incidence_arr, 0, time);
                Console.Write("Prevalance array : " + "\n");
                print_array(prevalence_arr, 0, time);
                if (this.checkBox4.Checked)
                {
                    log.write_log_file(this,this.comboBox6.Text);
                }
                if(this.checkBox5.Checked)
                chart.draw_charts(incidence_arr, time, 1, "incidencearr.xls");
                if(this.checkBox6.Checked)
                chart.draw_charts(prevalence_arr, time, 2, "prevalancearr.xls");
                 
            }
    //       
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = this.trackBar1.Value;
            ratio = (value < 50) ? (float)((value/5)*1.0/ 10) : (value - 50) / 5 + 1;

            this.label6.Text = Convert.ToString(ratio);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            max_value1 = 1;
            int max_time = time;
            for (int i = 0; i < time; i++)
                if (incidence_arr[i] > max_value1)
                    max_value1 = incidence_arr[i];
            Console.Write("\nMax Value : " + max_value1+"\n");
            draw_plot(incidence_arr, time, g1,max_value1,panel1_width,panel1_height,2,0);
            this.pictureBox5.Image = b1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            g3.Clear(Color.White);
            int max_time = time, max_value = 0;
            draw_plot(prevalence_arr,time, g2, no_of_nodes,panel1_width,panel1_height,2,1);
            this.pictureBox6.Image = b2;
            prevalence_flag = true;
            Console.Write("\nPacket Distribution  Time = " +test_count+ "\n");
            for (int i = 0; i <= no_of_packets; i++)
            {
                Console.Write(packet_distribution[test_count, i] + " *** ");
            }
            Console.Write("\n");
            for (int i = 0; i <= no_of_packets; i++)
            {
                Console.Write(packet_distribution1[test_count, i] + " *** ");
            }
            test_count++;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton11.Checked)
            {
                is_network_static = true;
                this.trackBar3.Enabled = false;
                rate_of_network_growth = no_of_nodes;
            }
            else if (this.radioButton12.Checked)
            {
                this.trackBar3.Enabled = true;
                is_network_static = false;
                this.trackBar3.Value = 10;
                rate_of_network_growth = this.trackBar3.Value;
                this.label17.Text = "10";
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.label17.Text = Convert.ToString(this.trackBar3.Value);
            rate_of_network_growth = this.trackBar3.Value;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox3.SelectedIndex == 0)
            {
                eligible_from_seedeer_nodes = -1;
                this.textBox4.Visible = false;

            }
            else if (this.comboBox3.SelectedIndex == 1)
            {
                eligible_from_seedeer_nodes = widthstep;
                this.textBox4.Visible = true;
                this.textBox4.Text = Convert.ToString(widthstep);
                    
            }
            else if (this.comboBox3.SelectedIndex == 2)
            {
                this.textBox4.Visible = true;
                eligible_from_seedeer_nodes = Convert.ToInt32(this.textBox4.Text);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            eligible_from_seedeer_nodes = Convert.ToInt32(this.textBox4.Text);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            desired_time = this.trackBar4.Value;
            this.label27.Text = Convert.ToString(desired_time);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            random_node = Convert.ToInt32(this.textBox5.Text);
            if (random_node > no_of_nodes - 1)
                MessageBox.Show("The index of random node is greater than number of nodes minus 1.Please enter a value less than" + Convert.ToString(no_of_nodes));
            else if ((this.comboBox5.SelectedIndex == 1)&&(mode == 3))
            {
                for (int i = 0; i < no_of_packets; i++)
                {
                    sorted_packets_arr[i] = network[random_node].packets[i];
                }
                for (int i = 0; i < no_of_packets; i++)
                {
                    for (int j = i; j < no_of_packets; j++)
                    {
                        
                        if (sorted_packets_arr[j] < sorted_packets_arr[i])
                        {
                            int t = sorted_packets_arr[j];
                            sorted_packets_arr[j] = sorted_packets_arr[i];
                            sorted_packets_arr[i] = t;
                        }
                    }
                }
                print_array(sorted_packets_arr, 0, no_of_packets);
                draw_plot(sorted_packets_arr,no_of_packets, g4,time, panel4_width, panel4_height, 4, 3);
                per_node_flag = true;

            }
            else
            {
                print_array(network[random_node].packets, 0, no_of_packets);
                draw_plot(network[random_node].packets, no_of_packets, g4, time, panel4_width, panel4_height, 4, 3);
                per_node_flag = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            random_node = rand.Next(no_of_nodes -1);
            this.textBox5.Text = Convert.ToString(random_node);
            g4.Clear(Color.White);
            if (random_node > no_of_nodes - 1)
                MessageBox.Show("The index of random node is greater than number of nodes minus 1.Please enter a value less than" + Convert.ToString(no_of_nodes));
            else if ((this.comboBox5.SelectedIndex == 1) && (mode == 3))
            {
                for (int i = 0; i < no_of_packets; i++)
                {
                    sorted_packets_arr[i] = network[random_node].packets[i];
                }
                for (int i = 0; i < no_of_packets; i++)
                {
                    for (int j = i; j < no_of_packets; j++)
                    {

                        if (sorted_packets_arr[j] < sorted_packets_arr[i])
                        {
                            int t = sorted_packets_arr[j];
                            sorted_packets_arr[j] = sorted_packets_arr[i];
                            sorted_packets_arr[i] = t;
                        }
                    }
                }
                Console.Write("Printing Sorted array\n");
                print_array(sorted_packets_arr, 0, no_of_packets);
                
                draw_plot(sorted_packets_arr, no_of_packets, g4, time, panel4_width, panel4_height, 4, 3);
                this.pictureBox8.Image = b4;
                per_node_flag = true;
            }
            else
            {
                print_array(network[random_node].packets, 0, no_of_packets);
                draw_plot(network[random_node].packets, no_of_packets, g4, time, panel4_width, panel4_height, 4, 3);
                this.pictureBox8.Image = b4;
            }
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                max_running_time = Convert.ToInt32(this.textBox6.Text);
            }
            catch (Exception ex)
            {
                max_running_time = 2 * no_of_nodes;
            }
            this.trackBar4.Maximum = max_running_time;
            this.trackBar4.Value  = max_running_time;
            this.label27.Text = Convert.ToString(this.trackBar4.Value);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            stop_flag = true;
            this.button2.Enabled = true;
            this.button7.Enabled = false;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((acc_density_flag) || (packet_distribution_flag))
            {
                g3.Clear(Color.White);
                if (this.comboBox4.SelectedIndex == 0)
                {
                    pac_arr = new int[21];
                    int packets_per_interval = no_of_packets / 21 + 1;
                    for (int i = 0; i < 21; i++)
                        for (int j = 0; j < packets_per_interval; j++)
                        {
                            if ((j + i * packets_per_interval) > no_of_packets)
                                break;
                            pac_arr[i] += packet_distribution[plotting_time, j + i * packets_per_interval];
                        }
                    print_array(pac_arr, 0, 21);
                    draw_bargraph(pac_arr, 21, g3, no_of_nodes, panel2_width, panel2_height);
                    this.pictureBox7.Image = b3;
                    packet_distribution_flag = true;
                }
                else
                {
                    accu_density_arr[0] = packet_distribution[plotting_time, 0];
                    for (int i = 1; i < no_of_packets; i++)
                    {
                        accu_density_arr[i] = accu_density_arr[i - 1] + packet_distribution[plotting_time, i];
                    }
                    print_array(accu_density_arr, 0, no_of_packets);
                    draw_plot(accu_density_arr, no_of_packets, g3, no_of_nodes, panel2_width, panel2_height, 2, 2);
                    this.pictureBox7.Image = b3;
                    acc_density_flag = true;
                }
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            g4.Clear(Color.White);
            if (per_node_flag)
            {
                if ((this.comboBox5.SelectedIndex == 1) && (mode == 3))
                {
                    for (int i = 0; i < no_of_packets; i++)
                    {
                        sorted_packets_arr[i] = network[random_node].packets[i];
                    }
                    for (int i = 0; i < no_of_packets; i++)
                    {
                        for (int j = i; j < no_of_packets; j++)
                        {

                            if (sorted_packets_arr[j] < sorted_packets_arr[i])
                            {
                                int t = sorted_packets_arr[j];
                                sorted_packets_arr[j] = sorted_packets_arr[i];
                                sorted_packets_arr[i] = t;
                            }
                        }
                    }
                    print_array(sorted_packets_arr, 0, no_of_packets);
                    draw_plot(sorted_packets_arr, no_of_packets, g4, time, panel4_width, panel4_height, 4, 3);
                    this.pictureBox8.Image = b4;
                    per_node_flag = true;

                }
                else
                {
                    print_array(network[random_node].packets, 0, no_of_packets);
                    draw_plot(network[random_node].packets, no_of_packets, g4, time, panel4_width, panel4_height, 4, 3);
                    this.pictureBox8.Image = b4;
                    per_node_flag = true;
                }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.label11.Text = Convert.ToString(this.trackBar2.Value * 1.0/ 100);
            threshold_probability = this.trackBar2.Value;
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            graph_type = this.comboBox6.SelectedIndex;
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            h.showHelp(0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            h.showHelp(1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            h.showHelp(2);
        }

      
        private void pictureBox5_MouseMove_1(object sender, MouseEventArgs e)
        {
            this.label19.Text = "Time";
            if (max_value1 > 0)
            {
                this.label21.Text = Convert.ToString((int)(e.X * 1.0 / barwidth[0]));
                this.label22.Text = Convert.ToString(incidence_arr[(int)(e.X / barwidth[0])]);
            }
        }

        private void pictureBox6_MouseMove_1(object sender, MouseEventArgs e)
        {
            this.label19.Text = "Time";
            if (prevalence_flag)
            {
                this.label21.Text = Convert.ToString((int)(e.X * 1.0 / barwidth[1]));
                this.label22.Text = Convert.ToString(prevalence_arr[(int)(e.X / barwidth[1])]);
            }
        }

        private void pictureBox6_MouseClick_1(object sender, MouseEventArgs e)
        {
            Console.Write("X Position : " + e.X + "\n");
            plotting_time = (int)(time * 1.0 * e.X / panel1_width);
            Console.Write("time : " + plotting_time + "\n");
            if (this.comboBox4.SelectedIndex == 0)
            {
                pac_arr = new int[21];
                int packets_per_interval = no_of_packets / 21 + 1;
                for (int i = 0; i < 21; i++)
                    for (int j = 0; j < packets_per_interval; j++)
                    {
                        if ((j + i * packets_per_interval) > no_of_packets)
                            break;
                        pac_arr[i] += packet_distribution[plotting_time, j + i * packets_per_interval];
                    }
                print_array(pac_arr, 0, 21);
                g3.Clear(Color.White);
                draw_bargraph(pac_arr, 21, g3, no_of_nodes, panel2_width, panel2_height);
                this.pictureBox7.Image = b3;
                packet_distribution_flag = true;
            }
            else
            {
                accu_density_arr[0] = packet_distribution[plotting_time, 0];
                for (int i = 1; i < no_of_packets; i++)
                {
                    accu_density_arr[i] = accu_density_arr[i - 1] + packet_distribution[plotting_time, i];
                }
                print_array(accu_density_arr, 0, no_of_packets);
                g3.Clear(Color.White);
                draw_plot(accu_density_arr, no_of_packets, g3, no_of_nodes, panel2_width, panel2_height, 2, 2);
                this.pictureBox7.Image = b3;
                acc_density_flag = true;
            } 
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.comboBox4.SelectedIndex == 0)
            {
                this.label19.Text = "Packets";
                if (packet_distribution_flag)
                {
                    int packets_per_interval = no_of_packets / 21 + 1;
                    if (packets_per_interval <= 1)
                        this.label21.Text = Convert.ToString((int)(e.X * 1.0 / 15));
                    else
                        this.label21.Text = Convert.ToString((e.X / 15) * packets_per_interval) + " - " + Convert.ToString((e.X / 15) * packets_per_interval + packets_per_interval - 1);
                    this.label22.Text = Convert.ToString(pac_arr[e.X / 15]);
                }
            }
            else
            {
                this.label19.Text = "Packets";
                if (acc_density_flag)
                {
                    this.label21.Text = Convert.ToString((int)(e.X * 1.0 / barwidth[2]));
                    this.label22.Text = Convert.ToString(accu_density_arr[(int)(e.X / barwidth[2])]);

                }
            }
        }

        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
            if (per_node_flag)
            {
                this.label31.Text = Convert.ToString((int)(e.X * 1.0 / barwidth[3] + 1));
                this.label32.Text = Convert.ToString(network[random_node].packets[(int)(e.X / barwidth[3])]);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.pictureBox7.Image.Save("packet_distribution.jpg");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.pictureBox8.Image.Save("packet_vs_time.jpg");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            h.showHelp(3);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            this.label25.Text = Convert.ToString(this.trackBar5.Value * 1.0 / 100);
            leaving_probability = this.trackBar5.Value;
   
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            h.showHelp(4);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = this.comboBox2.SelectedIndex;
            if (mode == 1)
            {
                this.radioButton8.Checked = true;
                this.groupBox2.Enabled = true;
                this.radioButton3.Checked = true;
                this.groupBox1.Enabled = true;
                alturists = 100;
                traders = 0;
                parasites = 0;
            }
            else if (mode == 0)
            {
                this.radioButton8.Checked = true;
                this.groupBox2.Enabled = true;
                this.radioButton3.Checked = true;
                this.groupBox1.Enabled = true;
                alturists = Convert.ToInt16(this.textBox9.Text);
                traders = Convert.ToInt16(this.textBox10.Text);
                parasites = Convert.ToInt16(this.textBox11.Text);
            }
            else if (mode == 2)
            {
                parasites = 100;
                traders = 0;
                alturists = 0;
                this.radioButton5.Checked = true;
                this.groupBox1.Enabled = false;
                this.groupBox2.Enabled = true;
            }
            else
            {
                this.groupBox2.Enabled = true;
                this.groupBox1.Enabled = true;
                traders = 100;
                alturists = 0;
                parasites = 0;
                this.groupBox2.Enabled = true;

            }
            this.textBox9.Text = Convert.ToString(alturists);
            this.textBox10.Text = Convert.ToString(traders);
            this.textBox11.Text = Convert.ToString(parasites);
            
        }

       
        
        
        

       
       

        
        

        

      

        
        
       
        




    }
}
