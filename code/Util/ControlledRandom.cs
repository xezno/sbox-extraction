using System;
using Sandbox;

namespace Extraction.Util
{
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
