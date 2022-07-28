using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class Cluster
    {
        public int ClusterNumber, ClusterSize;//O(1)

        public Cluster(int clusterNumber)//O(1)
        {
            ClusterNumber = clusterNumber;//O(1)
            //ClusterSize = 1 because each Distencet Color is considered a separate cluster.
            ClusterSize = 1;//O(1)
        }
    }
}
