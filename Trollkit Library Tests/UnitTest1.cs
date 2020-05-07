using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trollkit_Library;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Library_Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			string testText = "adasdsada\0\0\0\0\0\0\0\0";

			TransferCommandObject t = new TransferCommandObject { Command = "Login", Value = testText };


			var t2 = ClientServerPipeline.BufferDeserialize(ClientServerPipeline.BufferSerialize(t));

			Assert.AreEqual(t.Value, t2.Value);
		}
	}
}
