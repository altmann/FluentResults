using FluentAssertions;
using FluentResults.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentResults.Test
{
    public class CovarianceTests
    {
        public interface IInterfaceA { }
        public interface IInterfaceB : IInterfaceA { }
        public sealed class ClassA : IInterfaceA { }
        public sealed class ClassB : IInterfaceB { }

        [Fact]
        public void Result_of_ClassA_IsNot_Result_of_IInterfaceB()
        {
            IResult<IInterfaceA> result = Result.Create<IInterfaceA>(new ClassA());

            Assert.True(result is IResult<IInterfaceA>);
            Assert.True(result is IResult<ClassA>);
            Assert.False(result is IResult<IInterfaceB>);
            Assert.False(result is IResult<ClassB>);
        }
        [Fact]
        public void Result_of_ClassB_IsAlso_Result_of_IInterfaceA()
        {
            IResult<IInterfaceA> result = Result.Create<IInterfaceA>(new ClassB());

            Assert.True(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<ClassA>);
            Assert.True(result is IResult<IInterfaceB>);
            Assert.True(result is IResult<ClassB>);
        }

        [Fact]
        public void WithValue_NewClassA_Correctly_Creates_Covariance()
        {
            IResult<IInterfaceA> result = Result.Create<IInterfaceA>();

            Assert.True(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<IInterfaceB>);
            Assert.False(result is IResult<ClassA>);
            Assert.False(result is IResult<ClassB>);

            result = result.WithValue(new ClassA());

            Assert.True(result is IResult<IInterfaceA>);
            Assert.True(result is IResult<ClassA>);
            Assert.False(result is IResult<IInterfaceB>);
            Assert.False(result is IResult<ClassB>);
        }

        [Fact]
        public void WithValue_NewClassB_Correctly_Creates_Covariance()
        {
            IResult<IInterfaceA> result = Result.Create<IInterfaceA>();

            Assert.True(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<IInterfaceB>);
            Assert.False(result is IResult<ClassA>);
            Assert.False(result is IResult<ClassB>);

            result = result.WithValue(new ClassB());

            Assert.True(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<ClassA>);
            Assert.True(result is IResult<IInterfaceB>);
            Assert.True(result is IResult<ClassB>);
        }

        [Fact]
        public void IResultBase_WithValue()
        {
            IResultBase result = Result.Create();

            Assert.False(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<IInterfaceB>);
            Assert.False(result is IResult<ClassA>);
            Assert.False(result is IResult<ClassB>);

            result = result.WithValue(new ClassB());

            Assert.True(result is IResult<IInterfaceA>);
            Assert.False(result is IResult<ClassA>);
            Assert.True(result is IResult<IInterfaceB>);
            Assert.True(result is IResult<ClassB>);
        }
    }
}
