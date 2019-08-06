using Refactoring.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Refactoring
{
    class Program
    {
        static IDictionary<string, Course> courses = new Dictionary<string, Course>();

        static void Main(string[] args)
        {
            Console.WriteLine("Wellcome to Refactoring Example");
          
            courses.Add("dpattern", new Course() { Name = "Design Pattern", Type = Types.Software });
            courses.Add("hface", new Course() { Name = "Human Face", Type = Types.Art });
            courses.Add("redis", new Course() { Name = "Redis", Type = Types.Software });

            Invoice invoice = new Invoice();
            invoice.customerName = "Hasel Team";
            invoice.registers = new Register[]{
                new Register(){courseID="dpattern",student=20},
                new Register() { courseID = "hface", student = 15 },
                new Register() { courseID = "redis", student = 5 },
            };

            decimal totalAmount = 0;
            decimal volumeCredits = 0;
            var result = $"{invoice.customerName} için Fatura Detayı: \n";

            CultureInfo trFormat = new CultureInfo("tr-TR", false);
            trFormat.NumberFormat.CurrencySymbol = "TL";
            trFormat.NumberFormat.NumberDecimalDigits = 2;

            foreach (Register reg in invoice.registers)
            {
                int thisAmount = GetAmount(reg, FindCourse(reg));
                //kazanılan para puan
                volumeCredits += Math.Max(reg.student - 15, 0);

                // extra bonus para puan her 5 yazılım öğrencisi için
                decimal fiveStudentGroup = reg.student / 5;
                if (Types.Software == FindCourse(reg).Type) volumeCredits += Math.Floor(fiveStudentGroup);

                // her bir şiparişin fiyatı
                result += $"{FindCourse(reg).Name}: {(thisAmount / 100).ToString("C", trFormat)} ({reg.student} kişi)\n";
                totalAmount += thisAmount;
            }
            result += $"Toplam borç { (totalAmount / 100).ToString("C", trFormat)}\n";
            result += $"Kazancınız { volumeCredits.ToString("C", trFormat) } \n";
            Console.WriteLine(result);
            Console.ReadLine();
        }

        private static int GetAmount(Register register, Course lesson)
        {
            var result = 0;

            switch (lesson.Type)
            {
                case Types.Art:
                    {
                        result = 3000;
                        if (register.student > 15)
                        {
                            result += 1000 * (register.student - 10);
                        }
                        break;
                    }
                case Types.Software:
                    {
                        result = 30000;
                        if (register.student > 10)
                        {
                            result += 10000 + 500 * (register.student - 5);
                        }
                        result += 300 * register.student;
                        break;
                    }
            }

            return result;
        }

        public static Course FindCourse(Register register)
        {
            return courses[register.courseID];
        }
    }
}
