using System;

namespace conconcon_tri_buter
{
    public class Math
    {
        /// <summary>
        /// 正态分布随机数
        /// </summary>
        const int N = 100;
        const int MAX = 50;
        const double MIN = 0.1;
        const int MIU = 40;
        const int SIGMA = 1;
        static Random aa = new Random((int)(DateTime.Now.Ticks / 10000));

        /// <summary>
        /// 产生(min,max)之间均匀分布的随机数
        /// </summary>
        /// <param name="min">最小阈值</param>
        /// <param name="max">最大阈值</param>
        /// <returns>随机数</returns>
        public double AverageRandom(double min, double max)
        {
            int MINnteger = (int)(min * 10000);
            int MAXnteger = (int)(max * 10000);
            int resultInteger = aa.Next(MINnteger, MAXnteger);
            return resultInteger / 10000.0;
        }

        /// <summary>
        /// 正态分布概率密度函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="miu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public double Normal(double x, double miu, double sigma) =>
            1.0 / (x * System.Math.Sqrt(2 * System.Math.PI) * sigma) * System.Math.Exp(-1 * (System.Math
                .Log(x) - miu) * (System.Math.Log(x) - miu) / (2 * sigma * sigma));

        public double Random_Normal(double miu, double sigma, double min, double max)//产生正态分布随机数
        {
            double x, dScope, y;
            do
            {
                x = AverageRandom(min, max);
                y = Normal(x, miu, sigma);
                dScope = AverageRandom(0, Normal(miu, miu, sigma));
            } while (dScope > y);
            return x;
        }

        /// <summary>
        /// 指数分布随机数
        /// const_a是指数分布的参数λ
        /// </summary>
        /// <param name="const_a"></param>
        /// <returns></returns>
        public double RandExp(double const_a)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            double p, temp;
            if (const_a != 0) temp = 1 / const_a;
            else throw new InvalidOperationException("除数不能为零！不能产生参数为零的指数分布！");
            double randres;
            while (true) //用于产生随机的密度，保证比参数λ小
            {
                p = rand.NextDouble();
                if (p < const_a) break;
            }
            randres = -temp * System.Math.Log(temp * p, System.Math.E);
            return randres;
        }

        /// <summary>
        /// 负指数分布随机数产生
        /// </summary>
        /// <param name="lam">参数</param>
        /// <returns></returns>
        Random ran = new Random();

        public double ngtIndex(double lam)
        {
            double dec = ran.NextDouble();
            while (dec == 0) dec = ran.NextDouble();
            return -System.Math.Log(dec) / lam;
        }

        /// <summary>
        /// 泊松分布产生
        /// </summary>
        /// <param name="lam">参数</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public double poisson(double lam, double time)
        {
            int count = 0;
            while (true)
            {
                time -= ngtIndex(lam);
                if (time > 0) count++;
                else break;
            }
            return count;
        }
    }
}
