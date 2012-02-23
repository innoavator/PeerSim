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
using System.Linq;
using System.Text;

namespace P2P
{
    partial class Exceptions
    {
        private Form1 f;
        
        public bool check_exceptions(Form1 form)
        {
            this.f = form;
            if (f.mode == 2)
            {
                System.Windows.Forms.MessageBox.Show("Data spreading is not possible in the pure parasites model.Please choose another mode");
                return false;
            }
            else if ((f.graph_type <2) && (f.max_no_of_edges < 4))
            {
                System.Windows.Forms.MessageBox.Show("Lattice structure of graph comprises of a node connected to its four neighbours. The max no of edges is presently less than 4.Please make it 4 or else choose random graph model.");
                return false;
            }
       /*     else if ((f.graph_type == 1)||(f.graph_type == 3))
            {
                System.Windows.Forms.MessageBox.Show("This type of network has not yet been implemented.");
                return false;
            }
            */
            else if ((f.mode == 3) && (f.no_of_packets == 1))
            {
                System.Windows.Forms.MessageBox.Show("Traders model is not possible with 1 Packet of data.Please increase the no of packets");
                return false;
            }
            else if ((f.mode == 3) && (f.ratio < 0.4))
            {
                System.Windows.Forms.MessageBox.Show("Spread of data in the network will be too slow in the traders model with ratio < 0.5. Please increase the ratio of Seeder exchange speed to node exchange speed");
                return false;
            }
            else if ((f.seeder_packets > f.no_of_packets) || (f.seeder_packets == 0))
            {
                System.Windows.Forms.MessageBox.Show("No of seeder packets must be greater than 0 and less than or equal to no of packets.Please decrease the no of seeder packets");
                return false;
            }
            else if ((f.seeder_packets < f.no_of_packets) && (f.ratio == 0))
            {
                System.Windows.Forms.MessageBox.Show("Data Cannot Spread in the network with seeder givig only once(ratio = 0) and the no of seeder packets being less than the total no of packets.Please increase the ratio or make the no of seeder packets equal to the total no of packets.");
                return false;
            }
            else if ((f.no_of_download_edges == 0) && (f.ratio == 0))
            {
                System.Windows.Forms.MessageBox.Show("Data Cannot Spread in the network if number of download edges and ratio are both zero.Please increase the number of download edges and the artio.You may also try increasing the no of Seeder packets.");
                return false;
            }

            else if ((f.no_of_upload_edges == 0) && (f.ratio == 0))
            {
                System.Windows.Forms.MessageBox.Show("Data Cannot Spread in the network if number of upload edges and ratio are both zero.Please increase the number of upload edges and the ratio.You may also try increasing the no of Seeder packets.");
                return false;
            }
            else if (f.max_no_of_edges <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Please increase the max_no_of_edges above 0");
                return false;
            }
            else if((f.mode == 3)&&(f.eligible_from_seedeer_nodes != -1))
            {
                System.Windows.Forms.MessageBox.Show("In this case only the nodes eligible to receive nodes from the seeder will receive the packets.The other nodes will never receive the data. Please change the mode or the no of eligible nodes to all alive nodes");
                return false;
            }
            else if (f.parasites == 100)
            {
                System.Windows.Forms.MessageBox.Show("Data Spreading is not possible when all the nodes behave as parasites.Please increase thepercentage of alturists.");
                return false;
            }
            else if ((f.seeder_packets == 1) && (f.traders > 0)&&(f.ratio == 0))
            {
                System.Windows.Forms.MessageBox.Show("In case of only 1 packet of data and the ratio being zero,the traders might never get the data packet.");
                return true;
            }
            else
                return true;
        }
    }
}
