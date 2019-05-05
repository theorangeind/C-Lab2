using System;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class MainClass
    {
        Random rand = new Random();

        public void circTask(TextBox field, int N, bool detailed)
        {
            field.Text = "Сircles with area less than average:\r\n";

            Circle[] arr = new Circle[N];
            float sum = 0;

            for (int i = 0; i < N; i++)
            {
                arr[i] = new Circle((float)rand.Next(15) + 1);
                sum += arr[i].getArea();
            }

            float averageArea = sum / N;
            if (detailed) field.Text += "(Average area is " + averageArea + ")\r\n";
            field.Text += "\r\n";

            for (int i = 0; i < N; i++)
            {
                if(arr[i].getArea() < averageArea)
                {
                    field.Text += "CIRCLE " + (i + 1) + "\r\n";
                    if (detailed) field.Text += arr[i].info() + "\r\n";
                    field.Text += "\r\n";
                }
            }

            field.Text += "\r\nFull list:\r\n\r\n";
            for (int i = 0; i < N; i++)
            {
                field.Text += "CIRCLE " + (i + 1) + "\r\n" + arr[i].info() + "\r\n";
            }
        }

        public void coneTask(TextBox field, int M, bool detailed)
        {
            field.Text = "Сone with the largest volume:\r\n";

            Cone[] arr = new Cone[M];
            float max = 0;
            int result = 0;

            for (int i = 0; i < M; i++)
            {
                arr[i] = new Cone((float)rand.Next(15) + 1, (float)rand.Next(31) + 1);
                if (arr[i].getVolume() > max)
                {
                    max = arr[i].getVolume();
                    result = i;
                }
            }

            field.Text += "CONE " + (result + 1) + "\r\n";
            if (detailed) field.Text += arr[result].info(true);
            field.Text += "\r\n";

            field.Text +=  "\r\nFull list:\r\n\r\n";
            for(int i = 0; i < M; i++)
            {
                field.Text += "CONE " + (i + 1) + "\r\n" + arr[i].info(true) + "\r\n";
            }
		}

		public void WriteToFile(String path, TextBox field)
		{
			FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			BinaryWriter writer = new BinaryWriter(fileStream);

			writer.Write(field.Text);

			writer.Close();
			fileStream.Close();
		}

		public void ReadFromFile(String path, TextBox field)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader reader = new BinaryReader(fileStream);

			field.Text = reader.ReadString();

			reader.Close();
			fileStream.Close();
		}


		public void circTest(TextBox field)
		{
			Circle circ = new Circle((float)rand.Next(15) + 1);
			field.Text = "Circle info:\r\n" + circ.info();
		}

		public void coneTest(TextBox field)
		{
			Cone cone = new Cone((float)rand.Next(15) + 1, (float)rand.Next(31) + 1);
			field.Text = "Cone info:\r\n" + cone.info(true);
		}
	}

    class Circle
    {
        public float R; //радиус окружности

        public Circle(float radius)
        {
            this.R = radius;
        }

        //вычисление площади окружности
        public float getArea()
        {
            return (float)(this.R * this.R * Math.PI);
        }

        //вычисление длины окружности
        public float getLength()
        {
            return (float)(2 * this.R * Math.PI);
        }

        public string info()
        {
            return "Radius: " + R + "\r\nLength: " + getLength() + "\r\nArea: " + getArea() + "\r\n";
        }
    }

    class Cone : Circle
    {
        public float H; //высота конуса

        public Cone(float radius, float height) : base(radius)
        {
            this.H = height;
        }

        //вычисление площади поверхности конуса
        public new float getArea()
        {
            return (float)(base.getArea() + (R * Math.Sqrt(R * R + H * H) * Math.PI));
        }

        //вычисление площади поверхности конуса с помощью образующей
        public float getArea(float coneGen)
        {
            return (float)(R * coneGen * Math.PI + base.getArea());
        }

        //вычисление объема
        public float getVolume()
        {
            return (getArea() * this.H) / 3;
        }

        public string info()
        {
            return "Radius: " + R + "\r\nHeight: " + H + "\r\nVolume: " + getVolume() + "\r\n";
        }

        public string info(bool detailed)
        {
            if (!detailed) return info();
            else return info() + "Surface area: " + getArea() + "\r\n";
        }
    }
}
