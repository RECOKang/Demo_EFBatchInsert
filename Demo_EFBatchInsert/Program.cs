using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_EFBatchInsert
{
    class Program
    {

        static void Main(string[] args)
        {
            var testData = GetSample(3000);
            Demo(TestRun01, testData);
            Demo(TestRun02, testData);
            Demo(TestRun03, testData);
            Demo(TestRun04, testData);
            Console.ReadLine();
        }

        private static IEnumerable<Users> GetSample(int ItemCount)
        {
            var randomchar = "0123456789abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ";
            var rand = new Random();

            for (int i = 1; i <= ItemCount; i++)
            {
                yield return new Users
                {
                    UID = i,
                    Id = i,
                    Name = new string(Enumerable.Repeat(randomchar, 10).Select(x => x[rand.Next(x.Length)]).ToArray())
                };
            }
        }

        private static void Demo(Action<IEnumerable<Users>> fun, IEnumerable<Users> userData)
        {
            var sw = new Stopwatch();
            sw.Start();
            fun(userData);
            sw.Stop();
            Console.WriteLine($"資料筆數：{userData.Count()}\t花費時間：{sw.ElapsedMilliseconds}");
        }

        /// <summary>
        /// 逐筆Add，逐筆SaveChange
        /// </summary>
        /// <param name="ItemData"></param>
        private static void TestRun01(IEnumerable<Users> ItemData)
        {
            TSQL2019Entities dbContext = new TSQL2019Entities();

            foreach (var item in ItemData)
            {
                dbContext.Users.Add(item);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 逐筆Add，一次SaveChange
        /// </summary>
        /// <param name="ItemData"></param>
        private static void TestRun02(IEnumerable<Users> ItemData)
        {
            TSQL2019Entities dbContext = new TSQL2019Entities();

            foreach (var item in ItemData)
            {
                dbContext.Users.Add(item);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 批次Add，批次SaveChange
        /// </summary>
        /// <param name="ItemData"></param>
        private static void TestRun03(IEnumerable<Users> ItemData)
        {
            TSQL2019Entities dbContext = new TSQL2019Entities();
            var batchCount = 100;
            var i = 0;

            foreach (var item in ItemData)
            {
                dbContext.Users.Add(item);

                if (i % batchCount == 0)
                    dbContext.SaveChanges();
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 批次Add，批次SaveChange，最後關閉DBContext
        /// </summary>
        /// <param name="ItemData"></param>
        private static void TestRun04(IEnumerable<Users> ItemData)
        {
            var batchCount = 100;
            var skipCount = 0;

            var _ItemData = ItemData.ToArray();
            for (int i = 1; i <= (ItemData.Count() / batchCount); i++)
            {
                using (TSQL2019Entities dbContext = new TSQL2019Entities())
                {
                    for (int j = 0; j < batchCount; j++)
                    {
                        dbContext.Users.Add(_ItemData[j]);
                    }

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
