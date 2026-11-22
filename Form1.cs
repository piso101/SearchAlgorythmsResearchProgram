using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.IO;

namespace paradygmaty_programowania
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }
        public float TimeElapsedAverage=0;
        public float TimesRun=0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0 && trackBar1.Value != 0)
            {
                for (int i = 0; i < trackBar2.Value; i++)
                {
                    float value = Picker(trackBar1.Value, comboBox1.Text);
                    label5.Text = ((value / 100).ToString()) + " nanoSeconds";
                    TimeElapsedAverage += value;
                    TimesRun++;
                    label6.Text = ((TimeElapsedAverage / TimesRun) / 100).ToString() + " NanoSeconds";
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeElapsedAverage = 0;
            TimesRun = 0;
            if (comboBox1.Text != "LinearSearch")
            {
                if (comboBox1.Text == "BinarySearch")
                {
                    LoadSearchImage("Binary Search.png");
                }
                else if (comboBox1.Text == "JumpSearch")
                {
                    LoadSearchImage("JumpSearch.png");
                }
                else if (comboBox1.Text == "InterpolationSearch")
                {
                    LoadSearchImage("InterpolationSearch.png");
                }
                else if (comboBox1.Text == "MultiThreadingSearch")
                {
                    LoadSearchImage("MultiThreadSearch.png");
                }
            }
            else
            {
                LoadSearchImage("LinearSearch.png");
            }
        }

        private void LoadSearchImage(string imageName)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", imageName);
            pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);
        }

        public void LinearSearch(int[] arr, int x)
        {
            int n = arr.Length;
            int i;
            for (i = 0; i < n; i++)
            {
                if (arr[i] == x)
                {
                    Console.WriteLine("Element is present at index " + i);
                    break;
                }
            }
            if (i == n)
            {
                Console.WriteLine("Element is not present in array");
            }
        }

        //binary search
        public void BinarySearch(int[] arr, int x)
        {
            int l = 0, r = arr.Length - 1;
            while (l <= r)
            {
                int m = l + (r - l) / 2;

                // Check if x is present at mid
                if (arr[m] == x)
                {
                    Console.WriteLine("Element is present at index " + m);
                    break;
                }

                // If x greater, ignore left half
                if (arr[m] < x)
                {
                    l = m + 1;
                }

                // If x is smaller, ignore right half
                else
                {
                    r = m - 1;
                }
            }
        }

        //jump search
        public void JumpSearch(int[] arr, int x)
        {
            int n = arr.Length;
            
            // Finding block size to be jumped
            int step = (int)Math.Floor(Math.Sqrt(n));

            // Finding the block where element is
            // present (if it is present)
            int prev = 0;
            while (arr[Math.Min(step, n) - 1] < x)
            {
                prev = step;
                step += (int)Math.Floor(Math.Sqrt(n));
                if (prev >= n)
                {
                    Console.WriteLine("Element is not present in array");
                    return;
                }
            }

            // Doing a linear search for x in block
            // beginning with prev.
            while (arr[prev] < x)
            {
                prev++;

                // If we reached next block or end of
                // array, element is not present.
                if (prev == Math.Min(step, n))
                {
                    Console.WriteLine("Element is not present in array");
                    return;
                }
            }

            // If element is found
            if (arr[prev] == x)
            {
                Console.WriteLine("Element is present at index " + prev);
            }
        }

        //interpolation search
        public void InterpolationSearch(int[] arr, int x)
        {
            // Find indexes of two corners
            int lo = 0, hi = (arr.Length - 1);

            // Since array is sorted, an element present
            // in array must be in range defined by corner
            while (lo <= hi && x >= arr[lo] && x <= arr[hi])
            {
                // Probing the position with keeping
                // uniform distribution in mind.
                int pos = lo + (((hi - lo) / (arr[hi] - arr[lo])) * (x - arr[lo]));

                // Condition of target found
                if (arr[pos] == x)
                {
                    Console.WriteLine("Element found at index " + pos);
                    return;
                }

                // If x is larger, x is in upper part
                if (arr[pos] < x)
                {
                    lo = pos + 1;
                }

                // If x is smaller, x is in the lower part
                else
                {
                    hi = pos - 1;
                }
            }
        }

        //multithread search
        private readonly object lockObj = new object();

        private bool elementFound = false;

        public static void MultiThreadSearch(int[] array, int value)
        {
                Parallel.ForEach(Partitioner.Create(0, array.Length), (range, loopState) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        if (array[i] == value)
                        {
                            Console.WriteLine("Element found at index " + i);
                            loopState.Stop();
                        }
                    }
                });
        }

        public float Picker(int size, string whichone)
        {
            
            int SizeOfArray = size;
            int FindNumber = 100;
            int[] Table = new int[SizeOfArray];
            Random rnd = new Random();

            //Assing each Element of Aray to random number in range of 0 to 1000000
            for (int i = 0; i < SizeOfArray; i++)
            {
                Table[i] = rnd.Next(0, 1000000);
            }
            Stopwatch _stpWatch = new Stopwatch();

            if (whichone != "LinearSearch")
            {
                if (whichone == "BinarySearch")
                {

                    _stpWatch.Start();
                    Array.Sort(Table); // Binary Search need sorted array!!!!!
                    BinarySearch(Table, FindNumber);
                }
                else if (whichone == "JumpSearch")
                {

                    _stpWatch.Start();
                    Array.Sort(Table); // Jump Search need sorted array!!!!!
                    JumpSearch(Table, FindNumber);
                }
                else if (whichone == "InterpolationSearch")
                {
                    _stpWatch.Start();
                    Array.Sort(Table); // Interpolation Search need sorted array!!!!!
                    InterpolationSearch(Table, FindNumber);
                }
                else if (whichone == "MultiThreadingSearch")
                {
                    _stpWatch.Start();
                    MultiThreadSearch(Table, FindNumber);
                }
            }
            else
            {
                _stpWatch.Start();
                LinearSearch(Table, FindNumber);
            }
            _stpWatch.Stop();
            float result =_stpWatch.ElapsedTicks;
            return result;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label9.Text = trackBar2.Value.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}