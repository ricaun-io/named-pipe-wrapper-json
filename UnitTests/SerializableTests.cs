﻿using ricaun.NamedPipeWrapper;
using ricaun.NamedPipeWrapper.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UnitTests
{
    public class SerializableTests
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PipeName = "serializable_test_pipe";

        private NamedPipeServer<ClassWriteRead, ClassWriteRead> _server;
        private NamedPipeClient<ClassWriteRead, ClassWriteRead> _client;

        private ClassWriteRead _expectedData;
        private ClassWriteRead _actualData;

        private DateTime _startTime;

        private readonly ManualResetEvent _barrier = new ManualResetEvent(false);

        private readonly IList<Exception> _exceptions = new List<Exception>();


        #region Setup and teardown

        [SetUp]
        public void SetUp()
        {
            Logger.Debug("Setting up test...");

            _barrier.Reset();
            _exceptions.Clear();

            _server = new NamedPipeServer<ClassWriteRead, ClassWriteRead>(PipeName);
            _client = new NamedPipeClient<ClassWriteRead, ClassWriteRead>(PipeName);

            _expectedData = null;
            _actualData = null;

            _server.ClientMessage += ServerOnClientMessage;

            _server.Error += OnError;
            _client.Error += OnError;

            _server.Start();
            _client.Start();

            // Give the client and server a few seconds to connect before sending data
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Logger.Debug("Client and server started");
            Logger.Debug("---");

            _startTime = DateTime.Now;
        }

        private void OnError(Exception exception)
        {
            _exceptions.Add(exception);
            _barrier.Set();
        }

        [TearDown]
        public void TearDown()
        {
            Logger.Debug("---");
            Logger.Debug("Stopping client and server...");

            _server.ClientMessage -= ServerOnClientMessage;

            _server.Stop();
            _client.Stop();

            Logger.Debug("Client and server stopped");
            Logger.DebugFormat("Test took {0}", (DateTime.Now - _startTime));
            Logger.Debug("~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        #endregion

        #region Events

        private void ServerOnClientMessage(NamedPipeConnection<ClassWriteRead, ClassWriteRead> connection, ClassWriteRead message)
        {
            Logger.DebugFormat("Received {0} bytes from the client", message.ToString().Length);
            _actualData = message;
            _barrier.Set();
        }

        #endregion

        [Serializable]
        public class ClassWriteRead
        {
            public List<int> List { get; set; }
            public string Text { get; set; }
            public int MyProperty { get; set; }
            public double Number { get; set; }
            public string NullText { get; set; }
            public override bool Equals(object obj)
            {
                if (obj is ClassWriteRead c)
                    return MyProperty == c.MyProperty;
                return false;
            }
            public override int GetHashCode()
            {
                return MyProperty;
            }
            public override string ToString()
            {
                return JsonUtils.Serialize(this);
            }
        }


        [Test]
        public void TestCircularReferences()
        {
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                TestClass(new ClassWriteRead()
                {
                    MyProperty = random.Next(),
                    List = new List<int>(Enumerable.Range(0, i)),
                    Text = "1234567890"
                });
            }
        }

        [Test]
        public void TestCircularReferences1E4()
        {
            var random = new Random();
            TestClass(new ClassWriteRead()
            {
                MyProperty = random.Next(),
                List = new List<int>(Enumerable.Range(0, 1000)),
                Text = "1234567890"
            });
        }

        [Test]
        public void TestCircularReferences1E9()
        {
            var random = new Random();
            TestClass(new ClassWriteRead()
            {
                MyProperty = random.Next(),
                List = new List<int>(Enumerable.Range(0, 10000000)),
                Text = "1234567890"
            });
        }

        [Test]
        public void TestPipeExists()
        {
            Assert.IsTrue(NamedPipeUtils.PipeFileExists(PipeName), string.Format("PipeFile should Exist"));
        }

        private void TestClass(ClassWriteRead _expectedValue)
        {
            _expectedData = _expectedValue;

            _barrier.Reset();
            _client.PushMessage(_expectedData);
            _barrier.WaitOne(TimeSpan.FromSeconds(30));

            if (_exceptions.Any())
                throw new AggregateException(_exceptions);

            //Console.WriteLine($"{_expectedData} = {_actualData}");

            Assert.AreEqual(_expectedData, _actualData, string.Format("Data should be equal"));

            var _actualValue = _actualData;

            //Console.WriteLine($"{_expectedValue.MyProperty} = {_actualValue.MyProperty}");

            Assert.AreEqual(_expectedValue, _actualValue, string.Format("Data should be equal"));
            //Assert.AreEqual(_expectedValue.ToString(), _actualValue.ToString(), string.Format("Data should be equal"));
        }
    }
}
