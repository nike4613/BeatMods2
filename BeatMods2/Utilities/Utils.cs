using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeatMods2.Utilities
{
    public static class Utils
    {
        public static unsafe byte[] BytesFromInts(ReadOnlySpan<int> ints)
        {
            var bytes = new byte[ints.Length * sizeof(int)];
            fixed (int* intptr = ints)
            fixed (byte* byteptr = bytes)
            {
                Buffer.MemoryCopy(intptr, byteptr, bytes.Length, bytes.Length);
            }
            return bytes;
        }

        public static MaybeOwningReadOnlyMemory<byte> CoerceToSize(int sizeBytes, ReadOnlyMemory<byte> input)
        {
            if (input.Length >= sizeBytes)
                return input.Slice(0, sizeBytes);
            else
            {
                var mem = MemoryPool<byte>.Shared.Rent(sizeBytes);
                input.CopyTo(mem.Memory);
                return new MaybeOwningReadOnlyMemory<byte>(mem);
            }
        }

        /// <summary>
        /// Converts a hex string to a byte array.
        /// </summary>
        /// <param name="hex">the hex stream</param>
        /// <returns>the corresponding byte array</returns>
        public static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Converts a byte array to a hex string.
        /// </summary>
        /// <param name="ba">the byte array</param>
        /// <returns>the hex form of the array</returns>
        public static string BytesToString(ReadOnlySpan<byte> ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GetCryptoRandomHexString(int size)
        {
            using var rand = new RNGCryptoServiceProvider();
            using var mem = MemoryPool<byte>.Shared.Rent(size);
            rand.GetBytes(mem.Memory.Span.Slice(0, size));
            return BytesToString(mem.Memory.Span.Slice(0, size));
        }
    }

    public readonly struct MaybeOwningReadOnlyMemory<T> : IDisposable
    {
        private readonly IMemoryOwner<T>? owner;

        public MaybeOwningReadOnlyMemory(ReadOnlyMemory<T> mem)
        {
            Memory = mem; owner = null;
        }
        public MaybeOwningReadOnlyMemory(IMemoryOwner<T> mem)
        {
            Memory = mem.Memory; owner = mem;
        }

        public ReadOnlyMemory<T> Memory { get; }

        public static implicit operator MaybeOwningReadOnlyMemory<T>(ReadOnlyMemory<T> mem)
            => new MaybeOwningReadOnlyMemory<T>(mem);
        public static implicit operator ReadOnlyMemory<T>(MaybeOwningReadOnlyMemory<T> mem)
            => mem.Memory;

        public void Dispose()
            => owner?.Dispose();

        public override bool Equals(object? obj)
            => obj is MaybeOwningReadOnlyMemory<T> morom 
            && Memory.Equals(morom.Memory) && Equals(owner, morom.owner);

        public override int GetHashCode()
            => HashCode.Combine(owner, Memory);

        public static bool operator ==(MaybeOwningReadOnlyMemory<T> left, MaybeOwningReadOnlyMemory<T> right)
            => left.Equals(right);

        public static bool operator !=(MaybeOwningReadOnlyMemory<T> left, MaybeOwningReadOnlyMemory<T> right)
            => !(left == right);
    }
}
