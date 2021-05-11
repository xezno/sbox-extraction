// Code by Ognik (https://github.com/ogniK5377)
using Sandbox;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Extraction.Util
{
	[StructLayout( LayoutKind.Explicit )]
	public struct TypeUnpackFloat
	{
		[FieldOffset( 0 )]
		public uint Raw;

		[FieldOffset( 0 )]
		public float Base;
	}
	[StructLayout( LayoutKind.Explicit )]
	public struct TypeUnpackDouble
	{
		[FieldOffset( 0 )]
		public ulong Raw;

		[FieldOffset( 0 )]
		public double Base;
	}

	public class NetworkWriter : IDisposable
	{
		private List<byte> WriteBuffer = new();
		private byte CurrentByte = 0;
		private int BitPosition = 0;
		internal NetWrite Write;
		private bool disposedValue;

		public NetworkWriter( NetWrite Write )
		{
			this.Write = Write;
		}

		private void WriteToBuffer( byte b, int bitCount )
		{
			int remaining = bitCount;
			while ( remaining > 0 )
			{
				int remainingInBuffer = 8 - BitPosition;
				int bitsToCopy = Math.Min( remainingInBuffer, remaining );
				int mask = (1 << bitsToCopy) - 1;
				CurrentByte |= (byte)((b & mask) << BitPosition);

				b >>= bitsToCopy;
				BitPosition += bitsToCopy;
				remaining -= bitsToCopy;

				if ( BitPosition >= 8 )
				{
					WriteBuffer.Add( CurrentByte );
					BitPosition = 0;
					CurrentByte = 0;
				}

			}
		}

		public void WriteByte( byte value )
		{
			WriteToBuffer( value, 8 );
		}

		public void WriteSByte( sbyte value )
		{
			WriteToBuffer( (byte)value, 8 );
		}

		public void WriteUShort( ushort value )
		{
			WriteToBuffer( (byte)(value & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 8) & 0xff), 8 );
		}

		public void WriteShort( short value )
		{
			WriteToBuffer( (byte)(value & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 8) & 0xff), 8 );
		}

		public void WriteUInt( uint value )
		{
			WriteToBuffer( (byte)(value & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 8) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 16) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 24) & 0xff), 8 );
		}

		public void WriteInt( int value )
		{
			WriteToBuffer( (byte)(value & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 8) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 16) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 24) & 0xff), 8 );
		}
		public void WriteULong( ulong value )
		{
			WriteToBuffer( (byte)(value & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 8) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 16) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 24) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 32) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 40) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 48) & 0xff), 8 );
			WriteToBuffer( (byte)((value >> 56) & 0xff), 8 );
		}

		public void WriteFloat( float value )
		{
			TypeUnpackFloat typeUnpack = new TypeUnpackFloat { Base = value };
			WriteUInt( typeUnpack.Raw );
		}

		public void WriteDouble( double value )
		{
			TypeUnpackDouble typeUnpack = new TypeUnpackDouble { Base = value };
			WriteULong( typeUnpack.Raw );
		}

		public void WriteBits( ulong value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( long value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( int value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( uint value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( short value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( ushort value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( byte value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBits( sbyte value, int BitCount )
		{
			int remaining = BitCount;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				WriteToBuffer( (byte)(value & 0xff), copyAmount );
				value >>= copyAmount;
				remaining -= copyAmount;
			}
		}

		public void WriteBit( bool value )
		{
			WriteToBuffer( (byte)(value ? 1 : 0), 1 );
		}

		public void WriteULeb128( ulong value )
		{
			int bitCount = 0;
			ulong x = 0;
			ulong remaining = value;
			int i = 0;
			while ( remaining > 0 )
			{
				bitCount += 8;
				x |= (remaining & 0x7f) << (i * 8);
				remaining >>= 7;
				x |= (remaining != 0 ? 1ul : 0ul) << ((i * 8) + 7);
				i++;
			}

			WriteBits( x, bitCount );
		}

		public void WriteULeb128( uint value )
		{
			WriteULeb128( (ulong)value );
		}

		public void WriteULeb128( ushort value )
		{
			WriteULeb128( (ulong)value );
		}

		public void WriteSLeb128( long value )
		{
			int shift = 0;
			long x = 0;
			bool isNegative = value < 0;
			bool isEncoding = true;
			while ( isEncoding )
			{
				long n = value & 0x7f;
				value >>= 7;

				/// Sign extend
				if ( isNegative )
				{
					value |= -(1L << 57);
				}

				bool signBit = (n & 0x40) != 0;
				if ( (value == 0 && !signBit) || (value == -1 && signBit) )
				{
					isEncoding = false;
				}
				else
				{
					n |= 0x80;
				}

				x |= n << shift;
				shift += 8;
			}

			WriteBits( x, shift );
		}

		public void WriteSLeb128( int value )
		{
			WriteSLeb128( (long)value );
		}

		public void WriteSLeb128( short value )
		{
			WriteSLeb128( (long)value );
		}

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue )
			{
				if ( disposing )
				{
					// Flush buffer
					if ( BitPosition != 0 )
					{
						WriteBuffer.Add( CurrentByte );
					}
					foreach ( byte x in WriteBuffer )
					{
						Write.Write( x );
					}
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose( disposing: true );
		}
	}

	public class NetworkReader : IDisposable
	{
		private byte CurrentByte = 0;
		private int BitPosition = 0;
		private int TotalByteCount = 0;
		private int CurrentBytePosition = 0;
		internal NetRead Reader;
		private bool disposedValue;

		public NetworkReader( NetRead Reader )
		{
			this.Reader = Reader;
			TotalByteCount = Reader.Remaining;
			CurrentBytePosition = 0;
			if ( Reader.Remaining > 0 )
			{
				CurrentByte = Reader.Read<byte>();
			}
		}

		private byte ReadFromBuffer( int bitCount )
		{
			int currentPos = 0;
			byte current = 0;
			int remaining = bitCount;
			while ( remaining > 0 )
			{
				int remainingInBuffer = 8 - BitPosition;
				int bitsToRead = Math.Min( remainingInBuffer, remaining );
				int mask = (1 << bitsToRead) - 1;

				byte buf = (byte)(CurrentByte & mask);
				CurrentByte >>= bitsToRead;
				BitPosition += bitsToRead;
				remaining -= bitsToRead;
				current |= (byte)(buf << currentPos);
				currentPos += bitsToRead;

				if ( BitPosition >= 8 )
				{
					CurrentBytePosition++;
					if ( CurrentBytePosition < TotalByteCount )
					{
						CurrentByte = Reader.Read<byte>();
					}
					else
					{
						throw new Exception();
					}
					BitPosition = 0;
				}
			}
			return current;
		}

		public byte ReadByte()
		{
			return ReadFromBuffer( 8 );
		}

		public sbyte ReadSByte()
		{
			return (sbyte)ReadFromBuffer( 8 );
		}

		public ushort ReadUShort()
		{
			return (ushort)(ReadFromBuffer( 8 ) | (ReadFromBuffer( 8 ) << 8));
		}

		public short ReadShort()
		{
			return (short)(ReadFromBuffer( 8 ) | (ReadFromBuffer( 8 ) << 8));
		}

		public uint ReadUInt()
		{
			return ((uint)ReadFromBuffer( 8 ) | ((uint)ReadFromBuffer( 8 ) << 8) | ((uint)ReadFromBuffer( 8 ) << 16) | ((uint)ReadFromBuffer( 8 ) << 24));
		}

		public int ReadInt()
		{
			return ((int)ReadFromBuffer( 8 ) | ((int)ReadFromBuffer( 8 ) << 8) | ((int)ReadFromBuffer( 8 ) << 16) | ((int)ReadFromBuffer( 8 ) << 24));
		}

		public ulong ReadULong()
		{
			return (ulong)((ulong)ReadFromBuffer( 8 ) | ((ulong)ReadFromBuffer( 8 ) << 8) | ((ulong)ReadFromBuffer( 8 ) << 16) | ((ulong)ReadFromBuffer( 8 ) << 24) | ((ulong)ReadFromBuffer( 8 ) << 32) | ((ulong)ReadFromBuffer( 8 ) << 40) | ((ulong)ReadFromBuffer( 8 ) << 48) | ((ulong)ReadFromBuffer( 8 ) << 56));
		}

		public long ReadLong()
		{
			return ((long)ReadFromBuffer( 8 ) | ((long)ReadFromBuffer( 8 ) << 8) | ((long)ReadFromBuffer( 8 ) << 16) | ((long)ReadFromBuffer( 8 ) << 24) | ((long)ReadFromBuffer( 8 ) << 32) | ((long)ReadFromBuffer( 8 ) << 40) | ((long)ReadFromBuffer( 8 ) << 48) | ((long)ReadFromBuffer( 8 ) << 56));
		}

		public float ReadFloat()
		{
			return new TypeUnpackFloat { Raw = ReadUInt() }.Base;
		}

		public double ReadDouble()
		{
			return new TypeUnpackDouble { Raw = ReadULong() }.Base;
		}

		public ulong ReadULeb128()
		{
			ulong x = 0;
			int shift = 0;

			while ( true )
			{
				x |= (ulong)ReadFromBuffer( 7 ) << shift;
				shift += 7;
				if ( !ReadBit() )
				{
					break;
				}
			}
			return x;
		}

		public long ReadSLeb128()
		{
			byte curByte = 0;
			long x = 0;
			int shift = 0;

			while ( true )
			{
				curByte = ReadFromBuffer( 7 );
				x |= (long)curByte << shift;
				shift += 7;
				if ( !ReadBit() )
				{
					break;
				}
			}

			if ( shift < 64 && ((curByte & 0x40) != 0) )
			{
				x |= -(1L << shift);
			}

			return x;
		}

		public byte ReadBitsByte( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			byte value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= (byte)(ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public sbyte ReadBitsSByte( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			sbyte value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= (sbyte)(ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public ushort ReadBitsUShort( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			ushort value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= (ushort)(ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public short ReadBitsShort( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			short value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= (short)(ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public uint ReadBitsUInt( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			uint value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= ((uint)ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public int ReadBitsInt( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			int value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= ((int)ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public ulong ReadBitsULong( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			ulong value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= ((ulong)ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public long ReadBitsLong( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			long value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= ((long)ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public byte ReadBits( int bitCount )
		{
			int remaining = bitCount;
			int offset = 0;
			byte value = 0;
			while ( remaining > 0 )
			{
				int copyAmount = Math.Min( 8, remaining );
				value |= (byte)(ReadFromBuffer( copyAmount ) << offset);
				offset += copyAmount;
				remaining -= copyAmount;
			}
			return value;
		}

		public bool ReadBit()
		{
			return ReadFromBuffer( 1 ) == 1;
		}


		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue )
			{
				if ( disposing )
				{
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose( disposing: true );
		}
	}
}
