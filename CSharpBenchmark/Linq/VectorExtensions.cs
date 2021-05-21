using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CSharpBenchmark.Linq
{
    public static class VectorExtensions
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe Vector256<int> JoinMask(this Vector256<long> l1, Vector256<long> l2)
		{
			return Vector256.WithUpper(
				Vector256.WithLower(
					Vector256<int>.Zero, AsInt(l1)
				),
				AsInt(l2)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe Vector256<byte> JoinMask(Vector128<byte> l1, Vector128<byte> l2)
		{
			return Vector256.WithUpper(
				Vector256.WithLower(
					Vector256<byte>.Zero, l1
				),
				l2
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static Vector256<byte> Reverse(this Vector256<byte> source)
		{
			var shuffleMask = stackalloc byte[] {
    			15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0
			};
			var shuffleMaskVector = Avx2.LoadVector128(shuffleMask);
			return JoinMask(
				Avx2.Shuffle(source.GetUpper(), shuffleMaskVector),
				Avx2.Shuffle(source.GetLower(), shuffleMaskVector)
			);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
			return Avx.LoadVector128(content + 2);
		}
	}
}
