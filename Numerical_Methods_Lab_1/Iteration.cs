using System;

namespace Numerical_Methods_Lab_1
{
    public class Iteration
    {
        public double F(double x) => x*x*x - 6*x*x + 5*x + 12;
        private double Fp(double x) => 3*x*x - 12*x + 5;
        
        private Func<double,double> MakeG(double alpha) => (x) => x - alpha * F(x);
        
        private Func<double,double> MakeGp(double alpha) => (x) => 1.0 - alpha * Fp(x);

        private double EstimateL(Func<double,double> gp, double x0, double delta = 1.0, int samples = 100)
        {
            double a = x0 - delta, b = x0 + delta;
            if (a == b) return Math.Abs(gp(x0));
            var max = 0.0;
            for (var i = 0; i <= samples; i++)
            {
                var t = a + (b - a) * i / (double)samples;
                var val = Math.Abs(gp(t));
                if (val > max) max = val;
            }
            return max;
        }

        private int EstimateIterationsApriori(Func<double,double> g, Func<double,double> gp, double x0, double tol)
        {
            var delta = 1.0;
            var L = EstimateL(gp, x0, delta, 200);
            if (L >= 1.0)
                return -1;

            var x1 = g(x0);
            var initDiff = Math.Abs(x1 - x0);
            if (initDiff == 0.0) return 1;
            if (L == 0.0) return 1;

            var numerator = (1.0 - L) * tol / initDiff;
            if (numerator <= 0.0) return -1;

            var nReal = Math.Log(numerator) / Math.Log(L);
            var n = (int)Math.Ceiling(nReal);
            if (n < 1) n = 1;
            return n;
        }

        public (double root, int iters) SimpleIteration(double x0, double tol, double alpha = 0.0, int maxIter = 1000)
        {
            var x = x0;

            if (alpha <= 0.0)
            {
                var M = Math.Abs(Fp(x0));
                var safety = 1.0;
                alpha = 1.0 / (M + safety);
                if (alpha > 1.0) alpha = 1.0;
            }

            var g = MakeG(alpha);
            var gp = MakeGp(alpha);

            var apriori = EstimateIterationsApriori(g, gp, x0, tol);
            Console.WriteLine(apriori == -1
                ? "Апріорна оцінка: L >= 1 або не вдалося отримати гарантовану збіжність на обраному інтервалі."
                : $"Апріорна оцінка потрібної кількості ітерацій: {apriori}");

            for (var k = 1; k <= maxIter; k++)
            {
                var fx = F(x);
                var xNew = x - alpha * fx;

                Console.WriteLine($"{k,3}: x = {xNew,12:F10}    f(x) = {F(xNew),12:E6}    |dx| = {Math.Abs(xNew - x):E6}");

                if (Math.Abs(xNew - x) < tol || Math.Abs(F(xNew)) < tol)
                    return (xNew, k);

                x = xNew;
            }

            return (x, maxIter);
        }
    }
}
