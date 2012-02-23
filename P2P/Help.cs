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
    class Help
    {
        public void showHelp(int id)
        {
            switch (id)
            {
                case 0: System.Windows.Forms.MessageBox.Show("The max no of edges specify the maximum no of users to which one user can be connected at a time.The radio button labelled max, if checked, allots each user the maximum no of connections specified above.The random button on the other hand, generates a random number less than the maximum no of connections for each user and allots him that many connections.Please note that if we use max no of connections,then in the infinite model,then the new nodes entering the network can form connections to only those nodes to whom those leaving the network were connected since all the other nodes have been set to maximum connections.The options of maximum and random are not available in lattice network model as that model has fixed four edges.");
                        break;
                case 1: System.Windows.Forms.MessageBox.Show("Ratio of Seeder Exchange Speed to Node Exchange Speed . Ratio = 0 means the seeder gives only once.");
                        break;
                case 2: System.Windows.Forms.MessageBox.Show("This is the probability that a download or an upload at a particular connection is possible.This feature has been introduced to take into account network disturbances due to which downloads may fail at certain instants and also to enable random matching.Before every download a random probability is generated which if greater than or equal to the threshold probability set by us,the download is successful,else it fails.Thus if the threshold prob. is set to zero,every download is a successful download");
                        break;
                case 3: System.Windows.Forms.MessageBox.Show("This is the probability that a user after completing his download leaves the network and someone else takes his place. each time a user completes his download a random probability is generated which if greater than the threshold probability of leaving the network set by you,the user leaves the network and a new one takes its place.Thus if the probability is set to zero,every user leaves the network as soon as he completes his download without giving the packets to anyone else.");
                        break;
                case 4: System.Windows.Forms.MessageBox.Show("This program provides four choices of networks \n 1.Latttice network is a network where each node is connected to its four neighbours. \n In random network each node is randomly based on the number of edges settings.\n 2. In finite network, the nodes are present and the program runs till the time whe all nodes complete their downloads.In case of infinite networks, the program runs for infinite time unless it is stopped as every node after completing its download is reset to a new node. ");
                        break;
            }
        }
    }
}
