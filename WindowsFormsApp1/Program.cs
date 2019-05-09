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

		Circle[] circles;
		Cone[] cones;

		int writableTask = 0; //1 - окружности, 2 - конусы
		bool details = false;

		public void circTask(TextBox field, int N, bool detailed)
        {
			float sum = 0;

			if(circles == null)
			{
				circles = new Circle[N];
				for (int i = 0; i < N; i++)
				{
					circles[i] = Circle.generateRandom();
					sum += circles[i].getArea();
				}
			}
			else
			{
				for (int i = 0; i < circles.Length; i++)
				{
					sum += circles[i].getArea();
				}
			}
            float averageArea = sum / circles.Length;


			field.Text = "Сircles with area less than average:\r\n";
			if (detailed) field.Text += "(Average area is " + averageArea + ")\r\n";
            field.Text += "\r\n";
			//вывод окружностей, соответствующих заданию
            for (int i = 0; i < circles.Length; i++)
            {
                if(circles[i].getArea() < averageArea)
                {
                    field.Text += "CIRCLE " + (i + 1) + "\r\n";
                    if (detailed) field.Text += circles[i].info() + "\r\n";
                    field.Text += "\r\n";
                }
            }
			//вывод полного списка
            field.Text += "\r\nFull list:\r\n\r\n";
            for (int i = 0; i < circles.Length; i++)
            {
                field.Text += "CIRCLE " + (i + 1) + "\r\n" + circles[i].info() + "\r\n";
            }

			details = detailed;
			writableTask = 1;
        }

        public void coneTask(TextBox field, int M, bool detailed)
        {
			float max = 0;
            int result = 0;

			if(cones == null)
			{
				cones = new Cone[M];
				for (int i = 0; i < M; i++)
				{
					cones[i] = Cone.generateRandom();
				}
			}

			//поиск конуса, соответствующего заданию
            for (int i = 0; i < cones.Length; i++)
            {
                if (cones[i].getVolume() > max)
                {
                    max = cones[i].getVolume();
                    result = i;
                }
            }


			field.Text = "Сone with the largest volume:\r\n";
			field.Text += "CONE " + (result + 1) + "\r\n";
            if (detailed) field.Text += cones[result].info(true);
            field.Text += "\r\n";
            field.Text +=  "\r\nFull list:\r\n\r\n";
            for(int i = 0; i < cones.Length; i++)
            {
                field.Text += "CONE " + (i + 1) + "\r\n" + cones[i].info(true) + "\r\n";
            }

			details = detailed;
			writableTask = 2;
		}

		public void WriteToFile(String path, TextBox field)
		{
			FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			BinaryWriter writer = new BinaryWriter(fileStream);

			if(writableTask == 1)
			{
				writer.Write(writableTask);
				writer.Write(circles.Length);
				writer.Write(details);
				foreach(Circle item in circles)
				{
					writer.Write((double)item.R);
				}
			}
			else if (writableTask == 2)
			{
				writer.Write(writableTask);
				writer.Write(cones.Length);
				writer.Write(details);
				foreach (Cone item in cones)
				{
					writer.Write((double)item.R);
					writer.Write((double)item.H);
				}
			}

			writer.Close();
			fileStream.Close();
		}

		public void ReadFromFile(String path, TextBox field)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader reader = new BinaryReader(fileStream);

			int task = reader.ReadInt32();

			if (task == 1)
			{
				int length = reader.ReadInt32();
				bool detailedInfo = reader.ReadBoolean();
				Circle[] tempArr = new Circle[length];

				int i = 0;
				while(fileStream.CanRead && i < length)
				{
					tempArr[i] = new Circle((float)reader.ReadDouble());
					i++;
				}

				circles = tempArr;

				circTask(field, length, detailedInfo);
			}
			else if (task == 2)
			{
				int length = reader.ReadInt32();
				bool detailedInfo = reader.ReadBoolean();
				Cone[] tempArr = new Cone[length];

				int i = 0;
				while (fileStream.CanRead && i < length)
				{
					float radius = (float)reader.ReadDouble();
					float height = (float)reader.ReadDouble();
					tempArr[i] = new Cone(radius, height);
					i++;
				}

				cones = tempArr;

				coneTask(field, length, detailedInfo);
			}

			reader.Close();
			fileStream.Close();
		}


		public void circTest(TextBox field)
		{
			Circle circ = Circle.generateRandom();
			field.Text = "Circle info:\r\n" + circ.info();
		}

		public void coneTest(TextBox field)
		{
			Cone cone = Cone.generateRandom();
			field.Text = "Cone info:\r\n" + cone.info(true);
		}

		public void clearInfo()
		{
			circles = null;
			cones = null;

			writableTask = 0;
			details = false;
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

		//создает случайную окружность
		public static Circle generateRandom()
		{
			Random rand = new Random();
			return new Circle((float)rand.Next(15) + 1);
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
        public float getArea()
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

		//создает случайный конус
		public static Cone generateRandom()
		{
			Random rand = new Random();
			return new Cone((float)rand.Next(15) + 1, (float)rand.Next(31) + 1);
		}
	}
}
