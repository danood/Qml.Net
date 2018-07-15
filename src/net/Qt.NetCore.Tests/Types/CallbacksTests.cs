﻿using System;
using System.Runtime.InteropServices;
using AdvancedDLSupport;
using FluentAssertions;
using Moq;
using Qt.NetCore.Qml;
using Qt.NetCore.Types;
using Xunit;

namespace Qt.NetCore.Tests.Types
{
    public class CallbacksTests : BaseTests
    {
        [Fact]
        public void Can_call_is_type_valid()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                callbacks.Setup(x => x.IsTypeValid("test-type")).Returns(true);

                Interop.RegisterCallbacks(callbacks.Object);
                Interop.Callbacks.IsTypeValid("test-type").Should().BeTrue();

                callbacks.Verify(x => x.IsTypeValid("test-type"), Times.Once);
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }

        [Fact]
        public void Can_build_type_info()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                NetTypeInfo typeInfo = null;
                callbacks.Setup(x => x.BuildTypeInfo(It.IsAny<NetTypeInfo>()))
                    .Callback(new Action<NetTypeInfo>(x => typeInfo = x));

                Interop.RegisterCallbacks(callbacks.Object);
                Interop.Callbacks.BuildTypeInfo(new NetTypeInfo("test").Handle);

                callbacks.Verify(x => x.BuildTypeInfo(It.IsAny<NetTypeInfo>()), Times.Once);
                typeInfo.FullTypeName.Should().Be("test");
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }

        [Fact]
        public void Can_release_gc_handle()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                IntPtr handle = IntPtr.Zero;
                callbacks.Setup(x => x.ReleaseGCHandle(It.IsAny<IntPtr>()))
                    .Callback(new Action<IntPtr>(x => handle = x));

                Interop.RegisterCallbacks(callbacks.Object);
                Interop.Callbacks.ReleaseGCHandle(new IntPtr(3));

                callbacks.Verify(x => x.ReleaseGCHandle(It.IsAny<IntPtr>()), Times.Once);
                handle.Should().Be(new IntPtr(3));
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }
        
        [Fact]
        public void Can_instantiate_type()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                string typeName = null;
                callbacks.Setup(x => x.InstantiateType(It.IsAny<string>()))
                    .Callback(new Action<string>(x => typeName = x))
                    .Returns((GCHandle)new IntPtr(3));

                Interop.RegisterCallbacks(callbacks.Object);
                var result = Interop.Callbacks.InstantiateType("test");

                callbacks.Verify(x => x.InstantiateType(It.IsAny<string>()), Times.Once);
                typeName.Should().Be("test");
                ((IntPtr)result).Should().Be(new IntPtr(3));
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }
        
        [Fact]
        public void Can_read_property()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                var property = IntPtr.Zero;
                var instance = IntPtr.Zero;;
                var result = IntPtr.Zero;;
                callbacks.Setup(x =>
                        x.ReadProperty(It.IsAny<NetPropertyInfo>(), It.IsAny<NetInstance>(), It.IsAny<NetVariant>()))
                    .Callback(new Action<NetPropertyInfo, NetInstance, NetVariant>((p, i, r) =>
                    {
                        property = p.Handle;
                        instance = i.Handle;
                        result = r.Handle;
                    }));

                Interop.RegisterCallbacks(callbacks.Object);
                Interop.Callbacks.ReadProperty(new IntPtr(1), new IntPtr(2), new IntPtr(3));

                callbacks.Verify(x => x.ReadProperty(It.IsAny<NetPropertyInfo>(), It.IsAny<NetInstance>(), It.IsAny<NetVariant>()), Times.Once);
                property.Should().Be(new IntPtr(1));
                instance.Should().Be(new IntPtr(2));
                result.Should().Be(new IntPtr(3));
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }
        
        [Fact]
        public void Can_write_property()
        {
            try
            {
                var callbacks = new Mock<ICallbacks>();
                var property = IntPtr.Zero;
                var instance = IntPtr.Zero;;
                var value = IntPtr.Zero;;
                callbacks.Setup(x =>
                        x.WriteProperty(It.IsAny<NetPropertyInfo>(), It.IsAny<NetInstance>(), It.IsAny<NetVariant>()))
                    .Callback(new Action<NetPropertyInfo, NetInstance, NetVariant>((p, i, v) =>
                    {
                        property = p.Handle;
                        instance = i.Handle;
                        value = v.Handle;
                    }));

                Interop.RegisterCallbacks(callbacks.Object);
                Interop.Callbacks.WriteProperty(new IntPtr(1), new IntPtr(2), new IntPtr(3));

                callbacks.Verify(x => x.WriteProperty(It.IsAny<NetPropertyInfo>(), It.IsAny<NetInstance>(), It.IsAny<NetVariant>()), Times.Once);
                property.Should().Be(new IntPtr(1));
                instance.Should().Be(new IntPtr(2));
                value.Should().Be(new IntPtr(3));
            }
            finally
            {
                Interop.SetDefaultCallbacks();
            }
        }
    }
}