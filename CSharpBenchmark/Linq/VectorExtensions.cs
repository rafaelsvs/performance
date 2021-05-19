using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CSharpBenchmark.Linq
{
    public static class VectorExtensions
    {
		public static unsafe Vector256<int> JoinMask(this Vector256<long> l1, Vector256<long> l2)
		{
			var r =  Vector256.WithUpper(
				Vector256.WithLower(
					new Vector256<int>(), l1.AsInt()
				),
				l2.AsInt()
			);
			return r;
		}

		public static unsafe Vector128<int> AsInt(this Vector256<long> l)
		{
			// (0, 1, 0, 2, 0, 3, 0, 4) -> (1, 2, 1, 2, 3, 4, 3, 4) 
			var v = Avx2.Shuffle(
				l.AsInt32(),
				136
			);
			var content = stackalloc int[8];
			Avx2.Store(content, v);
			// (1, 2, 1, 2, 3, 4, 3, 4)  -> (1, 2, 3, 4) 
			return Avx.LoadDquVector128(content + 2);
		}
	}
}
