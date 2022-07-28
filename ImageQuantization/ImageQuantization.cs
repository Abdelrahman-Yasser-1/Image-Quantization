using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class ImageQuantization
    {
        public static List<int>[] adjacencyList;//O(1)
        public static bool[] visited;//o(1)
        public static RGBPixel[,,] color_palette = new RGBPixel[256, 256, 256];//O(1)
        public static Dictionary<int, int> color_cluster = new Dictionary<int, int>();//O(1)
        public static Dictionary<int, RGBPixel> cluster_color = new Dictionary<int, RGBPixel>();//O(1)
        public static RGBPixelD cluster_sum = new RGBPixelD();//O(1)
        public static int cluster_count = 0;//O(1)

        public static List<RGBPixel> Finding_Distinct_Colors(RGBPixel[,] ImageMatrix)
        {
            int height = ImageOperations.GetHeight(ImageMatrix), width = ImageOperations.GetWidth(ImageMatrix);//O(1)

            bool[,,] taken = new bool[256, 256, 256];//O(1)

            List<RGBPixel> colors = new List<RGBPixel>();//O(1)

            for (int i = 0; i < height; i++)//O(N) * body(O(N)) = O(N^2) --> N = Height == Width
            {
                for (int j = 0; j < width; j++)//O(N) * body(O(1)) = O(N) --> N = Height == Width
                {
                    int red = ImageMatrix[i, j].red, green = ImageMatrix[i, j].green, blue = ImageMatrix[i, j].blue;//O(1)
                    if (!taken[red, green, blue])//O(1)
                    {
                        taken[red, green, blue] = true;//O(1)
                        colors.Add(ImageMatrix[i, j]);//O(1)
                    }
                }
            }

            return colors;//O(1)
            //Total complexity = O(N^2)
        }
        
        public static double Calculate_Distances(RGBPixel pixel1, RGBPixel pixel2)
        {
            return Math.Sqrt(((pixel1.red - pixel2.red) * (pixel1.red - pixel2.red)) 
                           + ((pixel1.green - pixel2.green) * (pixel1.green - pixel2.green)) 
                           + ((pixel1.blue - pixel2.blue) * (pixel1.blue - pixel2.blue)));//O(1)

            //Total complexity = O(1)
        }
        
        public static List<Edge> Find_the_minimum_spanning_tree(List<RGBPixel> DistinctColors, ref Edge[] MSTResult)
        {
            int size = DistinctColors.Count;//O(1)
            bool[] heapFlag = new bool[size];//O(1)
            Edge[] edges = new Edge[size];//O(1)
            double[] key = new double[size];//O(1)
            MinHeap minHeap = new MinHeap(DistinctColors.Count);//O(1)

            for (int i = 0; i < size; i++)//O(V) * body(O(log(V))) = O(Vlog(V)) --> V = Number of Distinct Colors
            {
                if (i == 0)
                    minHeap.Add(new HeapNode(i, 0));//O(log(V))
                else
                    minHeap.Add(new HeapNode(i, double.MaxValue));//O(log(V))

                edges[i] = new Edge();//O(1)
                heapFlag[i] = true;//O(1)
                key[i] = double.MaxValue;//O(1)
            }

            while (minHeap.size != 0)//O(V)
            {
                HeapNode root = minHeap.GetRoot();//O(log(V))
                heapFlag[root.vertex] = false;//O(1)
                for (int i = 0; i < size; i++)//O(V)
                {
                    if (heapFlag[i])//O(1)
                    {
                        double CalculatedKey = Calculate_Distances(DistinctColors[root.vertex], DistinctColors[i]);//O(1)
                        if (key[i] > CalculatedKey)
                        {
                            minHeap.ChangeKey(i, CalculatedKey);//O(log(V))
                            edges[i].source = root.vertex;//O(1)
                            edges[i].weight = CalculatedKey;//O(1)
                            edges[i].destination = i;//O(1)
                            key[i] = CalculatedKey;//O(1)
                        }
                    }
                }
            }
            MSTResult = edges;//O(1)
            return new List<Edge>(edges);//O(1)

            //Total complexity = O(Elog(V)) 
        }

        private static void DFS(List<int>[] adjacencyList, int index, List<RGBPixel> DistinctColors, int clusterNumber)
        {
            visited[index] = true;//O(1)
            for (int i = 0; i < adjacencyList[index].Count; i++)//O(V) --> V = Number of Distinct Colors
            {
                if (!visited[adjacencyList[index][i]])//O(1)
                {
                    color_cluster[adjacencyList[index][i]] = color_cluster[index];//O(1)
                    
                    cluster_sum.red += DistinctColors[adjacencyList[index][i]].red;//O(1)
                    cluster_sum.green += DistinctColors[adjacencyList[index][i]].green;//O(1)
                    cluster_sum.blue += DistinctColors[adjacencyList[index][i]].blue;//O(1)
                    cluster_count++;//O(1)

                    DFS(adjacencyList, adjacencyList[index][i], DistinctColors, clusterNumber);
                }
            }
            RGBPixel pixel = new RGBPixel();//O(1)
            pixel.red = (byte)(cluster_sum.red / cluster_count);//O(1)
            pixel.green = (byte)(cluster_sum.green / cluster_count);//O(1)
            pixel.blue = (byte)(cluster_sum.blue / cluster_count);//O(1)
            cluster_color[clusterNumber] = pixel;//O(1)
            
            //Total complexity = O(V+E)
        }
        
        public static void Extract_the_K_clusters(int K, List<RGBPixel> DistinctColors, List<Edge> edges)
        {
            adjacencyList = new List<int>[DistinctColors.Count];//O(1)
            visited = new bool[DistinctColors.Count];//O(1)

            //Cut maximum k-1 edges.
            for (int i = 0; i < K - 1; i++)//O(K) * body (O(D)) --> O(KD)
            {
                double max = -1;//O(1)
                int maxIndex = -1;//O(1)
                for (int j = 0; j < edges.Count; j++) //O(D)
                {
                    if (visited[edges[j].destination] == false && edges[j].weight > max)//O(1)
                    {
                        max = edges[j].weight;//O(1)
                        maxIndex = j;//O(1)
                    }
                }
                edges.RemoveAt(maxIndex);//O(1)
            }

            for (int i = 0; i < DistinctColors.Count; i++) //O(D)
            {
                adjacencyList[i] = new List<int>(DistinctColors.Count);//O(1)
            }

            for (int i = 0; i < edges.Count; i++)//O(D)
            {
                adjacencyList[edges[i].destination].Add(edges[i].source);//O(1)
                adjacencyList[edges[i].source].Add(edges[i].destination);//O(1)
            }
           
            for (int i = 0; i < DistinctColors.Count; i++)//O(D) * body(O(V+E))
            {
                if (!visited[i])//O(1) //the code in this block will execute k times only.
                {
                    cluster_sum.red = DistinctColors[i].red;//O(1)
                    cluster_sum.green = DistinctColors[i].green;//O(1)
                    cluster_sum.blue = DistinctColors[i].blue;//O(1)
                    cluster_count = 1;//O(1)
                    color_cluster[i] = i;//O(1)
                    int clusterNumber = i;//O(1)
                    DFS(adjacencyList, i, DistinctColors, clusterNumber);//O(V+E)
                }
            }

            //Total complexity = O(K*D)
        }

        public static RGBPixel[,,] Find_representative_color(List<RGBPixel> DistinctColors)
        {
            RGBPixel[,,] color_palette = new RGBPixel[256, 256, 256];//O(1)
            for (int i = 0; i < DistinctColors.Count; i++)//O(D)
            {
                color_palette[DistinctColors[i].red, DistinctColors[i].green, DistinctColors[i].blue] = cluster_color[color_cluster[i]];//O(1)
            }
            return color_palette;//O(1)

            //Total complexity = O(D)
        }

        public static RGBPixel[,] Quantize_the_image(RGBPixel[,] ImageMatrix, int k, ref List<RGBPixel> Distinct, ref Edge[] MST_Result)
        {
            //Console.WriteLine("\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a");
            //Console.WriteLine("start extract DistinctColors");
            //long timeBefore = System.Environment.TickCount;
            List<RGBPixel> colors = Finding_Distinct_Colors(ImageMatrix);//O(N^2)
            //long timeAfter = System.Environment.TickCount;
            //TimeSpan t = TimeSpan.FromMilliseconds(timeAfter - timeBefore);
            //string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            //Console.WriteLine("End extract DistinctColors with time: " + time);
            Distinct = colors;
            
            //Console.WriteLine("Start MST");
            //timeBefore = System.Environment.TickCount;
            List<Edge> edges = Find_the_minimum_spanning_tree(colors, ref MST_Result);//O(ElogV)
            //timeAfter = System.Environment.TickCount;
            //t = TimeSpan.FromMilliseconds(timeAfter - timeBefore);
            //time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            //Console.WriteLine("End MST with time: " + time);
            
            //Console.WriteLine("Start Clusturing");
            //timeBefore = System.Environment.TickCount;
            Extract_the_K_clusters(k, colors, edges);//O(K*D)
            //timeAfter = System.Environment.TickCount;
            //t = TimeSpan.FromMilliseconds(timeAfter - timeBefore);
            //time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            //Console.WriteLine("End Clusturing with time: " + time);

            color_palette = Find_representative_color (colors);//O(N^2)
            //Console.WriteLine("Start mapping");
            //timeBefore = System.Environment.TickCount;
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)//O(N) * body(O(N)) = O(N^2) --> N = Height == Width
            {
                for (int j = 0; j < ImageMatrix.GetLength(1); j++)//O(N) * body(O(1)) = O(N) --> N = Height == Width
                {
                    ImageMatrix[i, j] = color_palette[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue];//O(1)
                }
            }
            //timeAfter = System.Environment.TickCount;
            //time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            //Console.WriteLine("End color_paletteping with time: " + time);
            
            //Console.WriteLine("\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a\a");

            return ImageMatrix;

            //Total complexity = O(N^2)
        }
    }
}
