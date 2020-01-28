using System;
using System.Collections.Generic;
using Infrastructure;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Scripting
{
    public class ExtensionTypeActivatorTests
    {
        public class NoConstructor
        {
        }

        public class Constructor1
        {
            public Constructor1(HashSet<string> strings){}
        }

        public class Constructor2
        {
            public Constructor2(HashSet<string> stringsHash, Queue<string> stringsQueue){}
        }
        
        private readonly IExtensionTypeActivator _subject = new ExtensionTypeActivator(TestObjects.Logger);
        
        [Fact]
        public void CreateInstance_Returns_Instance_For_Default_Constructor()
        {
            _subject.CreateInstance(typeof(NoConstructor), new Dictionary<Type, object>()).ShouldNotBeNull();
        }


        [Fact]
        public void CreateInstance_Returns_Instance_For_Parameterized_Constructor()
        {
            _subject.CreateInstance(typeof(Constructor1), new Dictionary<Type, object>
            {
                [typeof(HashSet<string>)] = new HashSet<string>()
            }).ShouldNotBeNull();
        }

        [Fact]
        public void CreateInstance_Returns_Instance_For_MultiParameter_Constructor()
        {
            _subject.CreateInstance(typeof(Constructor2), new Dictionary<Type, object>
            {
                [typeof(HashSet<string>)] = new HashSet<string>(),
                [typeof(Queue<string>)] = new Queue<string>()
            }).ShouldNotBeNull();
        }

        [Fact]
        public void CreateInstance_Throws_For_Incompatible_Constructor()
        {
            Should.Throw<NotSupportedException>(() =>
                {
                    _subject.CreateInstance(typeof(Constructor1), new Dictionary<Type, object>());
                });
        }
    }
}