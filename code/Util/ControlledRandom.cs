using System;
using Sandbox;

namespace Extraction.Util
{
	/// <summary>
	/// This sounds dumb, but it's a decent way to ensure that the same number is never picked twice.
	/// Like Apple's "random" shuffle thing. It makes it feel more random by being less random.
	/// </summary>
	public class ControlledRandom
	{
		public int lastValue = 0;
		
		public T FromArray<T>(T[] array)
		{
			if ( array == null || array.Length < 2 )
				return default;

			int index;
			do
			{
				index = Rand.Int( 0, array.Length - 1 );
			} while ( index == lastValue );

			lastValue = index;
			
			return array[index];
		}
	}
}
