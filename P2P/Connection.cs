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
    class Connection
    {
        private Form1 f;

        public void connect_network(Form1 form, bool is_maximum)           //if 1 then generate maximum else random connections
        {
            int k;
            int cons;
            Random r = new Random();
            int no_of_repititions = 0;
            this.f = form;
            for (int i = 0; i < f.no_of_nodes; i++)
            {
                if (is_maximum)
                    cons = f.max_no_of_edges;
                else
                    cons = r.Next(1, f.max_no_of_edges);

                if (f.network[i].no_of_connections < cons)
                {
                    no_of_repititions = 0;

                    for (int j = f.network[i].no_of_connections; j < cons; )
                    {
                        if (no_of_repititions > 10)
                            break;
                        k = r.Next(f.no_of_nodes);
                        if (k == i)
                            continue;
                        if (f.network[k].no_of_connections < f.max_no_of_edges)
                        {
                            f.network[k].is_connected = true;
                            f.network[i].is_connected = true;
                            f.network[i].edges[j] = k;
                            f.network[i].no_of_connections++;
                            f.network[k].edges[f.network[k].no_of_connections] = i;
                            f.network[k].no_of_connections++;
                        }
                        else
                        {
                            no_of_repititions++;
                            continue;
                        }
                        j++;
                    }
                }

            }

        }
        public void connect_node(Form1 f, int index, bool is_maximum)
        {
            this.f = f;
            int no_of_repititions = 0, cons,k;
            Random r = new Random(4);
            if (is_maximum)
                cons = f.max_no_of_edges;
            else
                cons = r.Next(1, f.max_no_of_edges);
            for (int j = f.network[index].no_of_connections; j < cons; )
            {
                if (no_of_repititions > 25)
                    break;
                k = r.Next(f.no_of_nodes);
                if (k == index)
                    continue;
                if (f.network[k].no_of_connections < f.max_no_of_edges)
                {
                    f.network[k].is_connected = true;
                    f.network[index].is_connected = true;
                    f.network[index].edges[j] = k;
                    f.network[index].no_of_connections++;
                    f.network[k].edges[f.network[k].no_of_connections] = index;
                    f.network[k].no_of_connections++;
                }
                else
                {
                    no_of_repititions++;
                    continue;
                }
            }
        }

        public void allot_category(Form1 form,node[] network,int alturists,int traders,int parasites)
        {
            int index;
            this.f = form;
            Random r = new Random();
            for (int i = 0; i < alturists;)
            {
                index = r.Next() % f.no_of_nodes;
                if (network[index].category == 0)
                    network[index].category = 1;
                else continue;
                i++;
            }
            for (int i = 0; i < traders;)
            {
                index = r.Next()%f.no_of_nodes;
                if (network[index].category == 0)
                    network[index].category = 2;
                else
                    continue;
                i++;
            }
            for (int i = 0; i < f.no_of_nodes;i++ )
            {
                if (network[i].category == 0)
                {
                    network[i].category = 3;
                    network[i].upl_edges = 0;
                }
            }
        }
    }
}
