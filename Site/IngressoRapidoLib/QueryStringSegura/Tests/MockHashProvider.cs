#if UNIT_TESTS
using System;

namespace IngressoRapido.QueryStringSegura.Tests
{
	public class MockHashProvider : IHashProvider
	{
        public byte[] Hash(byte[] buffer)
        {
            return null;
        }
    }
}
#endif