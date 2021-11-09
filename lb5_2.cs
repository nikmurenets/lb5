using System;

namespace Laba5_2
{
    public class Cone
    {
        public float Height { get; protected set; } = 0f;
        public float BaseRadius { get; protected set; } = 0f;

        public virtual void ReadParams()
        {
            Console.Write("Высота: ");
            Height = float.Parse(Console.ReadLine());

            Console.Write("Радиус основания: ");
            BaseRadius = float.Parse(Console.ReadLine());
        }

        public virtual void PrintParams()
        {
            Console.WriteLine($"Высота: {Height}");
            Console.WriteLine($"Радиус основания: {BaseRadius}");
        }

        public virtual double CalculateVolume()
        {
            return 1d / 3d * Math.PI * BaseRadius * BaseRadius * Height;
        }

        public double CalculateLowerBaseSquare()
        {
            return Math.PI * BaseRadius * BaseRadius;
        }
    }

    // Усеченный конус
    public class Frustum : Cone
    {
        public float UpperBaseRadius { get; protected set; }

        public override void ReadParams()
        {
            base.ReadParams();

            Console.Write("Радиус верхнего основания: ");
            UpperBaseRadius = float.Parse(Console.ReadLine());
        }

        public override void PrintParams()
        {
            base.PrintParams();

            Console.WriteLine(
                $"Радиус верхнего основания: {UpperBaseRadius}"
            );
        }

        public override double CalculateVolume()
        {
            double Sqr(double a)
            {
                return a * a;
            }

            return
                1d / 3d * Math.PI * Height * (
                    Sqr(UpperBaseRadius) +
                    UpperBaseRadius * BaseRadius +
                    Sqr(BaseRadius)
                );
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.InputEncoding =
                    Console.OutputEncoding =
                        System.Text.Encoding.Unicode;

                Console.WriteLine("Конус или усеченный конус?");
                Console.WriteLine("1 - Конус");
                Console.WriteLine("2 - Усеченный конус");
                Console.WriteLine();
                Console.Write("Ваш выбор: ");

                int answer = int.Parse(Console.ReadLine());

                Cone cone = null;
                if (answer == 1)
                    cone = new Cone();
                else if (answer == 2)
                    cone = new Frustum();
                else
                    throw new Exception("Неизвестный выбор");

                Console.WriteLine();
                Console.WriteLine("Введите параметры фигуры");
                cone.ReadParams();

                Console.WriteLine();
                Console.WriteLine("Введенные параметры:");
                cone.PrintParams();

                Console.WriteLine();
                Console.WriteLine($"Обьем фигуры: {cone.CalculateVolume()}");
                Console.WriteLine(
                    $"Площадь нижнего основания: {cone.CalculateLowerBaseSquare()}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
